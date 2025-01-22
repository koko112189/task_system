using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using CsvHelper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;
using System.Globalization;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskServiceService;
        private readonly IMapper _mapper;
        private readonly ICsvProcessingService _csvProcessingService;

        public TasksController(ITaskService taskService, IMapper mapper, ICsvProcessingService csvProcessingService)
        {
            _taskServiceService = taskService;
            _mapper = mapper;
            _csvProcessingService = csvProcessingService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskDto task)
        {
            try
            {
                var _task = _mapper.Map<UserTask>(task);

                await _taskServiceService.CreateTaskAsync(_task);
                return Ok(_task);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2627 || sqlEx.Number == 2601))
                {
                    return StatusCode(409, "El documento ya existe."); 
                }

                return StatusCode(500, "Ocurrió un error inesperado al guardar el aplicante.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var task = await _taskServiceService.GetTaskByIdAsync(id);
            return task == null ? NotFound() : Ok(_mapper.Map<TaskDto>(task));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await _taskServiceService.GetTasksAsync();
            return Ok(tasks);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _taskServiceService.DeleteTaskAsync(id);
            return NoContent();
        }

        [HttpPost("upload-csv")]
        public async Task<IActionResult> UploadCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No se ha proporcionado un archivo válido.");
            }

            try
            {
                var (successfulRecords, failedRecords) = await _csvProcessingService.ProcessCsvAsync(file);

                if (failedRecords.Any())
                {
                    return Ok(new
                    {
                        Message = "Algunos registros no se pudieron guardar.",
                        SuccessfulCount = successfulRecords.Count,
                        FailedCount = failedRecords.Count,
                        SuccessfulRecords = successfulRecords,
                        FailedRecords = failedRecords.Select(f => new
                        {
                            Record = f.Record,
                            Error = f.Error
                        }).ToList()
                    });
                }
                else
                {
                    return Ok(new
                    {
                        Message = "Todos los registros se guardaron correctamente.",
                        SuccessfulCount = successfulRecords.Count,
                        SuccessfulRecords = successfulRecords
                    });
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error inesperado: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> updateStatus(UserTask task)
        {
            await _taskServiceService.UpdateTaskAsync(task);
            return NoContent();
        }
    }
}
