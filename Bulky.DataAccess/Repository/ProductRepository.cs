using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bulky.DataAccess.Data;

namespace Bulky.DataAccess.Repository
{
	public class ProductRepository: Repository<Product>, IProductRepository
	{
		private ApplicationDbContext _db;
		public ProductRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(Product obj)
		{ 
			var productFromDB= _db.Products.FirstOrDefault(p => p.Id == obj.Id);
			if (productFromDB != null) { 
				productFromDB.Title = obj.Title;
				productFromDB.Description = obj.Description;
				productFromDB.ISBN = obj.ISBN;
				productFromDB.Author = obj.Author;
				productFromDB.Price100 = obj.Price100;
				productFromDB.Price50 = obj.Price50;
				productFromDB.Price=obj.Price;
				productFromDB.CategoryID = obj.CategoryID;
				productFromDB.ListPrice= obj.ListPrice;
				if(obj.ImageUrl!= null)
				{
					productFromDB.ImageUrl = obj.ImageUrl;
				}
			}
		}
	}
}
