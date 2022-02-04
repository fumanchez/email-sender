using System.ComponentModel.DataAnnotations;

namespace EmailSender.Api.Data.Models;

public record EmailBase
{
    [Required] public string Subject { get; init; }
    [Required] public string Body { get; init; }
    [Required] public List<string> Recipients { get; init; }
}
