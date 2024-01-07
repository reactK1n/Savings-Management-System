using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;

namespace SavingsManagementSystem.Repository.Interfaces
{
	public interface IGenericRepository<T> where T : class
	{
		Task CreateAsync(T entity);

		void Update(T entity);

		void Delete(T entity);
	}
}
