using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Controllers;
using Xunit;

namespace WebAPI.Tests.Controllers
{
    public class TasksControllerTests
    {
        private readonly Mock<ITaskService> _mockTaskService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ICsvProcessingService> _mockCsvProcessingService;
        private readonly TasksController _controller;

        public TasksControllerTests()
        {
            _mockTaskService = new Mock<ITaskService>();
            _mockMapper = new Mock<IMapper>();
            _mockCsvProcessingService = new Mock<ICsvProcessingService>();
            _controller = new TasksController(_mockTaskService.Object, _mockMapper.Object, _mockCsvProcessingService.Object);
        }

        [Fact]
        public async Task Create_ValidTask_ReturnsOkResult()
        {
            // Arrange
            var taskDto = new TaskDto { Title = "Test Task", Description = "Test Description" };
            var userTask = new UserTask { Id = Guid.NewGuid(), Title = "Test Task", Description = "Test Description" };

            _mockMapper.Setup(m => m.Map<UserTask>(taskDto)).Returns(userTask);
            _mockTaskService.Setup(s => s.CreateTaskAsync(userTask)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(taskDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(userTask, okResult.Value);
        }

   

        [Fact]
        public async Task GetById_ExistingTask_ReturnsOkResult()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var userTask = new UserTask { Id = taskId, Title = "Test Task", Description = "Test Description" };

            _mockTaskService.Setup(s => s.GetTaskByIdAsync(taskId)).ReturnsAsync(userTask);

            // Act
            var result = await _controller.GetById(taskId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(userTask, okResult.Value);
        }

        [Fact]
        public async Task GetById_NonExistingTask_ReturnsNotFound()
        {
            // Arrange
            var taskId = Guid.NewGuid();

            _mockTaskService.Setup(s => s.GetTaskByIdAsync(taskId)).ReturnsAsync((UserTask)null);

            // Act
            var result = await _controller.GetById(taskId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResultWithTasks()
        {
            // Arrange
            var tasks = new List<UserTask>
            {
                new UserTask { Id = Guid.NewGuid(), Title = "Task 1", Description = "Description 1" },
                new UserTask { Id = Guid.NewGuid(), Title = "Task 2", Description = "Description 2" }
            };

            _mockTaskService.Setup(s => s.GetTasksAsync()).ReturnsAsync(tasks);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(tasks, okResult.Value);
        }

        [Fact]
        public async Task Delete_ExistingTask_ReturnsNoContent()
        {
            // Arrange
            var taskId = Guid.NewGuid();

            _mockTaskService.Setup(s => s.DeleteTaskAsync(taskId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(taskId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

      

        [Fact]
        public async Task UploadCsv_InvalidFile_ReturnsBadRequest()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(0);

            // Act
            var result = await _controller.UploadCsv(fileMock.Object);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateStatus_ValidTask_ReturnsNoContent()
        {
            // Arrange
            var updateTaskDto = new UpdateTaskDto { Id = Guid.NewGuid(), Status = "Completed" };
            var userTask = new UserTask { Id = updateTaskDto.Id, Status = updateTaskDto.Status };

            _mockMapper.Setup(m => m.Map<UserTask>(updateTaskDto)).Returns(userTask);
            _mockTaskService.Setup(s => s.UpdateTaskAsync(userTask)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.updateStatus(updateTaskDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}