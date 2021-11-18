using System;
using Microsoft.EntityFrameworkCore;

namespace SyncSharp.Commons.Infrastructure;

internal class SqliteSyncDbContext : SyncDbContextBase
{
    internal SqliteSyncDbContext(DbContextOptions<SqliteSyncDbContext> options, Type[] syncTypes)
        : base(options, syncTypes)
    {
    }
}