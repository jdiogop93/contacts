namespace Contacts.Application.Common.Interfaces;

public interface IMailService
{
    void Send(string to, string subject, string html, string from = null);
}
