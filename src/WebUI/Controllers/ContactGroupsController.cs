using Contacts.Application.Common.Exceptions;
using Contacts.Application.Common.Models;
using Contacts.Application.ContactGroups.Commands.CreateContactGroup;
using Contacts.Application.ContactGroups.Commands.DisableContactGroup;
using Contacts.Application.ContactGroups.Commands.UpdateContactGroup;
using Contacts.Application.ContactGroups.Common;
using Contacts.Application.ContactGroups.Queries.GetContactGroup;
using Contacts.Application.ContactGroups.Queries.GetContactGroupsList;
using Contacts.Application.Contacts.Queries.GetContactsList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.WebUI.Controllers;

[Authorize]
public class ContactGroupsController : ApiControllerBase
{
    private readonly ILogger<ContactGroupsController> _logger;

    public ContactGroupsController
    (
        ILogger<ContactGroupsController> logger
    )
    {
        _logger = logger;
    }


    /// <summary>
    /// get groups list
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpGet("list")]
    public async Task<ActionResult<PaginatedList<ContactGroupListItemDto>>> GetList([FromQuery] GetContactGroupsListQuery query)
    {
        try
        {
            var result = await Mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError("ERROR => GET contactgroups/list", ex);
            throw;
        }
    }


    /// <summary>
    /// create group (with contacts)
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateContactGroupCommand command)
    {
        try
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError("ERROR => POST contactgroups", ex);
            throw;
        }
    }


    /// <summary>
    /// get group
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ContactGroupDto>> Get(int id)
    {
        try
        {
            var result = await Mediator.Send(new GetContactGroupQuery(id));
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR => GET contactgroups/{id}", ex);
            throw;
        }
    }


    /// <summary>
    /// update group (with contacts)
    /// </summary>
    /// <param name="id"></param>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateContactGroupCommand command)
    {
        if (id != command.Id)
        {
            return NotFound();
        }

        try
        {
            await Mediator.Send(command);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR => PUT contactgroups/{id}", ex);
            throw;
        }
    }


    /// <summary>
    /// disable group
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPatch("disable/{id}")]
    public async Task<ActionResult> Disable(int id)
    {
        try
        {
            await Mediator.Send(new DisableContactGroupCommand(id));
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR => PATCH contactgroups/disable/{id}", ex);
            throw;
        }
    }

}