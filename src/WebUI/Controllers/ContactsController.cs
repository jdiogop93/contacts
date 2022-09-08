using Contacts.Application.Common.Models;
using Contacts.Application.Contacts.Commands.CreateContact;
using Contacts.Application.Contacts.Commands.DisableContact;
using Contacts.Application.Contacts.Queries.GetContact;
using Contacts.Application.Contacts.Queries.GetContactsList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.WebUI.Controllers;

[Authorize]
public class ContactsController : ApiControllerBase
{
    private readonly ILogger<ContactsController> _logger;

    public ContactsController(
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
            var a = await Mediator.Send(query);
            return a;
        }
        catch (Exception ex)
        {
            _logger.LogError("ERROR: contacts/list", ex);
            throw;
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
        return await Mediator.Send(command);
    }


    /// <summary>
    /// get contact
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ContactItemDto>> Get(int id)
    {
        return await Mediator.Send(new GetContactQuery(id));
    }


    /// <summary>
    /// disable contact
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPatch("disable/{id}")]
    public async Task<ActionResult> Disable(int id)
    {
        await Mediator.Send(new DisableContactCommand(id));
        return Ok();
    }

}