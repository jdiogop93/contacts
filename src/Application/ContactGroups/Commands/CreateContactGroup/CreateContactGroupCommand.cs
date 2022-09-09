using Contacts.Application.Common.Exceptions;
using Contacts.Application.Common.Interfaces;
using Contacts.Domain.Entities;
using MediatR;

namespace Contacts.Application.ContactGroups.Commands.CreateContactGroup;

public record CreateContactGroupCommand : IRequest<int>
{
    public string Name { get; set; }

    public HashSet<int> ContactsIds { get; set; }
}


public class CreateContactGroupCommandHandler : IRequestHandler<CreateContactGroupCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateContactGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateContactGroupCommand request, CancellationToken cancellationToken)
    {
        #region validates the existance of all contacts
        var contacts = _context.Contacts
            .Where(x => request.ContactsIds.Any(id => id == x.Id) && x.Active)
            .ToList();

        if (request.ContactsIds.Count != contacts.Count)
        {
            var idContactNotFound = request.ContactsIds.FirstOrDefault(id => !contacts.Any(c => c.Id == id));
            throw new NotFoundException(nameof(Contact), idContactNotFound);
        }
        #endregion

        var currentTime = DateTime.UtcNow;

        var group = new ContactGroup
        {
            Name = request.Name,
            Created = currentTime
        };
        _context.ContactGroups.Add(group);

        var contactGroupContacts = contacts
            .Select(x => new ContactGroupContact
            {
                ContactGroup = group,
                Contact = x,
                Created = currentTime
            })
            .ToList();
        if (contactGroupContacts.Count > 0)
        {
            _context.ContactGroupContacts.AddRange(contactGroupContacts);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return group.Id;
    }

}
