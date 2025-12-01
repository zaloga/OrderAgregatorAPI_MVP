using WebAPI.Services;

namespace WebAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Controllers
        builder.Services.AddControllers();

        // Swagger/OpenAPI
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // In-memory implementation of IOrderStore (can be replaced with other Order Store later)
        builder.Services.AddSingleton<IOrderStore, InMemoryOrderStore>();
        
        // Background worker that periodically flushes aggregates into console
        builder.Services.AddHostedService<OrderFlushBackgroundService>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.MapControllers();

        app.Run();
    }
}
