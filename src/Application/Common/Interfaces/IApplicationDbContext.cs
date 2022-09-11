using Contacts.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Contact> Contacts { get; }

    DbSet<ContactNumber> ContactNumbers { get; }

    DbSet<ContactGroup> ContactGroups { get; }

    DbSet<ContactGroupContact> ContactGroupContacts { get; }

    DbSet<Message> Messages { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
