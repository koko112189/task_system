using Domain.Entities;

namespace Application.Interfaces
{
    public interface ITasksRepository
    {
        Task<UserTask> GetByIdAsync(Guid id);
        Task<IEnumerable<UserTask>> GetAllAsync();
        Task AddAsync(UserTask task);
        Task UpdateAsync(UserTask task);
        Task DeleteAsync(Guid id);
        Task SaveChangesAsync();
    }
}
