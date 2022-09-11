using Contacts.Application.Common.Models;
using Contacts.Application.Contacts.Commands.CreateContact;
using Contacts.Application.Contacts.Commands.DisableContact;
using Contacts.Application.Contacts.Commands.UpdateContact;
using Contacts.Application.Contacts.Commands.UploadContactPhoto;
using Contacts.Application.Contacts.Common;
using Contacts.Application.Contacts.Queries.GetContact;
using Contacts.Application.Contacts.Queries.GetContactPhoto;
using Contacts.Application.Contacts.Queries.GetContactsList;
using Contacts.Application.Contacts.Queries.GetDetailedContact;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.WebUI.Controllers;

[Authorize]
public class ContactsController : ApiControllerBase
{
    private readonly ILogger<ContactsController> _logger;

    public ContactsController
    (
        ILogger<ContactsController> logger
    )
    {
        _logger = logger;
    }

    /// <summary>
    /// get contacts list
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpGet("list")]
    public async Task<ActionResult<PaginatedList<ContactListItemDto>>> GetList([FromQuery] GetContactsListQuery query)
    {
        try
        {
            var result = await Mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError("ERROR => GET contacts/list", ex);
            return BadRequest(ex.Message);
        }
    }


    /// <summary>
    /// create contact
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateContactCommand command)
    {
        try
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError("ERROR => POST contacts", ex);
            return BadRequest(ex.Message);
        }
    }


    /// <summary>
    /// upload contact photo
    /// </summary>
    /// <param name="id"></param>
    /// <param name="photo"></param>
    /// <returns></returns>
    [HttpPatch("upload-photo/{id}")]
    public async Task<ActionResult> UploadPhoto(int id, IFormFile photo)
    {
        try
        {
            await Mediator.Send(new UploadContactPhotoCommand { Id = id, Photo = photo });
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR => PATCH contacts/upload-photo/{id}", ex);
            return BadRequest(ex.Message);
        }
    }


    /// <summary>
    /// get contact photo
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("get-photo/{id}")]
    public async Task<FileStreamResult> GetPhoto(int id)
    {
        try
        {
            var media = await Mediator.Send(new GetContactPhotoQuery(id));
            return new FileStreamResult(new MemoryStream(media.Bytes), media.MimeType) { FileDownloadName = media.FileName };
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR => GET contacts/get-photo/{id}", ex);
            throw;
        }
    }


    /// <summary>
    /// get contact
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ContactItemDto>> Get(int id)
    {
        try
        {
            var result = await Mediator.Send(new GetContactQuery(id));
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR => GET contacts/{id}", ex);
            throw;
        }
    }


    /// <summary>
    /// get detailed contact
    /// </summary>
    /// <returns></returns>
    [HttpGet("detailed/{id}")]
    public async Task<ActionResult<ContactDetailedDto>> GetDetailed(int id)
    {
        try
        {
            var result = await Mediator.Send(new GetDetailedContactQuery(id));
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR => GET contacts/detailed/{id}", ex);
            throw;
        }
    }


    /// <summary>
    /// update contact
    /// </summary>
    /// <param name="id"></param>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateContactCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        try
        {
            await Mediator.Send(command);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR => PUT contacts/{id}", ex);
            throw;
        }
    }


    /// <summary>
    /// disable contact
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPatch("disable/{id}")]
    public async Task<ActionResult> Disable(int id)
    {
        try
        {
            await Mediator.Send(new DisableContactCommand(id));
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR => PATCH contacts/disable/{id}", ex);
            throw;
        }
    }

}