namespace EmailSender.Api.Data.Entities;

/// <summary>
/// Indicates that an object can be identified.
/// </summary>
public interface IEntity<TId>
{
    /// <summary>
    /// Unique identifier.
    /// </summary>
    TId Id { get; }
}
