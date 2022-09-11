using Contacts.Application.Common.Models;
using Contacts.Application.Contacts.Commands;
using Contacts.Application.Contacts.Commands.CreateContact;
using Contacts.Application.Contacts.Commands.DisableContact;
using Contacts.Application.Contacts.Commands.UpdateContact;
using Contacts.Application.Contacts.Common;
using Contacts.Application.Contacts.Queries.GetContact;
using Contacts.Application.Contacts.Queries.GetContactsList;
using Contacts.Application.Contacts.Queries.GetDetailedContact;
using Contacts.Infrastructure.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
            //command.Photo = await FilesHelper.RetrieveFile(Request);

            //var result = await Mediator.Send(command);
            //return Ok(result);
            return Ok(1);
        }
        catch (Exception ex)
        {
            _logger.LogError("ERROR => POST contacts", ex);
            return BadRequest(ex.Message);
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