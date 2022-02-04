namespace EmailSender.Api.Data.Entities.Attachments;

public interface IEntityAttachment<TId, TEntity>
    where TEntity : IEntity<TId>
{
    /// <summary>
    /// Identifies the entity to which this attachment belongs.
    /// </summary>
    TId TargetId { get; }
}
