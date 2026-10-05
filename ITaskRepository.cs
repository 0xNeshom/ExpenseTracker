public interface ITaskRepository
{
    List<TaskItem> GetAll();
    TaskItem? GetById(int id);
    void Add(TaskItem task);
    bool Update(int id, TaskItem updatedTask);
    bool Delete(int id);
}