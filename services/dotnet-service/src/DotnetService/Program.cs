var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => new { message = "Hello from .NET service!" });
app.MapGet("/health", () => new { status = "ok" });

app.Run();

// Expose for testing
public partial class Program { }
