
namespace Domain.Exceptions
{
    public class TaskNotFoundException : Exception
    {

        public TaskNotFoundException(Guid taskId)
            : base($"The task with ID '{taskId}' was not found.")
        {
        }

    }
}