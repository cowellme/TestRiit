using System.Reflection;
using Backend.Services;
using Bakcend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

namespace Bakcend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Hardware API",
                    Version = "v1",
                    Description = "Сервис управления техникой: CRUD "
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
            });


            var connectionString = builder.Configuration.GetConnectionString("Default")
                ?? throw new Exception("Не задана строка подключения");


            

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            builder.Services.AddCors(options =>
                options.AddDefaultPolicy(policy =>
                    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));


            builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            builder.Services.AddScoped<ITechnicTypeService, TechnicTypeService>();
            builder.Services.AddScoped<ITechnicService, TechnicService>();
            builder.Services.AddHostedService<DatabaseInitializer>();


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                
            }
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseCors();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
