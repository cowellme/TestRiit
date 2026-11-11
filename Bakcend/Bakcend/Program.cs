using Backend.Extensions;

namespace Bakcend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseAppSerilog();

            builder.Services.AddControllers();

            builder.Services.AddDatabase(builder.Configuration);

            builder.Services.AddApplicationServices();

            builder.Services.AddCorsPolicy();

            builder.Services.AddApiDocs();

            var app = builder.Build();

            app.UseApplicationPipeline();

            app.Run();
        }
    }
}
