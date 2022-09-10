namespace Contacts.Application.Contacts.Common;

public static class ContactHelper
{
    public static string RetrieveInitialsOfNames(string firstName, string lastName)
    {
        return $"{firstName[0]}{lastName[0]}";
    }
}
