using System.ComponentModel.DataAnnotations;

namespace EmailSender.Api.Data.Models;

public record UserCredentials
{
    [Required] public string Login { get; init; }
    [Required] public string Password { get; init; }
}
