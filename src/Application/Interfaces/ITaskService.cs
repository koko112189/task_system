using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITaskService
    {
        Task CreateTaskAsync(UserTask task);
        Task<IEnumerable<UserTask>> GetTasksAsync();
        Task<UserTask?> GetTaskByIdAsync(Guid id);
        Task UpdateTaskAsync(UserTask task);
        Task DeleteTaskAsync(Guid id);
    }
}
