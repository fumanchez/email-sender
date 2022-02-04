using EmailSender.Api.Data.Entities;
using EmailSender.Api.Data.Entities.Attachments;
using EmailSender.Api.Data.Models;
using EmailSender.Api.Data.Storage;
using EmailSender.Api.Smtp;

using Microsoft.AspNetCore.Mvc;

namespace EmailSender.Api.Controllers;

[ApiController] [Route("api/[controller]")]
public class MailsController : ControllerBase
{
    private readonly SmtpSession _session;
    private readonly IRepository<long, EmailBase, Email> _repository;
    private readonly IAttachmentsRepository<long, Email, EmailStatus> _statusesRepository;

    /// <summary>
    /// Initializes an email sending controller instance .
    /// </summary>
    public MailsController(SmtpSession session,
        IRepository<long, EmailBase, Email> repository,
        IAttachmentsRepository<long, Email, EmailStatus> statusesRepository)
    {
        _session = session;
        _repository = repository;
        _statusesRepository = statusesRepository;
    }

    /// <summary>
    /// Get all emails.
    /// </summary>
    [HttpGet]
    public async Task<IEnumerable<GetResponce>> GetEmails()
    {
        var emails = await _repository.Get();
        var statuses = await _statusesRepository.Get();
        return emails.Zip(statuses).Select(pair => new GetResponce(pair.First, pair.Second));
    }

    /// <summary>
    /// Find email by id.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<GetResponce>> FindEmail(long id)
    {
        var email = await _repository.Find(id);
        if (email is null) return NotFound();

        var statuses = await _statusesRepository.Find(id);

        return statuses.Any() ?
            Ok(new FindResponce(email, statuses.Last())) :
            Conflict(email);
    }

    /// <summary>
    /// Send email and log it.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Send([FromBody] EmailBase email)
    {
        var id = await _repository.Save(email);
        var sendedAt = DateTime.Now;

        try
        {
            await _session.Start();
            var responce = await _session.Send(email);
            await _statusesRepository.Save(new EmailStatus
            {
                TargetId = id,
                SendedAt = sendedAt,
                Result = "Ok"
            });

            return Ok(responce);
        }
        catch (Exception ex)
        {
            await _statusesRepository.Save(new EmailStatus
            {
                TargetId = id,
                SendedAt = sendedAt,
                Result = "Failed",
                FailedMessage = ex.Message
            });

            throw;
        }
    }

    public record GetResponce(Email Email, EmailStatus Status);
    public record FindResponce(EmailBase Email, EmailStatus Status);
}
