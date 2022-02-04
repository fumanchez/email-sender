using EmailSender.Api.Data.Models;

namespace EmailSender.Api.Data.Entities;

public record Email : EmailBase, IEntity<long>
{
    public long Id { get; init; }
}
