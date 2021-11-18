using System;
using Microsoft.EntityFrameworkCore;

namespace SyncSharp.Commons.Infrastructure;

internal sealed class MySqlSyncDbContext : SyncDbContextBase
{
    internal MySqlSyncDbContext(DbContextOptions<MySqlSyncDbContext> options, Type[] syncTypes)
        : base(options, syncTypes)
    {
    }
}