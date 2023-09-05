using Microsoft.EntityFrameworkCore;
using SavingsManagementSystem.Data.Contexts;
using SavingsManagementSystem.Repository.Interfaces;

namespace SavingsManagementSystem.Repository.Implementations
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		private readonly DbSet<T> _dbSet;
		public GenericRepository(SavingsDBContext context)
		{
			_dbSet = context.Set<T>();
		}

		public async Task CreateAsync(T entity)
		{
			await _dbSet.AddAsync(entity);
		}

		public void Update(T entity)
		{
			_dbSet.Update(entity);
		}

		public void Delete(T entity)
		{
			_dbSet.Remove(entity);
		}
	}
}
