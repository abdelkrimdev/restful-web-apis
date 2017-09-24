using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

using SupaTrupa.WebAPI.Settings;
using SupaTrupa.WebAPI.Shared.Contracts;

namespace SupaTrupa.WebAPI.Shared.MongoDb
{
    /// <summary>
    /// Deals with entities in MongoDb.
    /// </summary>
    /// <typeparam name="T">The type contained in the repository.</typeparam>
    /// <typeparam name="TKey">The type used for the entity's Id.</typeparam>
    public class MongoRepository<T, TKey> : IRepository<T, TKey>
        where T : IEntity<TKey>
    {
        /// <summary>
        /// MongoCollection field.
        /// </summary>
        protected IMongoCollection<T> _collection;

        /// <summary>
        /// Initializes a new instance of the MongoRepository class.
        /// Uses the default connection string and Database name from the appsettings.json file.
        /// </summary>
        public MongoRepository(IOptions<MongoSettings> settings)
            : this(MongoService.GetConnectionString(settings)) { }

        /// <summary>
        /// Initializes a new instance of the MongoRepository class.
        /// </summary>
        /// <param name="connectionString">connection string to use for connecting to MongoDB.</param>
        public MongoRepository(string connectionString)
        {
            _collection = MongoService.GetCollectionFromConnectionString<T>(connectionString);
        }

        /// <summary>
        /// Initializes a new instance of the MongoRepository class.
        /// </summary>
        /// <param name="connectionString">connection string to use for connecting to MongoDB.</param>
        /// <param name="collectionName">The name of the collection to use.</param>
        public MongoRepository(string connectionString, string collectionName)
        {
            _collection = MongoService.GetCollectionFromConnectionString<T>(connectionString, collectionName);
        }

        /// <summary>
        /// Initializes a new instance of the MongoRepository class.
        /// </summary>
        /// <param name="url">Url to use for connecting to MongoDB.</param>
        public MongoRepository(MongoUrl url)
        {
            _collection = MongoService.GetCollectionFromUrl<T>(url);
        }

        /// <summary>
        /// Initializes a new instance of the MongoRepository class.
        /// </summary>
        /// <param name="url">Url to use for connecting to MongoDB.</param>
        /// <param name="collectionName">The name of the collection to use.</param>
        public MongoRepository(MongoUrl url, string collectionName)
        {
            _collection = MongoService.GetCollectionFromUrl<T>(url, collectionName);
        }

        /// <summary>
        /// Returns the T by its given id.
        /// </summary>
        /// <param name="id">The Id of the entity to retrieve.</param>
        /// <returns>The Entity T.</returns>
        public virtual async Task<T> GetAsync(TKey id)
        {
            var result = await GetAsync(e => e.Id.Equals(id));
            return result.SingleOrDefault();
        }

        /// <summary>
        /// Returns the entities matching the predicate.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        public virtual async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            var result = Enumerable.Empty<T>();
            using (var cursor = await _collection.FindAsync(predicate))
            {
                while (await cursor.MoveNextAsync())
                {
                    result = result.Concat(cursor.Current);
                }
            }
            return result;
        }

        /// <summary>
        /// Adds the new entity in the repository.
        /// </summary>
        /// <param name="entity">The entity T.</param>
        public virtual async Task AddAsync(T entity) => await _collection.InsertOneAsync(entity);

        /// <summary>
        /// Adds the new entities in the repository.
        /// </summary>
        /// <param name="entities">The entities of type T.</param>
        public virtual async Task AddAsync(IEnumerable<T> entities) => await _collection.InsertManyAsync(entities);

        /// <summary>
        /// Update an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The updated entity.</returns>
        public virtual async Task UpdateAsync(T entity)
        {
            await _collection.FindOneAndReplaceAsync(e => e.Id.Equals(entity.Id), entity);
        }

