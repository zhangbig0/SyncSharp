using System;
using Microsoft.EntityFrameworkCore;

namespace SyncSharp.Commons.Infrastructure;

internal sealed class InMemoryDbContext : SyncDbContextBase
{
    internal InMemoryDbContext(DbContextOptions<InMemoryDbContext> options, Type[] syncTypes)
        : base(options, syncTypes)
    {
    }
}