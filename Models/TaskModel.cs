namespace Models
{
    public class TaskModel
    {
        public TaskModel(int accountId, string task, int sortId) {
            AccountId = accountId;
            Task = task;
            SortId = sortId;
        }

        public int Id { get; set; }
        public string Task { get; set; }
        public int AccountId { get; set; }
        public int SortId { get; set; }
        public bool IsCompleted { get; set; }
    }
}
