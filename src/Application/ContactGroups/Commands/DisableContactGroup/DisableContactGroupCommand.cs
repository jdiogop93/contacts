using Contacts.Application.Common.Exceptions;
using Contacts.Application.Common.Interfaces;
using Contacts.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Application.ContactGroups.Commands.DisableContactGroup;

public record DisableContactGroupCommand(int Id) : IRequest;

public class DisableContactGroupCommandHandler : IRequestHandler<DisableContactGroupCommand>
{
    private readonly IApplicationDbContext _context;

    public DisableContactGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DisableContactGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ContactGroups
            .Where(l => l.Id == request.Id && l.Active)
            .Include(x => x.Contacts.Where(c => c.Active))
            .SingleOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ContactGroup), request.Id);
        }

        var currentTime = DateTime.UtcNow;

        entity.Active = false;
        entity.DisabledAt = currentTime;

        if (entity.Contacts != null)
        {
            foreach (var cgc in entity.Contacts)
            {
                cgc.Active = false;
                cgc.DisabledAt = currentTime;
            }
        }

        _context.ContactGroups.Update(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
