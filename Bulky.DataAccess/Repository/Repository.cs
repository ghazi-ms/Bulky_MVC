﻿using System;
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
			_db.Products.Include(u => u.Category).Include(u => u.CategoryID);
        }

        public IEnumerable<T> GetAll(string? includeProperties=null)
		{
			//get the whole set and return it
			IQueryable<T> query = dbset;
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var propertie in includeProperties.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
                {
                    query=query.Include(propertie);
                }
            }
            return query.ToList();
		}

		public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
		{
			// this where i set the whole set to query
			// then tell it to loop on it using my condition
			// and get the first match or default
			IQueryable<T> query = dbset;
			query=query.Where(filter);
			if (!string.IsNullOrEmpty(includeProperties))
			{
				foreach (var propertie in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(propertie);
				}
			}
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
