using TaskManagerServer.Models.Validations;

namespace TaskManagerServer.Models
{
    [TaskValidation]
    public class TaskModel
    {
        public TaskModel(int accountId, string task) {
            AccountId = accountId;
            Task = task;
        }

        public int Id { get; set; }
        public string Task { get; set; }
        public int AccountId { get; set; }
    }
}
