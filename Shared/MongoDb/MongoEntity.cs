using MongoDB.Bson.Serialization.Attributes;

using SupaTrupa.WebAPI.Shared.Contracts;

namespace SupaTrupa.WebAPI.Shared.MongoDb
{
    /// <summary>
    /// Abstract Entity for all the BusinessEntities.
    /// </summary>
    /// <typeparam name="TKey">The type used for the entity's Id.</typeparam>
    public abstract class MongoEntity<TKey> : IEntity<TKey>
    {
        /// <summary>
        /// Gets or sets the id for this object (the primary record for an entity).
        /// </summary>
        /// <value>The id for this object (the primary record for an entity).</value>
        [BsonId]
        public virtual TKey Id { get; set; }
    }
}
