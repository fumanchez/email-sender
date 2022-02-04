using EmailSender.Api.Data.DbInterop;

namespace EmailSender.Api.Internal;

internal class CreateSqliteEmailsDbQuery
{
    private readonly SqliteConnectionFactory _connectionFactory;

    /// <summary>
    /// Creates the necessary tables in the database
    /// using <seealso cref="Execute"/> method.
    /// </summary>
    public CreateSqliteEmailsDbQuery(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    /// <summary>
    /// Creates tables for emails and their recipients.
    /// </summary>
    public void Execute()
    {
        CreateEmailsTable();
        CreateEmailsRecipientsTable();
        CreateEmailsStatusesTable();
    }

    private void CreateEmailsTable()
    {
        const string commandText =
             "create table if not exists Emails (" +
                 "Id integer primary key autoincrement, " +
                 "Subject text not null, " +
                 "Body text not null" +
                 ")";

        using var connection = _connectionFactory.Create();
        using var command = connection.CreateCommand();
        command.CommandText = commandText;
        command.ExecuteNonQuery();
    }

    private void CreateEmailsRecipientsTable()
    {
        const string commandText =
             "create table if not exists EmailsRecipients (" +
                 "EmailId integer, " +
                 "Recipient text not null, " +
                 "foreign key (EmailId) references Emails(Id)" +
                 ")";

        using var connection = _connectionFactory.Create();
        using var command = connection.CreateCommand();
        command.CommandText = commandText;
        command.ExecuteNonQuery();
    }

    private void CreateEmailsStatusesTable()
    {
        const string commandText =
             "create table if not exists EmailsStatuses (" +
                 "EmailId integer primary key, " +
                 "SendedAt text not null, " +
                 "Result text not null, " +
                 "FailedMessage text, " +
                 "foreign key (EmailId) references Emails(Id)" +
                 ")";

        using var connection = _connectionFactory.Create();
        using var command = connection.CreateCommand();
        command.CommandText = commandText;
        command.ExecuteNonQuery();
    }
}
