namespace EmailSender.Api.Smtp;

public record SmtpServerSettings(string Host, int Port, bool UseSsl = false);
