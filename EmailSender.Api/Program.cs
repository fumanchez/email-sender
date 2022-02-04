using EmailSender.Api.Data.Models;
using EmailSender.Api.Smtp;

using MailKit.Net.Smtp;

using MimeKit;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

// Configure routing
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
});

// Configure SMTP
builder.Services.AddTransient<SmtpClient>();

builder.Services.AddTransient<SmtpSession>(provider =>
{
    var client = provider.GetService<SmtpClient>() ?? throw new NullReferenceException();

    var server = new SmtpServerSettings(
        config["Smtp:Server:Host"],
        int.Parse(config["Smtp:Server:Port"]),
        bool.Parse(config["Smtp:Server:UseSsl"]));

    var name = config.GetValue<string>("Name");
    var address = config.GetValue<string>("Address");

    if (config["Credentials:Login"] is not null)
    {
        var credentials = new UserCredentials
        {
            Login = config["Credentials:Login"],
            Password = config["Credentials:Password"]
        };

        return new AuthorizedSmtpSession(client, server, new MailboxAddress(name, address), credentials);
    }

    return new SmtpSession(client, server, new MailboxAddress(name, address));
});

// Configure controllers
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI
// at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
