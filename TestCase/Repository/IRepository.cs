using System.Linq.Expressions;
using TestCase.Entities;

namespace TestCase.Repository
{
    public interface IRepository
    {
    }

    public interface IRepository<T> : IRepository where T : BaseEntity
    {
        /// <summary>
        /// Get entity by id
        /// </summary>
        /// <param name="id">Entity id</param>
        /// <returns>Entity of type T</returns>
        T? GetById(int id);

        /// <summary>
        /// Get all entities
        /// </summary>
        /// <returns>List of entities</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Find entities by expression
        /// </summary>
        /// <param name="predicate">Expression to match</param>
        /// <returns>List of matching entities</returns>
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Add entity
        /// </summary>
        /// <param name="entity">Entity to add</param>
        void Add(T entity);

        /// <summary>
        /// Add range of entities
        /// </summary>
        /// <param name="entities">Entities to add</param>
        void AddRange(IEnumerable<T> entities);

        /// <summary>
        /// Remove entity
        /// </summary>
        /// <param name="entity">Entity to remove</param>
        void Remove(T entity);

        /// <summary>
        /// Remove range of entities
        /// </summary>
        /// <param name="entities">Entities to remove</param>
        void RemoveRange(IEnumerable<T> entities);

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity to update</param>
        void Update(T entity);
    }
}