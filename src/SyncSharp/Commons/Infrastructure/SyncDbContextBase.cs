using System;
using Microsoft.EntityFrameworkCore;

namespace SyncSharp.Commons.Infrastructure;

internal class SyncDbContextBase : DbContext
{
    private readonly Type[] _syncTypes;

    protected SyncDbContextBase(DbContextOptions options, Type[] syncTypes) : base(options)
    {
        _syncTypes = syncTypes;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var syncType in _syncTypes)
        {
            modelBuilder.Model.AddEntityType(syncType);
        }

        base.OnModelCreating(modelBuilder);
    }
}