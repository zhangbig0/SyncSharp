using System;
using Microsoft.EntityFrameworkCore;

namespace SyncSharp.Commons.Infrastructure;

internal class SqlServerSyncDbContext : SyncDbContextBase
{
    internal SqlServerSyncDbContext(DbContextOptions<SqlServerSyncDbContext> options, Type[] syncTypes)
        : base(options, syncTypes)
    {
    }
}