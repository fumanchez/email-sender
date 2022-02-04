using Dapper;

using EmailSender.Api.Data.DbInterop;
using EmailSender.Api.Data.Entities;
using EmailSender.Api.Data.Entities.Attachments;

using Microsoft.Data.Sqlite;

namespace EmailSender.Api.Data.Storage;

public class SqliteEmailsStatusesRepository : IAttachmentsRepository<long, Email, EmailStatus>
{
    private readonly IDbConnectionFactory<SqliteConnection> _connectionFactory;

    public SqliteEmailsStatusesRepository(
        IDbConnectionFactory<SqliteConnection> connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    /// <summary>
    /// Get all emails statuses.
    /// </summary>
    /// <returns>
    /// Emails statuses collection.
    /// </returns>
    public Task<IEnumerable<EmailStatus>> Get()
    {
        using var connection = _connectionFactory.Create();
        var statuses = SelectEmailsStatuses(connection);
        return Task.FromResult(statuses);
    }

    /// <summary>
    /// Find all emails statuses for email with given <paramref name="emailId"/>.
    /// </summary>
    /// <returns>
    /// Emails statuses collection.
    /// </returns>
    public Task<IEnumerable<EmailStatus>> Find(in long emailId)
    {
        using var connection = _connectionFactory.Create();
        var statuses = SelectEmailsStatuses(emailId, connection);
        return Task.FromResult(statuses);
    }

    /// <summary>
    /// Save status for email.
    /// </summary>
    public Task Save(EmailStatus status)
    {
        using var connection = _connectionFactory.Create();
        Insert(status, connection);
        return Task.FromResult(status.TargetId);
    }

    private static IEnumerable<EmailStatus> SelectEmailsStatuses(in SqliteConnection connection)
    {
        const string sql =
            "select EmailId as TargetId, SendedAt, Result, FailedMessage from EmailsStatuses";

        return connection.Query<EmailStatus>(sql);
    }

    private static IEnumerable<EmailStatus> SelectEmailsStatuses(in long emailId, in SqliteConnection connection)
    {
        const string sql =
            "select EmailId as TargetId, SendedAt, Result, FailedMessage from EmailsStatuses " +
            "where TargetId = @emailId";

        return connection.Query<EmailStatus>(sql, param: new { emailId });
    }

    private static void Insert(in EmailStatus status, in SqliteConnection connection)
    {
        const string sql =
            "insert or replace into EmailsStatuses (EmailId, SendedAt, Result, FailedMessage) " +
            "values (@TargetId, @SendedAt, @Result, @FailedMessage)";

        connection.Execute(sql, param: status);
    }
}
