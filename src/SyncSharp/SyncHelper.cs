using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SyncSharp.Commons.DbSource;
using SyncSharp.Commons.Extensions;

namespace SyncSharp;

/// <summary>
/// database sync class
/// </summary>
/// <typeparam name="TDbContext"></typeparam>
public class SyncHelper<TDbContext> where TDbContext : DbContext
{
    private readonly DbSourceBase _receiverSource;
    private readonly DbSourceBase _senderSource;

    private readonly List<Type> _syncTypes = new();

    public SyncHelper(DbSourceBase senderSource, DbSourceBase receiverSource)
    {
        _senderSource = senderSource;
        _receiverSource = receiverSource;
    }

    /// <summary>
    /// Adds sync type to Db sync
    /// </summary>
    /// <typeparam name="TEntity">Sync type</typeparam>
    public void AddSync<TEntity>()
    {
        _syncTypes.Add(typeof(TEntity));
    }

    /// <summary>
    /// Adds the sync model's types
    /// </summary>
    /// <remarks></remarks>
    /// <param name="types"></param>
    public void AddRangeSyncTypes(IEnumerable<Type> types)
    {
        _syncTypes.AddRange(types);
    }

    /// <summary>
    /// start sync databases
    /// </summary>
    /// <exception cref="ArgumentException">when database don't set the model</exception>
    public async void StartSync()
    {
        await using var senderContext = CreateDbContextInstance(_senderSource, _syncTypes.ToArray());
        await using var receiverContext = CreateDbContextInstance(_receiverSource, _syncTypes.ToArray());

        await Seed(senderContext, receiverContext);

        foreach (var syncType in _syncTypes)
        {
            var distinctObjects = DistinctFrom(senderContext, receiverContext, syncType);

            await receiverContext.AddRangeAsync(distinctObjects);
        }

        await receiverContext.SaveChangesAsync();
    }

    public async void StartSync(DbContext senderContext, DbContext receiverContext)
    {
        await Seed(senderContext, receiverContext);

        foreach (var syncType in _syncTypes)
        {
            var distinctObjects = await DistinctFrom(senderContext, receiverContext, syncType);

            await receiverContext.AddRangeAsync(distinctObjects);
            await receiverContext.SaveChangesAsync();
        }
    }

    private async Task Seed(DbContext senderContext, DbContext receiverContext)
    {
        await senderContext.AddRangeAsync(_senderSource.Seed);
        await senderContext.SaveChangesAsync();

        await receiverContext.AddRangeAsync(_receiverSource.Seed);
        await receiverContext.SaveChangesAsync();
    }

    private static DbContext CreateDbContextInstance(DbSourceBase source, Type[] dbSetTypes)
    {
        var senderDbContextObject =
            Activator.CreateInstance(typeof(TDbContext), source.Build<TDbContext>(), dbSetTypes);

        if (senderDbContextObject == null)
        {
            throw new ArgumentException(
                $"{nameof(TDbContext)} don't have matched one DbContextOption parameters constructor");
        }

        return (TDbContext)senderDbContextObject;
    }

    private static async Task<IEnumerable<object>> DistinctFrom(
        DbContext senderDbContextBase, DbContext receiverDbContext, Type syncType)
    {
        var senderTypeSet = senderDbContextBase.Set(syncType);
        var receiverTypeSet = receiverDbContext.Set(syncType);

        var distinctMethod = GetDistinctMethodInfo();
        var invoke = distinctMethod.Invoke(null, new object[]
        {
            await senderTypeSet.Cast<object>().ToListAsync(),
            await receiverTypeSet.Cast<object>().ToListAsync()
        });
        Debug.Assert(invoke != null, nameof(invoke) + " != null");

        return (IEnumerable<object>)invoke;
    }

    private static MethodInfo GetDistinctMethodInfo()
    {
        var distinctMethodInfo = typeof(DbContextExtensions).GetMethod(nameof(DbContextExtensions.DistinctFrom));

        Debug.Assert(distinctMethodInfo != null, nameof(distinctMethodInfo) + " != null");

        return distinctMethodInfo;
    }
}