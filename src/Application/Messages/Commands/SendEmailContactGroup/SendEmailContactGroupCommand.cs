using AutoMapper;
using Contacts.Application.Common.Exceptions;
using Contacts.Application.Common.Interfaces;
using Contacts.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Application.Messages.Commands.SendEmailContactGroup;

//[Authorize]
public record SendEmailContactGroupCommand : IRequest<int>
{
    public int Id { get; set; } //ContactGroupId

    public string Subject { get; set; }
    public string Content { get; set; }
}

public class SendEmailContactGroupCommandHandler : IRequestHandler<SendEmailContactGroupCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMailService _mailService;

    public SendEmailContactGroupCommandHandler
    (
        IApplicationDbContext context,
        IMapper mapper,
        IMailService mailService
    )
    {
        _context = context;
        _mapper = mapper;
        _mailService = mailService;
    }

    public async Task<int> Handle(SendEmailContactGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ContactGroups
            .Where(l => l.Id == request.Id && l.Active)
            .Include(x => x.Contacts.Where(n => n.Active))
            .ThenInclude(x => x.Contact)
            .SingleOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ContactGroup), request.Id);
        }

        var emails = entity.Contacts
            .Select(c => c.Contact.Email)
            .ToList();

        _mailService.Send(
            emails,
            request.Subject,
            request.Content
        );

        //if it reached here, it means the sending was successful

        var message = new Message
        {
            Type = Domain.Enums.MessageType.EMAIL,
            Subject = request.Subject,
            Content = request.Content,
            EmailsTo = string.Join(";", emails)
        };

        _context.Messages.Add(message);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
