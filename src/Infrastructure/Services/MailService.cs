using Contacts.Application.Common.Interfaces;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;

namespace Contacts.Infrastructure.Services;

public class MailService : IMailService
{
    private readonly IConfiguration _configuration;

    public MailService
    (
        IConfiguration configuration
    )
    {
        _configuration = configuration;
    }

    public void Send(IList<string> to, string subject, string html/*, string from = null*/)
    {
        // create message
        var email = new MimeMessage();

        email.From.Add(MailboxAddress.Parse(/*from ??*/_configuration["Smtp:Username"]));
        email.To.AddRange(to.Select(x => MailboxAddress.Parse(x)));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html) { Text = html };

        // send email
        using var smtp = new SmtpClient();

        //smtp.Connect(_appSettings.SmtpHost, _appSettings.SmtpPort, SecureSocketOptions.StartTls);
        smtp.Connect(_configuration["Smtp:Host"], int.Parse(_configuration["Smtp:Port"]), SecureSocketOptions.Auto);

        //smtp.Authenticate(_appSettings.SmtpUser, _appSettings.SmtpPass);
        smtp.Authenticate(_configuration["Smtp:Username"], _configuration["Smtp:Password"]);

        smtp.Send(email);
        smtp.Disconnect(true);
    }
}