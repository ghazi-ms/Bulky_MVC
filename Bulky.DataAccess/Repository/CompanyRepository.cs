using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
	public class CompanyRepository :Repository<Company>, ICompanyRepository
	{
		private ApplicationDbContext _db;
		public CompanyRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(Company obj)
		{
			ArgumentNullException.ThrowIfNull(obj);
			_db.Companies.Update(obj);
		}
	}
}
