var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => new { Message = "Hello from .NET Minimal API!", Status = "Running" });

app.MapGet("/tasks", () => new[] {
    new { Id = 1, Title = "Learn Docker", IsCompleted = true }
});

app.Run();