using EmailSender.Api.Data.Entities;
using EmailSender.Api.Data.Entities.Attachments;

namespace EmailSender.Api.Data.Storage;

/// <summary>
/// Stores attachments which belong to an entity of <typeparamref name="TTarget"/> type
/// and have the same <typeparamref name="TId"/>.
/// </summary>
public interface IAttachmentsRepository<TId, TTarget, TAttachment>
    where TTarget : IEntity<TId>
    where TAttachment : IEntityAttachment<TId, TTarget>
{
    /// <summary>
    /// Get all entities attachments.
    /// </summary>
    /// <returns>
    /// Attachments collection.
    /// </returns>
    Task<IEnumerable<TAttachment>> Get();

    /// <summary>
    /// Find all attachments belongs to entity with given <paramref name="targetId"/>.
    /// </summary>
    /// <returns>
    /// Attachments collection.
    /// </returns>
    Task<IEnumerable<TAttachment>> Find(in long targetId);

    /// <summary>
    /// Save entity attachment.
    /// </summary>
    Task Save(TAttachment attachment);
}
