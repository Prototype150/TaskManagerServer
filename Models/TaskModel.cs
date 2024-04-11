using System.Text.Json.Serialization;

namespace Models
{
    public enum TaskState
    {
        InProgress,
        Completed,
        Failed,
        Extended,
        ExtendedCompleted
    }

    public class TaskModel
    {
        [JsonConstructor]
        public TaskModel(int accountId, string task, int sortId, TaskState state, DateOnly dueDate) {
            AccountId = accountId;
            Task = task;
            SortId = sortId;
            State = state;
            DueDate = dueDate;
        }

        public TaskModel(int accountId, string task, int sortId, DateOnly dueDate) : this(accountId, task, sortId, TaskState.InProgress, dueDate) {}
        public TaskModel(int accountId, string task, DateOnly dueDate) : this(accountId, task, 0, TaskState.InProgress, dueDate) { }
        public TaskModel(int accountId, string task, TaskState state, DateOnly dueDate) : this(accountId, task, 0, state, dueDate) { }

        public int Id { get; set; }
        public string Task { get; set; }
        public int AccountId { get; set; }
        public int SortId { get; set; }
        public bool IsCompleted { get; set; }
        public TaskState State { get; set; }
        public DateOnly DueDate { get; set; }
    }
}
