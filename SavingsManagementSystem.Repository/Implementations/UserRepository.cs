﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SavingsManagementSystem.Data.Contexts;
using SavingsManagementSystem.Model;
using SavingsManagementSystem.Repository.Interfaces;

namespace SavingsManagementSystem.Repository.Implementations
{
	public class UserRepository : GenericRepository<ApplicationUser> , IUserRepository
	{
		private readonly DbSet<ApplicationUser> _dbSet;
		private readonly SavingsDBContext _context;

		public UserRepository(SavingsDBContext context) : base(context)
		{
			_context = context;
			_dbSet = context.Set<ApplicationUser>();
		}

		public async Task Create(ApplicationUser user)
		{
			await _dbSet.AddAsync(user);
		}

		public ICollection<ApplicationUser> FetchUser()
		{
			return _dbSet.ToList();
		}

		public async Task<ApplicationUser> FetchUserAsync(string userId)
		{
			var user = await _dbSet.FindAsync(userId);
			return user;
		}

		public new void Update(ApplicationUser user)
		{
			_dbSet.Update(user);
		}

		public new void Delete(ApplicationUser user)
		{
			_dbSet.Remove(user);
		}

		public async Task<ApplicationUser> GetUserByRefreshTokenAsync(string refreshToken, string userId)
		{
			var user = await _dbSet.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken && u.Id == userId);
			return user;
		}

	}
}
