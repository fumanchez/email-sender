using System.Data;

namespace EmailSender.Api.Data.DbInterop;

public interface IDbConnectionFactory<T>
    where T : IDbConnection
{
    /// <summary>
    /// Creates a new database connection.
    /// </summary>
    /// <returns>
    /// Opened connection.
    /// </returns>
    T Create();
}
