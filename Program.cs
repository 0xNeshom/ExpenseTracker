// using TaskTracker.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<ITaskRepository, TaskRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


var nextId = 4;

app.MapGet("/api/tasks", (ITaskRepository repository) =>
{
    return repository.GetAll();
})
.WithName("GetTasks");

app.MapPost("/api/tasks", (ITaskRepository repository, TaskItem task) =>
{
    
    var createdTask = new TaskItem
    {
        Id = nextId++,
        Name = task.Name,
        IsComplete = task.IsComplete
    };
    repository.Add(createdTask);

    return Results.Created($"/api/tasks/{createdTask.Id}", createdTask);
})
.WithName("CreateTask")
.Produces<TaskItem>(StatusCodes.Status201Created);


app.MapGet("/api/tasks/{id}", (ITaskRepository repository, int id) =>
{
    var task = repository.GetById(id);

    return task is null
        ? Results.NotFound()
        : Results.Ok(task);
});

app.MapPut("/api/tasks/{id}", (ITaskRepository repository, int id, TaskItem updatedTask) =>
{
    var updated = repository.Update(id, updatedTask);
    // var task = tasks.FirstOrDefault(t => t.Id == id);
    
    // task.Name = updatedTask.Name;
    // task.IsComplete = updatedTask.IsComplete;

    return updated
        ? Results.NoContent()
        : Results.NotFound();
});

app.MapDelete("/api/tasks/{id}", (ITaskRepository repository, int id) =>
{
    // var task = tasks.FirstOrDefault(t => t.Id == id);
 
    return repository.Delete(id)
        ? Results.NoContent()
        : Results.NotFound();
});

app.Run();

public class TaskItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsComplete { get; set; }
}

