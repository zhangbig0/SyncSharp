using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace SyncSharp.Commons.DbSource;

public class SqliteDbSource : DbSourceBase
{
    public string? Path { get; set; }

    public override string ConnectionString =>
        new SqliteConnectionStringBuilder
        {
            DataSource = Path ?? throw new ArgumentException("missing sqlite database file", nameof(Path)),
            Mode = SqliteOpenMode.ReadWriteCreate,
            Password = Password,
            Cache = SqliteCacheMode.Shared
        }.ToString();

    internal override DbContextOptions<TDbContext> Build<TDbContext>()
    {
        return new DbContextOptionsBuilder<TDbContext>()
            .UseSqlite(ConnectionString)
            .Options;
    }
}