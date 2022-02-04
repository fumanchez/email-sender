using EmailSender.Api.Data.Entities;

namespace EmailSender.Api.Data.Storage;

/// <summary>
/// Stores entities by id.
/// </summary>
public interface IRepository<TId, TBase, TEntity>
    where TEntity : TBase, IEntity<TId>
{
    /// <summary>
    /// Get all stored entities.
    /// </summary>
    /// <returns>
    /// Entities collection.
    /// </returns>
    Task<IEnumerable<TEntity>> Get();

    /// <summary>
    /// Find stored entity by <paramref name="id"/>.
    /// </summary>
    /// <returns>
    /// Entity base.
    /// </returns>
    Task<TBase?> Find(in TId id);

    /// <summary>
    /// Save entity <paramref name="base"/>.
    /// </summary>
    /// <returns>
    /// Entity id.
    /// </returns>
    Task<TId> Save(TBase @base);
}
