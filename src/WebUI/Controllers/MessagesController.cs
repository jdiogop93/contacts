using Contacts.Application.Common.Interfaces;
using Contacts.Application.Messages.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.WebUI.Controllers;

[Authorize]
public class MessagesController : ApiControllerBase
{
    private readonly ILogger<MessagesController> _logger;
    private readonly IMailService _mailService;
    private readonly ISmsService _smsService;
    private readonly IConfiguration _configuration;

    public MessagesController
    (
        ILogger<MessagesController> logger,
        IMailService mailService,
        ISmsService smsService,
        IConfiguration configuration
    )
    {
        _logger = logger;
        _mailService = mailService;
        _smsService = smsService;
        _configuration = configuration;
    }


    /// <summary>
    /// send test email
    /// </summary>
    /// <param name="to"></param>
    /// <returns></returns>
    [HttpPost("send-test-email/{to}")]
    public ActionResult SendTestEmail(string to)
    {
        try
        {
            _mailService.Send(new List<string> { to }, "test subject", "this is a test body email");
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR => POST messages/send-test-email/{to}", ex);
            throw;
        }
    }


    /// <summary>
    /// send test sms
    /// </summary>
    /// <param name="to"></param>
    /// <returns></returns>
    [HttpPost("send-test-sms/{to}")]
    public async Task<ActionResult> SendTestSms(string to)
    {
        try
        {
            await _smsService.SendAsync(new string[] { to }, "this is a text body sms");
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR => POST messages/send-test-sms/{to}", ex);
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

        try
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR => POST messages/send-email-group/{id}", ex);
            throw;
        }
    }


    /// <summary>
    /// send sms to all contacts of group
    /// </summary>
    /// <param name="id"></param>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("send-sms-group/{id}")]
    public async Task<ActionResult> SendSmsGroup(int id, SendSmsContactGroupCommand command)
    {
        if (id != command.Id)
        {
            return NotFound();
        }

        try
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR => POST messages/send-sms-group/{id}", ex);
            throw;
        }
    }

}