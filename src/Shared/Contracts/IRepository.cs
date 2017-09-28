using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SupaTrupa.WebAPI.Shared.Contracts
{
    /// <summary>
    /// IRepository definition.
    /// </summary>
    /// <typeparam name="T">The type contained in the repository.</typeparam>
    /// <typeparam name="TKey">The type used for the entity's Id.</typeparam>
    public interface IRepository<T, TKey> where T : IEntity<TKey>
    {
        /// <summary>
        /// Returns an entity by its given id.
        /// </summary>
        /// <param name="id">The value representing the identifier of the entity to retrieve.</param>
        /// <returns>The Entity T.</returns>
        Task<T> GetAsync(TKey id);

        /// <summary>
        /// Returns the entities matching the predicate.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        Task<IEnumerable<T>> GetAsync(Func<T, bool> predicate);

        /// <summary>
        /// Adds the new entity in the repository.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        Task AddAsync(T entity);

        /// <summary>
        /// Adds the new entities in the repository.
        /// </summary>
        /// <param name="entities">The entities of type T.</param>
        Task AddAsync(IEnumerable<T> entities);

        /// <summary>
        /// Update an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The updated entity.</returns>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Update the entities.
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        Task UpdateAsync(IEnumerable<T> entities);

        /// <summary>
        /// Deletes an entity from the repository by its id.
        /// </summary>
        /// <param name="id">The entity's id.</param>
        Task DeleteAsync(TKey id);

        /// <summary>
        /// Deletes the given entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        Task DeleteAsync(T entity);

        /// <summary>
        /// Deletes the entities matching the predicate.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        Task DeleteAsync(Expression<Func<T, bool>> predicate);

		/// <summary>
		/// Checks if the entity exists for given predicate.
		/// </summary>
		/// <param name="predicate">The expression.</param>
		/// <returns>True when an entity matching the predicate exists, false otherwise.</returns>
		bool Exists(Expression<Func<T, bool>> predicate);

		/// <summary>
		/// Counts the total entities in the repository.
		/// </summary>
		/// <returns>Count of entities in the repository.</returns>
		long Count();
    }

    /// <summary>
    /// IRepository definition.
    /// </summary>
    /// <typeparam name="T">The type contained in the repository.</typeparam>
    /// <remarks>Entities are assumed to use strings for Id's.</remarks>
    public interface IRepository<T> : IRepository<T, string> where T : IEntity<string>
    { }
}
