using System;
using Microsoft.EntityFrameworkCore;

namespace SyncSharp.Commons.DbSource;

public class InMemoryDbSource : DbSourceBase
{
    internal override DbContextOptions<TDbContext> Build<TDbContext>()
    {
        return new DbContextOptionsBuilder<TDbContext>()
            .UseInMemoryDatabase(DatabaseName ?? throw new InvalidOperationException("DataBase name missing"))
            .Options;
    }
}