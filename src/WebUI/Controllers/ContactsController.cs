using Contacts.Application.Common.Models;
using Contacts.Application.Contacts.Commands.CreateContact;
using Contacts.Application.Contacts.Queries.GetContactsWithPagination;
using Contacts.Application.TodoLists.Commands.CreateTodoList;
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
    public async Task<ActionResult<PaginatedList<ContactListItemDto>>> GetContactsWithPagination([FromQuery] GetContactsWithPaginationQuery query)
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

}