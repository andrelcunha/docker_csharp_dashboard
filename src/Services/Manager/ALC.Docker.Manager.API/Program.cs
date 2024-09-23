
using ALC.Docker.Manager.API.Extensions;
using ALC.Docker.Manager.API.Service;

namespace ALC.Docker.Manager.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //Get AppSettings
        var appSettingsSection = builder.Configuration.GetSection("AppSetting");
        builder.Services.Configure<AppSetting>(appSettingsSection);


        // Add services to the container.
        builder.Services.AddScoped<IDockerService, DockerServiceUnix>();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
