using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SyncSharp.Commons.Extensions;

public static class DbContextExtensions
{
    internal static IQueryable Set(this DbContext context, Type type)
    {
        var methodInfo = typeof(DbContext).GetMethod(nameof(DbContext.Set), Type.EmptyTypes);

        Debug.Assert(methodInfo != null);

        var dbSet = methodInfo.MakeGenericMethod(type).Invoke(context, null);

        if (dbSet == null)
        {
            throw new ArgumentNullException($"{type.FullName} is missing in {nameof(context)}");
        }

        return (IQueryable)dbSet;
    }

    public static IEnumerable<object> DistinctFrom(this IEnumerable<object> sender, IEnumerable<object> receiver)
    {
        return sender.Where(s => !receiver.Any(r => r.Equals(s)));
    }
}