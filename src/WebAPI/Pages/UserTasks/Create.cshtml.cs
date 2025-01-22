using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TasksWeb.Pages.UserTasks
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("TasksApi");
        }

        [BindProperty]
        public TaskDto Task { get; set; }

        public List<User> Users { get; set; }

       


        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}
            

            var response = await _httpClient.PostAsJsonAsync("api/tasks", Task);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error creating task.");
                return Page();
            }
        }

        public async Task OnGetAsync()
        {
            var response = await _httpClient.GetAsync("api/auth");
            if (response.IsSuccessStatusCode)
            {
                Users = await response.Content.ReadFromJsonAsync<List<User>>();
            }
            else
            {
                Users = new List<User>();
            }
        }
    }
}
