using Contacts.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }

    DbSet<Contact> Contacts { get; }

    DbSet<ContactGroup> ContactGroups { get; }

    DbSet<ContactGroupContact> ContactGroupContacts { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
