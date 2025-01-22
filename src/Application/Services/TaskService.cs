using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata.Ecma335;

namespace Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TaskService( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateTaskAsync(UserTask task)
        {  
            try
            {
                await _unitOfWork.TaskRepository.AddAsync(task);
                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task DeleteTaskAsync(Guid id)
        {
            var existingTask = await _unitOfWork.TaskRepository.GetByIdAsync(id);
            if (existingTask == null)
            {
                throw new TaskNotFoundException(id);
            }
            await _unitOfWork.TaskRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<UserTask?> GetTaskByIdAsync(Guid id)
        {
            return await _unitOfWork.TaskRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<UserTask>> GetTasksAsync()
        {
            try
            {
               return await _unitOfWork.TaskRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task UpdateTaskAsync(UserTask task)
        {
            try
            {
                var existingTask = await _unitOfWork.TaskRepository.GetByIdAsync(task.Id);
                if (existingTask == null)
                {
                    throw new TaskNotFoundException(task.Id);
                }
                await _unitOfWork.TaskRepository.UpdateAsync(task);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
    }
}
