using ContactGroups.Application.ContactGroups.Commands.DisableContactGroup;
using Contacts.Application.ContactGroups.Commands.CreateContactGroup;
using Contacts.Application.ContactGroups.Commands.UpdateContactGroup;
using Contacts.Application.ContactGroups.Common;
using Contacts.Application.ContactGroups.Queries.GetContactGroup;
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
    /// create group (with contacts)
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateContactGroupCommand command)
    {
        return await Mediator.Send(command);
    }


    /// <summary>
    /// get group
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ContactGroupDto>> Get(int id)
    {
        return await Mediator.Send(new GetContactGroupQuery(id));
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

        await Mediator.Send(command);

        return Ok();
    }


    /// <summary>
    /// disable group
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPatch("disable/{id}")]
    public async Task<ActionResult> Disable(int id)
    {
        await Mediator.Send(new DisableContactGroupCommand(id));

        return Ok();
    }

}