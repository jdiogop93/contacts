using Contacts.Application.Common.Interfaces;
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
            _mailService.Send("jdiogopereira93@gmail.com", "teste", "isto é o corpo da mensagem");
            return Ok();
        }
        catch (Exception ex)
        {
            throw;
        }
    }


}