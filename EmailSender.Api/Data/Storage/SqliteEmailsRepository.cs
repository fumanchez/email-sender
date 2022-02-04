using Dapper;

using EmailSender.Api.Data.DbInterop;
using EmailSender.Api.Data.Entities;
using EmailSender.Api.Data.Models;
using EmailSender.Api.Internal.Extensions;

using Microsoft.Data.Sqlite;

namespace EmailSender.Api.Data.Storage;

public partial class SqliteEmailsRepository : IRepository<long, EmailBase, Email>
{
    private readonly IDbConnectionFactory<SqliteConnection> _connectionFactory;

    public SqliteEmailsRepository(
        IDbConnectionFactory<SqliteConnection> connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public Task<IEnumerable<Email>> Get()
    {
        using var connection = _connectionFactory.Create();
        var emails = SelectEmails(connection);
        return Task.FromResult(emails);
    }

    public Task<EmailBase?> Find(in long id)
    {
        using var connection = _connectionFactory.Create();
        var email = SelectEmail(id, connection);
        return Task.FromResult(email);
    }

    public Task<long> Save(EmailBase email)
    {
        using var connection = _connectionFactory.Create();
        var id = Insert(email, connection);
        return Task.FromResult(id);
    }

    private static IEnumerable<Email> SelectEmails(SqliteConnection connection)
    {
        const string sql =
            "select Id, Subject, Body, Recipient from Emails " +
            "left join EmailsRecipients on Id = EmailId";

        var rows = connection.Query<(long Id, string Subject, string Body, string Recipient)>(sql).ToList();

        var emails =
            from row in rows
            group row by row.Id into g
            select new Email
            {
                Id = g.Key,
                Body = g.First().Body,
                Subject = g.First().Subject,
                Recipients = g.Select(r => r.Recipient).ToList()
            };

        return emails;
    }

    private static EmailBase? SelectEmail(in long id, SqliteConnection connection)
    {
        const string sql =
            "select Subject, Body, Recipient from Emails " +
            "left join EmailsRecipients on Id = EmailId " +
            "where Id = @id";

        var rows = connection.Query<(string Subject, string Body, string Recipient)>(sql, param: new { id }).ToList();
        if (rows.Count < 1) return null;

        else return new EmailBase
        {
            Subject = rows[0].Subject,
            Body = rows[0].Body,
            Recipients = rows.ConvertAll(row => row.Recipient)
        };
    }

    private static long Insert(EmailBase email, SqliteConnection connection)
    {
        using var transaction = connection.BeginTransaction();

        InsertBase(email, connection, transaction);

        var emailId = connection.SelectLastInsertRowId();
        InsertRecipients(emailId, email.Recipients, connection, transaction);

        transaction.Commit();
        return emailId;
    }

    private static void InsertBase(in EmailBase email,
        in SqliteConnection connection, in SqliteTransaction transaction)
    {
        const string sql =
            "insert into Emails (Subject, Body) " +
            "values (@Subject, @Body)";

        connection.Execute(sql, email, transaction);
    }

    private static void InsertRecipients(in long emailId, in List<string> recipients,
        in SqliteConnection connection, in SqliteTransaction transaction)
    {
        foreach (var recipient in recipients)
            InsertRecipient(emailId, recipient, connection, transaction);
    }

    private static void InsertRecipient(in long emailId, in string recipient,
        in SqliteConnection connection, in SqliteTransaction transaction)
    {
        const string sql =
            "insert into EmailsRecipients (EmailId, Recipient) " +
            "values (@emailId, @recipient)";

        connection.Execute(sql, param: new { emailId, recipient }, transaction);
    }
}
