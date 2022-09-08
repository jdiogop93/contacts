using Contacts.Application.Common.Exceptions;
using Contacts.Application.Common.Interfaces;
using Contacts.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Application.Contacts.Commands.DisableContact;

public record DisableContactCommand(int Id) : IRequest;

public class DisableContactCommandHandler : IRequestHandler<DisableContactCommand>
{
    private readonly IApplicationDbContext _context;

    public DisableContactCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DisableContactCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Contacts
            .Where(l => l.Id == request.Id && l.Active)
            .Include(x => x.Numbers)
            .SingleOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Contact), request.Id);
        }

        var currentTime = DateTime.UtcNow;

        entity.Active = false;
        entity.DisabledAt = currentTime;

        if (entity.Numbers != null)
        {
            foreach (var number in entity.Numbers)
            {
                number.Active = false;
                number.DisabledAt = currentTime;
            }
        }

        _context.Contacts.Update(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
