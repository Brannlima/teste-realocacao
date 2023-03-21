using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using System.IO;
using TestApi.Models;
using Microsoft.OpenApi.Models;

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

        builder.Services.AddControllers();

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "TestApi", Version = "v1" });
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

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseExceptionHandler("/error"); // Adiciona o middleware de tratamento de erros

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGet(
                "/",
                async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                }
            );
        });

        app.Run();
    }

    private static void Configure(IApplicationBuilder app)
    {
        // O middleware de roteamento de endpoint já foi adicionado no método Main
    }
}
