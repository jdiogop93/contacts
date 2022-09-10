using Contacts.Application.Common.Interfaces;
using Contacts.Application.Messages.Commands.SendEmailContactGroup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.WebUI.Controllers;

[Authorize]
public class MessagesController : ApiControllerBase
{
    private readonly ILogger<MessagesController> _logger;
    private readonly IMailService _mailService;

    public MessagesController
    (
        ILogger<MessagesController> logger,
        IMailService mailService
    )
    {
        _logger = logger;
        _mailService = mailService;
    }


    /// <summary>
    /// send email
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost]
    public ActionResult Send()
    {
        try
        {
            _mailService.Send(new List<string> { "ze.diogo.pereira@hotmail.com" }, "teste", "isto é o corpo da mensagem");
            return Ok();
        }
        catch (Exception ex)
        {
            throw;
        }
    }


    /// <summary>
    /// send email to all contacts of group
    /// </summary>
    /// <param name="id"></param>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("send-email-group/{id}")]
    public async Task<ActionResult> SendEmailGroup(int id, SendEmailContactGroupCommand command)
    {
        if (id != command.Id)
        {
            return NotFound();
        }

        await Mediator.Send(command);

        return Ok();
    }


}