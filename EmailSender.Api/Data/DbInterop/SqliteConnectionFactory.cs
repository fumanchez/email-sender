using Microsoft.Data.Sqlite;

namespace EmailSender.Api.Data.DbInterop;

public class SqliteConnectionFactory : IDbConnectionFactory<SqliteConnection>
{
    /// <summary>
    /// Initializes factory that generates ready-to-use SQLite connections.
    /// </summary>
    public SqliteConnectionFactory(string connectionString)
    {
        ConnectionString = connectionString;
    }

    /// <summary>
    /// SQLite connection string.
    /// </summary>
    public string ConnectionString { get; set; }

    /// <summary>
    /// Creates a new SQLite connection using
    /// <seealso cref="ConnectionString"/>.
    /// </summary>
    /// <returns>
    /// Opened SQLite connection.
    /// </returns>
    public SqliteConnection Create()
    {
        var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        return connection;
    }
}
