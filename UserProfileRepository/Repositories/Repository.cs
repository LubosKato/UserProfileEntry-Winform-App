using System.Collections.Generic;
using System.Data;

namespace UserProfileRepository.Repositories
{
    public abstract class Repository<TEntity> where TEntity : new()
    {
        readonly DbContext _context;

        protected Repository(DbContext context)
        {
            _context = context;
        }

        protected DbContext Context { get; }

        protected IEnumerable<TEntity> ToList(IDbCommand command)
        {
            using (var reader = command.ExecuteReader())
            {
                List<TEntity> items = new List<TEntity>();
                while (reader.Read())
                {
                    var item = new TEntity();
                    Map(reader, item);
                    items.Add(item);
                }
                return items;
            }
        }

        protected abstract void Map(IDataRecord record, TEntity entity);
    }
}
