namespace Contacts.Application.Common.Interfaces;

public interface IMailService
{
    void Send(IList<string> to, string subject, string html/*, string from = null*/);
}
