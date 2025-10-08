using System.Linq.Expressions;
using LiteDB;
using TestCase.Entities;

namespace TestCase.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly ILiteDatabase _database;
        protected readonly ILiteCollection<T> _collection;

        public Repository(ILiteDatabase database)
        {
            _database = database;
            _collection = _database.GetCollection<T>();
        }

        public T? GetById(int id)
        {
            return _collection.FindById(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _collection.FindAll();
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _collection.Find(predicate);
        }

        public void Add(T entity)
        {
            // Set audit fields
            entity.CreatedTime = DateTime.UtcNow;
            entity.UpdatedTime = DateTime.UtcNow;
            
            _collection.Insert(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            // Set audit fields for all entities
            foreach (var entity in entities)
            {
                entity.CreatedTime = DateTime.UtcNow;
                entity.UpdatedTime = DateTime.UtcNow;
            }
            
            _collection.InsertBulk(entities);
        }

        public void Remove(T entity)
        {
            _collection.Delete(entity.Id);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            var ids = entities.Select(e => e.Id);
            _collection.DeleteMany(e => ids.Contains(e.Id));
        }

        public void Update(T entity)
        {
            // Update audit fields
            entity.UpdatedTime = DateTime.UtcNow;
            
            _collection.Update(entity);
        }
    }
}