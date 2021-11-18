using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SyncSharp.Commons.DbSource;

public abstract class DbSourceBase
{
    public string? DatabaseName;
    public string? Ip;
    public string? Password;
    public string? Port;

    public string? User;

    protected DbSourceBase(string? ip, string? port, string? user, string? password, string? databaseName)
    {
        Ip = ip;
        Port = port;
        User = user;
        Password = password;
        DatabaseName = databaseName;
    }

    protected DbSourceBase()
    {
    }

    public IEnumerable<object> Seed { get; init; } = new List<object>();

    public virtual string ConnectionString =>
        $"server={Ip}:{Port};user={User};password={Password};database={DatabaseName}";

    internal abstract DbContextOptions<TDbContext> Build<TDbContext>() where TDbContext : DbContext;
}