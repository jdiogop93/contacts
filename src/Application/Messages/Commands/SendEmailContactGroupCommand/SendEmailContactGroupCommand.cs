using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Contacts.Application.Common.Exceptions;
using Contacts.Application.Common.Interfaces;
using Contacts.Domain.Entities;
using Contacts.Domain.Enums;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ValidationException = Contacts.Application.Common.Exceptions.ValidationException;

namespace Contacts.Application.Messages.Commands;

//[Authorize]
public record SendEmailContactGroupCommand : IRequest<int>
{
    [Required]
    public int Id { get; set; } //ContactGroupId

    [Required]
    public string Subject { get; set; }

    [Required]
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
            .Where(c => c.Contact.Active)
            .Select(c => c.Contact.Email)
            .ToList();

        if (emails.Count == 0)
        {
            throw new ValidationException(
                new List<ValidationFailure>
                {
                    new ValidationFailure(nameof(ContactNumber), $"The contact group ({entity.Id}) doesnt have valid contact emails to send an email.")
                }
            );
        }

        var status = MessageResultStatus.OK;
        var result = status.ToString();
        try
        {
            _mailService.Send(
                emails,
                request.Subject,
                request.Content
            );
        }
        catch (Exception ex)
        {
            status = MessageResultStatus.ERROR;
            result = ex.ToString();
        }

        var message = new Message
        {
            Type = MessageType.EMAIL,
            Subject = request.Subject,
            Content = request.Content,
            EmailsTo = string.Join(";", emails),
            ResultStatus = status,
            Result = result
        };

        _context.Messages.Add(message);

        await _context.SaveChangesAsync(cancellationToken);

        return message.Id;
    }
}
