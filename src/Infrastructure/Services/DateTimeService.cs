using Contacts.Application.Common.Interfaces;

namespace Contacts.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime UtcNow => DateTime.UtcNow;
}
