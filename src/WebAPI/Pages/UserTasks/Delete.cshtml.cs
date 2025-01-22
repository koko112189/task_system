        using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TasksWeb.Pages.UserTasks
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public DeleteModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("TasksApi");
        }

        [BindProperty]
        public UserTask Task { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/tasks/{id}");
            if (response.IsSuccessStatusCode)
            {
                Task = await response.Content.ReadFromJsonAsync<UserTask>();
                return Page();
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/tasks/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error deleting task.");
                return Page();
            }
        }
    }
}
