using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseSqlite("Data Source=tasks.db"));
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();



app.MapGet("/api/tasks", async (ITaskService service) =>
{
    return service.GetAllAsync();
})
.WithName("GetTasks");

app.MapPost("/api/tasks",async (ITaskService service, TaskItem task) =>
{
    var createdTask =await service.CreateAsync(task);

    return Results.Created(
        $"/api/tasks/{createdTask.Id}",
        createdTask
    );
});

app.MapGet("/api/tasks/{id}",async (ITaskService service, int id) =>
{
    var task =await service.GetByIdAsync(id);

    return task is null
        ? Results.NotFound()
        : Results.Ok(task);
});

app.MapPut("/api/tasks/{id}",async (ITaskService service, int id, TaskItem updatedTask) =>
{
    var updated =await service.UpdateAsync(id, updatedTask);
    // var task = tasks.FirstOrDefault(t => t.Id == id);
    
    // task.Name = updatedTask.Name;
    // task.IsComplete = updatedTask.IsComplete;

    return updated
        ? Results.NoContent()
        : Results.NotFound();
});

app.MapDelete("/api/tasks/{id}",async (ITaskService service, int id) =>
{
    // var task = tasks.FirstOrDefault(t => t.Id == id);
 
    return await service.DeleteAsync(id)
        ? Results.NoContent()
        : Results.NotFound();
});

app.MapDelete("/api/tasks",async (ITaskService service) =>
{
    return await service.DeleteAllAsync()
        ? Results.NoContent()
        : Results.NotFound();
});

app.Run();

