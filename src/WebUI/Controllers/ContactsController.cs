using Contacts.Application.Common.Models;
using Contacts.Application.Contacts.Queries.GetContactsWithPagination;
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


    //[HttpPost("list")]
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

}