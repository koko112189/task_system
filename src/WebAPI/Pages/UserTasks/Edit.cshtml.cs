using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Threading.Tasks;

namespace TasksWeb.Pages.UserTasks
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public EditModel(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            _httpClient = httpClientFactory.CreateClient("TasksApi");
            _mapper = mapper;
        }

        [BindProperty]
        public UserTask Task { get; set; }

        private UpdateTaskDto TaskDto { get; set; }
        public List<User> Users { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/tasks/{id}");
            if (response.IsSuccessStatusCode)
            {
                Task = await response.Content.ReadFromJsonAsync<UserTask>();
                var response_users = await _httpClient.GetAsync("api/auth");
                if (response_users.IsSuccessStatusCode)
                {
                    Users = await response_users.Content.ReadFromJsonAsync<List<User>>();
                }
                else
                {
                    Users = new List<User>();
                }
                return Page();
            }
            else
            {
                return NotFound();
            }

            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}
            var _task = _mapper.Map<UpdateTaskDto>(Task);

            var response = await _httpClient.PutAsJsonAsync($"api/tasks", _task);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error updating task.");
                return Page();
            }
        }
    }
}
