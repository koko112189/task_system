using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TasksDbContext _context;

        public UserRepository(TasksDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAsync()
        {
            var query = _context.Users.AsNoTracking().AsQueryable();

            var users = await query.ToListAsync();
            return users;
        }
    }
}
