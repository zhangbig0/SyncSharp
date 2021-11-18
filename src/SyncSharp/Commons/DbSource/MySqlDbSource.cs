using System;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace SyncSharp.Commons.DbSource;

public class MySqlDbSource : DbSourceBase
{
    private ServerVersion ServerVersion => ServerVersion.AutoDetect(ConnectionString);

    public override string ConnectionString =>
        new MySqlConnectionStringBuilder
        {
            Password = Password,
            UserID = User,
            Database = DatabaseName,
            Port = uint.Parse(Port ?? throw new InvalidOperationException("port not formatted to the uint")),
            Server = Ip,
        }.ToString();


    internal override DbContextOptions<TDbContext> Build<TDbContext>()
    {
        return new DbContextOptionsBuilder<TDbContext>()
            .UseMySql(ConnectionString, ServerVersion)
            .Options;
    }
}