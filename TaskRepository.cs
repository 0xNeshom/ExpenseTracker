// namespace TaskTracker.Api;


public class TaskRepository : ITaskRepository{
    private readonly List<TaskItem> _tasks;

    public TaskRepository()
    {
        _tasks = new List<TaskItem>
        {
            new TaskItem { Id = 1, Name = "Task 1", IsComplete = false },
            new TaskItem { Id = 2, Name = "Task 2", IsComplete = true },
            new TaskItem { Id = 3, Name = "Task 3", IsComplete = false }
        };
    }

    public List<TaskItem> GetAll()
    {
    return _tasks;
    }
    public TaskItem? GetById(int id)
    {
        return _tasks.FirstOrDefault(t => t.Id == id);
    }
    public void Add(TaskItem task)
    {
        _tasks.Add(task);
    }
    public bool Delete(int id)
{
    var task = GetById(id);

    if (task is null)
    {
        return false;
    }

    _tasks.Remove(task);
    return true;
}

public bool Update(int id, TaskItem updatedTask)
{
    var task = GetById(id);

    if (task is null)
    {
        return false;
    }

    task.Name = updatedTask.Name;
    task.IsComplete = updatedTask.IsComplete;

    return true;
}
}
