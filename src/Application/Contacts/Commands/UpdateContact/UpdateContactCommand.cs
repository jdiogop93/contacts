using Contacts.Application.Common.Exceptions;
using Contacts.Application.Common.Interfaces;
using Contacts.Application.Contacts.Commands.Common;
using Contacts.Application.Contacts.Common;
using Contacts.Domain.Entities;
using Contacts.Domain.Enums;
using Contacts.Domain.ValueObjects;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Application.Contacts.Commands.UpdateContact;

public record UpdateContactCommand : IRequest
{
    //photo
    public int Id { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }

    #region Address
    public string Street { get; set; }
    public string ZipCode { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    #endregion

    public string Email { get; set; }
    public HashSet<ContactNumberDto> Numbers { get; set; }
}

public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateContactCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Contacts
            .Where(l => l.Id == request.Id && l.Active)
            .Include(x => x.Numbers.Where(n => n.Active)) //ver se é preciso filtrar pelos deste ContactId apenas
            .SingleOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Contact), request.Id);
        }

        var currentTime = DateTime.UtcNow;

        entity.FirstName = request.FirstName;
        entity.LastName = request.LastName;
        entity.Initials = ContactHelper.RetrieveInitialsOfNames(entity.FirstName, entity.LastName);
        entity.Address = new Address(request.Street, request.ZipCode, request.City, request.Country);
        entity.Email = request.Email;

        var numbers = new HashSet<ContactNumber>();
        #region ContactNumbers
        if (request.Numbers != null)
        {
            var numbersToUpdateReq = request.Numbers
                .Where(n => n.Id.HasValue && !n.ToDelete)
                .ToList();

            var numbersToDeleteReq = request.Numbers
                .Where(n => n.Id.HasValue && n.ToDelete)
                .ToList();

            //if there is the same ContactNumberId to save and to delete, throw exception
            if (
                numbersToUpdateReq != null
                && numbersToDeleteReq != null
                && numbersToUpdateReq.Any(s => numbersToDeleteReq.Any(d => d.Id == s.Id))
            )
            {
                var contactNumberToSaveAndDelete = numbersToUpdateReq
                    .FirstOrDefault(s => numbersToDeleteReq.Any(d => d.Id == s.Id));

                throw new ValidationException(
                    new List<ValidationFailure>
                    {
                        new ValidationFailure(nameof(ContactNumber), $"You cant save and delete the same contact number ({contactNumberToSaveAndDelete.Id.Value}) in the same request.")
                    }
                );
            }

            #region update
            if (
                numbersToUpdateReq != null
                && numbersToUpdateReq.Count > 0
                && ValidateExistanceOfContactNumbers(entity.Numbers, numbersToUpdateReq.Select(x => x.Id.Value).ToList())
            )
            {
                foreach (var n in numbersToUpdateReq)
                {
                    ContactNumber line = ValidateContactNumber(entity, n, numbersToUpdateReq, "update");

                    line.CountryCode = n.CountryCode;
                    line.PhoneNumber = n.PhoneNumber;
                    line.Type = (ContactNumberType)n.Type;
                    line.Default = n.Default;
                    numbers.Add(line);
                }
            }
            #endregion

            #region disable
            if (
                numbersToDeleteReq != null
                && numbersToDeleteReq.Count > 0
                && ValidateExistanceOfContactNumbers(entity.Numbers, numbersToDeleteReq.Select(x => x.Id.Value).ToList())
            )
            {
                foreach (var n in numbersToDeleteReq)
                {
                    ContactNumber line = ValidateContactNumber(entity, n, numbersToDeleteReq, "delete");

                    line.Active = false;
                    line.DisabledAt = currentTime;
                    numbers.Add(line);
                }
            }
            #endregion

            #region insert
            var numbersToAddReq = request.Numbers
                .Where(n => !n.Id.HasValue && !n.ToDelete)
                .ToList();

            foreach (var n in numbersToAddReq)
            {
                numbers.Add(new ContactNumber
                {
                    ContactId = entity.Id,
                    CountryCode = n.CountryCode,
                    PhoneNumber = n.PhoneNumber,
                    Type = (ContactNumberType)n.Type,
                    Default = n.Default
                });
            }
            #endregion
        }
        #endregion

        _context.Contacts.Update(entity);

        if (numbers != null)
        {
            var toUpdate = numbers.Where(x => x.Id != 0 && x.Active);
            var toDisable = numbers.Where(x => x.Id != 0 && !x.Active);
            var toAdd = numbers.Where(x => x.Id == 0);

            if (toUpdate.Any()) { _context.ContactNumbers.UpdateRange(toUpdate); }
            if (toDisable.Any()) { _context.ContactNumbers.UpdateRange(toDisable); }
            if (toAdd.Any()) { _context.ContactNumbers.AddRange(toAdd); }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private ContactNumber ValidateContactNumber(Contact entity, ContactNumberDto n, List<ContactNumberDto> numbersReq, string action)
    {
        var line = entity.Numbers.FirstOrDefault(x => x.Id == n.Id);
        if (line == null) //if this contact number doest not exist, throw exception
        {
            throw new NotFoundException(nameof(ContactNumber), n.Id);
        }

        var sameItems = numbersReq.Where(x => x.Id == line.Id);
        if (sameItems.Count() > 1)
        {
            throw new ValidationException(
                new List<ValidationFailure>
                {
                    new ValidationFailure(nameof(Contact), $"You are trying to {action} the contact number ({line.Id}) more than once in the same request.")
                }
            );
        }

        return line;
    }

    private bool ValidateExistanceOfContactNumbers(ICollection<ContactNumber> numbersDb, List<int> idsNumbersToSave)
    {
        idsNumbersToSave = idsNumbersToSave.Distinct().ToList();

        var numbers = numbersDb
            .Where(x => idsNumbersToSave.Any(n => n == x.Id) && x.Active)
            .ToList();

        if (idsNumbersToSave.Count != numbers.Count)
        {
            var idNumberNotFound = idsNumbersToSave.FirstOrDefault(n => !numbersDb.Any(c => c.Id == n));
            throw new NotFoundException(nameof(ContactNumber), idNumberNotFound);
        }

        return true;
    }
}
