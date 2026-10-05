using Microsoft.EntityFrameworkCore;

public class TaskRepository : ITaskRepository
{
    private readonly TaskDbContext _context;

    public TaskRepository(TaskDbContext context)
    {
        _context = context;
    }

    public async Task<List<TaskItem>> GetAllAsync()
    {
        return await _context.Tasks.ToListAsync();
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        return await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<TaskItem> AddAsync(TaskItem task)
    {
        _context.Tasks.Add(task);

        await _context.SaveChangesAsync();

        return task;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var task = await GetByIdAsync(id);

        if (task is null)
        {
            return false;
        }

        _context.Tasks.Remove(task);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateAsync(int id, TaskItem updatedTask)
    {
        var task = await GetByIdAsync(id);

        if (task is null)
        {
            return false;
        }

        task.Name = updatedTask.Name;
        task.IsComplete = updatedTask.IsComplete;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAllAsync()
    {
        var tasks = await _context.Tasks.ToListAsync();

        if (tasks.Count == 0)
        {
            return false;
        }

        _context.Tasks.RemoveRange(tasks);

        await _context.SaveChangesAsync();

        return true;
    }
}