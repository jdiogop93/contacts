using System.ComponentModel.DataAnnotations;
using Contacts.Application.Common.Exceptions;
using Contacts.Application.Common.Interfaces;
using Contacts.Domain.Entities;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ValidationException = Contacts.Application.Common.Exceptions.ValidationException;

namespace Contacts.Application.ContactGroups.Commands.UpdateContactGroup;

public record UpdateContactGroupCommand : IRequest
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public HashSet<int> ContactsIdsToSave { get; set; }
    public HashSet<int> ContactsIdsToDelete { get; set; }
}

public class UpdateContactGroupCommandHandler : IRequestHandler<UpdateContactGroupCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateContactGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateContactGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ContactGroups
            .Where(l => l.Id == request.Id && l.Active)
            .Include(x => x.Contacts.Where(c => c.ContactGroupId == request.Id && c.Active))
            .SingleOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ContactGroup), request.Id);
        }

        var currentTime = DateTime.UtcNow;


        #region ContactGroupContacts

        //if there is the same ContactId to save and to delete, throw exception
        if (
            request.ContactsIdsToSave != null
            && request.ContactsIdsToDelete != null
            && request.ContactsIdsToSave.Any(s => request.ContactsIdsToDelete.Any(d => d == s))
        )
        {
            var idContactToSaveAndDelete = request.ContactsIdsToSave
                .FirstOrDefault(s => request.ContactsIdsToDelete.Any(d => d == s));

            throw new ValidationException(
                new List<ValidationFailure>
                {
                    new ValidationFailure(nameof(Contact), $"You cant save and delete the same contact ({idContactToSaveAndDelete}) in the same request.")
                }
            );
        }

        var contactsToSave = new HashSet<ContactGroupContact>();
        var contactsToDisable = new HashSet<ContactGroupContact>();

        #region insert
        if (
            request.ContactsIdsToSave != null
            && request.ContactsIdsToSave.Count > 0
            && ValidateExistanceOfContacts(request.ContactsIdsToSave)
        )
        {
            foreach (var c in request.ContactsIdsToSave)
            {
                var line = entity.Contacts.FirstOrDefault(x => x.ContactId == c);
                if (line != null) { continue; } //if this contact already exists, go to the next one

                contactsToSave.Add(new ContactGroupContact
                {
                    ContactId = c,
                    ContactGroupId = entity.Id,
                    Created = currentTime
                });
            }
        }
        #endregion

        #region disable
        if (
            request.ContactsIdsToDelete != null
            && request.ContactsIdsToDelete.Count > 0
            && ValidateExistanceOfContacts(request.ContactsIdsToDelete)
        )
        {
            foreach (var c in request.ContactsIdsToDelete)
            {
                var line = entity.Contacts.FirstOrDefault(x => x.ContactId == c);
                if (line == null) { continue; } //if this contact doest not exist in this list, go to the next one

                line.Active = false;
                line.DisabledAt = currentTime;
                contactsToDisable.Add(line);
            }
        }
        #endregion
        #endregion


        entity.Name = request.Name;
        _context.ContactGroups.Update(entity);


        if (contactsToSave.Count > 0)
        {
            _context.ContactGroupContacts.AddRange(contactsToSave);
        }

        if (contactsToDisable.Count > 0)
        {
            _context.ContactGroupContacts.UpdateRange(contactsToDisable);
        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }


    private bool ValidateExistanceOfContacts(HashSet<int> contactIds)
    {
        var contacts = _context.Contacts
            .Where(x => contactIds.Any(id => id == x.Id) && x.Active)
            .ToList();

        if (contactIds.Count != contacts.Count)
        {
            var idContactNotFound = contactIds.FirstOrDefault(id => !contacts.Any(c => c.Id == id));
            throw new NotFoundException(nameof(Contact), idContactNotFound);
        }

        return true;
    }


}
