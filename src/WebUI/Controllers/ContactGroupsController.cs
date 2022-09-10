﻿using Contacts.Application.ContactGroups.Commands.CreateContactGroup;
using Contacts.Application.ContactGroups.Commands.UpdateContactGroup;
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
    /// create contact group (with contacts)
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateContactGroupCommand command)
    {
        return await Mediator.Send(command);
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

}