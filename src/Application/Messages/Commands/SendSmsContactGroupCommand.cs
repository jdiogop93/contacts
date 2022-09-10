using AutoMapper;
using Contacts.Application.Common.Exceptions;
using Contacts.Application.Common.Interfaces;
using Contacts.Domain.Entities;
using Contacts.Domain.Enums;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Application.Messages.Commands;

//[Authorize]
public record SendSmsContactGroupCommand : IRequest<int>
{
    public int Id { get; set; } //ContactGroupId

    public string Message { get; set; }
}

public class SendSmsContactGroupCommandHandler : IRequestHandler<SendSmsContactGroupCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ISmsService _smsService;

    public SendSmsContactGroupCommandHandler
    (
        IApplicationDbContext context,
        IMapper mapper,
        ISmsService smsService
    )
    {
        _context = context;
        _mapper = mapper;
        _smsService = smsService;
    }

    public async Task<int> Handle(SendSmsContactGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ContactGroups
            .Where(l => l.Id == request.Id && l.Active)
            .Include(x => x.Contacts.Where(n => n.Active))
            .ThenInclude(x => x.Contact)
            .ThenInclude(x => x.Numbers.Where(n => n.Default && n.Active).Take(1))
            .SingleOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ContactGroup), request.Id);
        }

        var phoneNumbers = entity.Contacts
            .Select(x => x.Contact)
            .SelectMany(x => x.Numbers)
            .Select(n => $"{n.CountryCode}{n.PhoneNumber}".Replace("+", ""))
            .ToArray();

        if (phoneNumbers.Length == 0)
        {
            throw new ValidationException(
                new List<ValidationFailure>
                {
                    new ValidationFailure(nameof(ContactNumber), $"The contact group ({entity.Id}) doesnt have valid contact phone numbers to send a message.")
                }
            );
        }

        var status = MessageResultStatus.OK;
        var result = status.ToString();
        try
        {
            await _smsService.SendAsync(
                phoneNumbers,
                request.Message,
                entity.Name
            );
        }
        catch (Exception ex)
        {
            status = MessageResultStatus.ERROR;
            result = ex.Message;
        }

        var message = new Message
        {
            Type = MessageType.SMS,
            Content = request.Message,
            PhoneNumbers = string.Join(";", phoneNumbers),
            ResultStatus = status,
            Result = result
        };

        _context.Messages.Add(message);

        await _context.SaveChangesAsync(cancellationToken);

        return message.Id;
    }
}