        /// <summary>
        /// Update the entities.
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        public virtual async Task UpdateAsync(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                await UpdateAsync(entity);
            }
        }

        /// <summary>
        /// Deletes an entity from the repository by its id.
        /// </summary>
        /// <param name="id">The entity's id.</param>
        public virtual async Task DeleteAsync(TKey id)
        {
            await _collection.FindOneAndDeleteAsync(e => e.Id.Equals(id));
        }

        /// <summary>
        /// Deletes the given entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        public virtual async Task DeleteAsync(T entity)
        {
            await DeleteAsync(entity.Id);
        }

        /// <summary>
        /// Deletes the entities matching the predicate.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        public virtual async Task DeleteAsync(Expression<Func<T, bool>> predicate)
        {
            await _collection.DeleteManyAsync(predicate);
        }

        /// <summary>
        /// Checks if the entity exists for given predicate.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        /// <returns>True when an entity matching the predicate exists, false otherwise.</returns>
        public virtual bool Exists(Expression<Func<T, bool>> predicate)
        {
            return _collection.AsQueryable().Any(predicate);
        }


        /// <summary>
        /// Counts the total entities in the repository.
        /// </summary>
        /// <returns>Count of entities in the collection.</returns>
        public virtual long Count() => _collection.Count(Builders<T>.Filter.Empty);

        #region IQueryable<T>

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An IEnumerator of type T object that can be used to iterate through the collection.</returns>
        public virtual IEnumerator<T> GetEnumerator() => _collection.AsQueryable().GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() => _collection.AsQueryable().GetEnumerator();

        /// <summary>
        /// Gets the type of the element(s) that are returned.
        /// When the expression tree associated with this instance of IQueryable is executed.
        /// </summary>
        public virtual Type ElementType
        {
            get { return _collection.AsQueryable().ElementType; }
        }

        /// <summary>
        /// Gets the expression tree that is associated with the instance of IQueryable.
        /// </summary>
        public virtual Expression Expression
        {
            get { return _collection.AsQueryable().Expression; }
        }

        /// <summary>
        /// Gets the query provider that is associated with this data source.
        /// </summary>
        public virtual IQueryProvider Provider
        {
            get { return _collection.AsQueryable().Provider; }
        }

        #endregion
    }

    /// <summary>
    /// Deals with entities in MongoDb.
    /// </summary>
    /// <typeparam name="T">The type contained in the repository.</typeparam>
    /// <remarks>Mongo Entities are assumed to use strings for Id's.</remarks>
    public class MongoRepository<T> : MongoRepository<T, string>, IRepository<T>
        where T : MongoEntity
    {
        /// <summary>
        /// Initializes a new instance of the MongoRepository class.
        /// Uses the default connection string and Database name from the appsettings.json file.
        /// </summary>
        public MongoRepository(IOptions<MongoSettings> settings)
            : base(settings) { }

        /// <summary>
        /// Initializes a new instance of the MongoRepository class.
        /// </summary>
        /// <param name="connectionString">Connection string to use for connecting to MongoDB.</param>
        public MongoRepository(string connectionString)
            : base(connectionString) { }

        /// <summary>
        /// Initializes a new instance of the MongoRepository class.
        /// </summary>
        /// <param name="connectionString">connection string to use for connecting to MongoDB.</param>
        /// <param name="collectionName">The name of the collection to use.</param>
        public MongoRepository(string connectionString, string collectionName)
            : base(connectionString, collectionName) { }

        /// <summary>
        /// Initializes a new instance of the MongoRepository class.
        /// </summary>
        /// <param name="url">Url to use for connecting to MongoDB.</param>
        public MongoRepository(MongoUrl url)
            : base(url) { }

        /// <summary>
        /// Initializes a new instance of the MongoRepository class.
        /// </summary>
        /// <param name="url">Url to use for connecting to MongoDB.</param>
        /// <param name="collectionName">The name of the collection to use.</param>
        public MongoRepository(MongoUrl url, string collectionName)
            : base(url, collectionName) { }
    }
}
