using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using CsvHelper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CsvProcessingService : ICsvProcessingService
    {

        private readonly ITaskService _applicantService;
        private readonly IMapper _mapper;

        public CsvProcessingService(ITaskService applicantService, IMapper mapper)
        {
            _applicantService = applicantService;
            _mapper = mapper;
        }

        public async Task<(List<TaskDto> SuccessfulRecords, List<(TaskDto Record, string Error)> FailedRecords)> ProcessCsvAsync(IFormFile file)
        {
            var successfulRecords = new List<TaskDto>();
            var failedRecords = new List<(TaskDto Record, string Error)>();

            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<TaskDto>().ToList();

                if (records == null || !records.Any())
                {
                    throw new ArgumentException("El archivo CSV no contiene datos válidos.");
                }

                foreach (var applicantDto in records)
                {
                    try
                    {
                        var existingApplicant = await _applicantService.GetTaskByIdAsync(applicantDto.AssignedUserId);
                        if (existingApplicant != null)
                        {
                            failedRecords.Add((applicantDto, "El documento ya existe."));
                            continue;
                        }

                        var applicant = _mapper.Map<UserTask>(applicantDto);
                        await _applicantService.CreateTaskAsync(applicant);
                        successfulRecords.Add(applicantDto);
                    }
                    catch (DbUpdateException ex)
                    {
                        if (ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2627 || sqlEx.Number == 2601))
                        {
                            failedRecords.Add((applicantDto, "El documento ya existe."));
                        }
                        else
                        {
                            failedRecords.Add((applicantDto, "Error al guardar el registro."));
                        }
                    }
                    catch (Exception ex)
                    {
                        failedRecords.Add((applicantDto, $"Error inesperado: {ex.Message}"));
                    }
                }
            }

            return (successfulRecords, failedRecords);
        }
    }
}

