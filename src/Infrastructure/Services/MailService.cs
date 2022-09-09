using Contacts.Application.Common.Interfaces;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace Contacts.Infrastructure.Services;

public class MailService : IMailService
{
    //private readonly AppSettings _appSettings;

    public MailService
    (
    //IOptions<AppSettings> appSettings
    )
    {
        //_appSettings = appSettings.Value;
    }

    public void Send(string to, string subject, string html, string from = null)
    {
        from = "contacts.challenge.set2022@sapo.pt";
        var pass = "!!chaLLenge123#";

        // create message
        var email = new MimeMessage();

        //email.From.Add(MailboxAddress.Parse(from ?? _appSettings.EmailFrom));
        email.From.Add(MailboxAddress.Parse(from));

        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html) { Text = html };

        // send email
        using var smtp = new SmtpClient();

        //smtp.Connect(_appSettings.SmtpHost, _appSettings.SmtpPort, SecureSocketOptions.StartTls);
        smtp.Connect("smtp.sapo.pt", 587, SecureSocketOptions.Auto);

        //smtp.Authenticate(_appSettings.SmtpUser, _appSettings.SmtpPass);
        smtp.Authenticate(from, pass);

        smtp.Send(email);
        smtp.Disconnect(true);
    }
}