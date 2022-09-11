using AutoMapper;
using Contacts.Application.Common.Exceptions;
using Contacts.Application.Common.Interfaces;
using Contacts.Application.Contacts.Commands.Common;
using Contacts.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Application.Contacts.Queries.GetContactPhoto;

//[Authorize]
public record GetContactPhotoQuery(int Id) : IRequest<MediaDto>;

public class GetContactPhotoQueryHandler : IRequestHandler<GetContactPhotoQuery, MediaDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetContactPhotoQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<MediaDto> Handle(GetContactPhotoQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Contacts
            .Where(l => l.Id == request.Id && l.Active)
            .SingleOrDefaultAsync(cancellationToken);

        if (entity == null || entity.Photo == null)
        {
            throw new NotFoundException(nameof(Contact), request.Id);
        }

        return new MediaDto
        {
            Bytes = entity.Photo.Bytes,
            FileName = entity.Photo.FileName,
            MimeType = entity.Photo.MimeType,
            Size = entity.Photo.Size
        };
    }
}
