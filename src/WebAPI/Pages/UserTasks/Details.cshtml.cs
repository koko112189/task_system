using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TasksWeb.Pages.UserTasks
{
    public class DetailsModel : PageModel
    {
        private readonly ITaskService _taskService;

        public DetailsModel(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public UserTask Task { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Task = await _taskService.GetTaskByIdAsync(id);
            if (Task == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
