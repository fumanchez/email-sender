using System.ComponentModel.DataAnnotations;

namespace EmailSender.Api.Data.Entities.Attachments;

public record EmailStatus : IEntityAttachment<long, Email>
{
    [Required] public long TargetId { get; init; }
    [Required] public DateTime SendedAt { get; init; }
    [Required] public string Result { get; init; }
    public string? FailedMessage { get; init; } = null;
}
