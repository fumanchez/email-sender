using EmailSender.Api.Data.Models;

using MailKit.Net.Smtp;

using MimeKit;

namespace EmailSender.Api.Smtp;

public class AuthorizedSmtpSession : SmtpSession
{
    private readonly UserCredentials _credentials;

    /// <summary>
    /// <inheritdoc cref="SmtpSession(ISmtpClient, SmtpServerSettings, MailboxAddress)"/>
    /// Uses <paramref name="credentials"/> for authentication.
    /// </summary>
    public AuthorizedSmtpSession(ISmtpClient client,
        SmtpServerSettings server,
        MailboxAddress sender,
        UserCredentials credentials)
        : base(client, server, sender)
    {
        _credentials = credentials;
    }

    public override async Task Start()
    {
        await base.Start();

        await _client.AuthenticateAsync(_credentials.Login, _credentials.Password);
    }
}
