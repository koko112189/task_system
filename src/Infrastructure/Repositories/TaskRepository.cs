using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TaskRepository : ITasksRepository
    {
        private readonly TasksDbContext _context;

        public TaskRepository(TasksDbContext context)
        {
            _context = context;
        }

        public async Task<UserTask?> GetByIdAsync(Guid id)
        {
            var task = await _context.Tasks 
            .FirstOrDefaultAsync(r => r.Id == id);

            return task;
        }

        public async Task<IEnumerable<UserTask>> GetAllAsync()
        {
            var query =  _context.Tasks.AsNoTracking().AsQueryable();

            var tasks = await query.ToListAsync();
            return tasks;
        }

        public async Task AddAsync(UserTask task)
        {
            await _context.Tasks.AddAsync(task);
        }

        public async Task DeleteAsync(Guid id)
        {
            var task = await GetByIdAsync(id);
            if (task != null)
                _context.Tasks.Remove(task);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserTask task)
        {
            var existingTask = await GetByIdAsync(task.Id);
            if (existingTask != null)
            {
                _context.Entry(existingTask).CurrentValues.SetValues(task);
            }
        }
    }
}
