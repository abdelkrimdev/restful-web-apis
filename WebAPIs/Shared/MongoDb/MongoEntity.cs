using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Contracts;

namespace Shared.MongoDb
{
    /// <summary>
    /// Abstract Mongo Entity.
    /// </summary>
    /// <remarks>Mongo Entities are assumed to use strings for Id's.</remarks>
    public abstract class MongoEntity : IEntity<string>
    {
        /// <summary>
        /// Initializes a new instance of the MongoEntity class.
        /// </summary>
        protected MongoEntity() => Id = ObjectId.GenerateNewId().ToString();

        /// <summary>
        /// Gets or sets the id for this object (the primary record for an entity).
        /// </summary>
        /// <value>The id for this object (the primary record for an entity).</value>
        [BsonId]
        public string Id { get; set; }
    }
}
