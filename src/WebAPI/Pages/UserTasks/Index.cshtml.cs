using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;

namespace TasksWeb.Pages.UserTasks
{
    public class IndexModel : PageModel
    {
        private readonly ITaskService _taskService;
        private readonly HttpClient _httpClient;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("TasksApi");
        }

        public IEnumerable<UserTask> Tasks { get; set; }

        public async Task OnGetAsync()
        {
            var response = await _httpClient.GetAsync("api/tasks");
            if (response.IsSuccessStatusCode)
            {
                Tasks = await response.Content.ReadFromJsonAsync<List<UserTask>>();
            }
            else
            {
                Tasks = new List<UserTask>();
            }
        }
    }
}
