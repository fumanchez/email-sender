using EmailSender.Api.Data.Models;

using MailKit.Net.Smtp;

using MimeKit;

namespace EmailSender.Api.Smtp;

public class SmtpSession : IDisposable, IAsyncDisposable
{
    private bool _disposed = false;

    private readonly MailboxAddress _sender;
    private readonly SmtpServerSettings _server;

    protected readonly ISmtpClient _client;

    /// <summary>
    /// Initializes new SMTP session.
    /// Need to be started using the <seealso cref="Start"/> method to begin email sending.
    /// </summary>
    public SmtpSession(ISmtpClient client,
        SmtpServerSettings server,
        MailboxAddress sender)
    {
        _client = client;
        _server = server;
        _sender = sender;
    }

    ~SmtpSession()
    {
        DisposeResources();
    }

    public FormatOptions FormatOptions { get; set; } = FormatOptions.Default;

    /// <summary>
    /// Sends email from given sender mailbox address.
    /// </summary>
    public Task<string> Send(EmailBase email, in CancellationToken cancellationToken = default)
    {
        var message = ToMimeMessage(email);
        message.From.Add(_sender);
        return _client.SendAsync(FormatOptions, message, cancellationToken);
    }

    /// <summary>
    /// Connects with the server.
    /// </summary>
    public virtual async Task Start()
    {
        var (host, port, useSsl) = _server;
        await _client.ConnectAsync(host, port, useSsl);
    }

    public void Dispose()
    {
        if (_disposed) return;
        else _disposed = true;

        DisposeResources();
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;
        else _disposed = true;

        await DisposeResourcesAsync();
        GC.SuppressFinalize(this);
    }

    protected virtual void DisposeResources()
    {
        _client.Disconnect(quit: true);
    }

    protected virtual async ValueTask DisposeResourcesAsync()
    {
        await _client.DisconnectAsync(quit: true).ConfigureAwait(false);
    }

    private static MimeMessage ToMimeMessage(EmailBase email)
    {
        var message = new MimeMessage
        {
            Subject = email.Subject,
            Body = new TextPart("plain") { Text = email.Body }
        };

        var recipients = email.Recipients.ConvertAll(ToMailboxAdress);
        message.To.AddRange(recipients);

        return message;
    }

    private static MailboxAddress ToMailboxAdress(string recipient) => new MailboxAddress(name: string.Empty, address: recipient);
}
