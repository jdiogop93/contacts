namespace Contacts.Application.Common.Interfaces;

public interface ISmsService
{
    Task SendAsync(string[] to, string body, string from = null);
}