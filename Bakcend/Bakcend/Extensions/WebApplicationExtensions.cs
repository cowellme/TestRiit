using Serilog;

namespace Backend.Extensions;

public static class WebApplicationExtensions
{
    public static void UseApplicationPipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
        app.MapOpenApi();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseCors();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
    }
}