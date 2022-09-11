using Contacts.Application.Common.Exceptions;
using Contacts.Application.Common.Interfaces;
using Contacts.Domain.Entities;
using Contacts.Domain.ValueObjects;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Application.Contacts.Commands.UploadContactPhoto;

public record UploadContactPhotoCommand : IRequest
{
    public int Id { get; set; } //ContactId
    public IFormFile Photo { get; set; }
}

public class UploadContactPhotoCommandHandler : IRequestHandler<UploadContactPhotoCommand>
{
    private readonly IApplicationDbContext _context;

    public UploadContactPhotoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UploadContactPhotoCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Contacts
            .Where(l => l.Id == request.Id && l.Active)
            .SingleOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Contact), request.Id);
        }

        var photo = request.Photo;
        if (photo == null || photo.Length == 0)
        {
            entity.Photo = null;
        }
        else
        {
            entity.Photo = new Media(photo.FileName, photo.ContentType, photo.Length);
            using (var sourceStream = photo.OpenReadStream())
            {
                var contents = new byte[sourceStream.Length];
                await sourceStream.ReadAsync(contents, 0, (int)sourceStream.Length);

                entity.Photo.Bytes = contents;
            }
        }

        _context.Contacts.Update(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

}
