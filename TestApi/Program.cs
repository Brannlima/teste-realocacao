using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using System.IO;
using TestApi.Models;
using TestApi.Data;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        builder.Services.AddDbContext<ApiContext>(
            options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
        );

        builder.Services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            });

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "TestApi", Version = "v1" });
        });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(
                "AllowAnyOrigin",
                builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
            );
        });

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestApi v1");
            c.RoutePrefix = string.Empty;
        });

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseCors("AllowAnyOrigin");

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseExceptionHandler("/error");

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.Run();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });
    }

    private static void Configure(IApplicationBuilder app)
    {
        app.UseExceptionHandler("/error");
    }
}
