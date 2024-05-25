using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Repository
{

	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly ApplicationDbContext _db;
		internal DbSet<T> dbset;
        public Repository(ApplicationDbContext db)
        {
			// i take the set or 'collection' from my db
			_db = db;           
			this.dbset=_db.Set<T>();
        }

        public IEnumerable<T> GetAll()
		{
			//get the whole set and return it
			IQueryable<T> query = dbset;
			return query.ToList();
		}

		public T Get(Expression<Func<T, bool>> filter)
		{
			// this where i set the whole set to query
			// then tell it to loop on it using my condition
			// and get the first match or default
			IQueryable<T> query = dbset;
			query=query.Where(filter);
			return query.FirstOrDefault();
		}

		public void Add(T entity)
		{
			dbset.Add(entity);
		}

		public void Remove(T entity)
		{
			dbset.Remove(entity);
		}

		public void RemoveRange(IEnumerable<T> entities)
		{
			dbset.RemoveRange(entities);
		}
	}
}
