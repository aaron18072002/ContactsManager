using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoriesContracts;
using Services;
using ServicesContracts.Interfaces;

namespace ContactsManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.ConfigureLogging(logBuilder =>
            {
                logBuilder.ClearProviders();
                logBuilder.AddConsole();
                logBuilder.AddDebug();
            });

            builder.Services.AddHttpLogging(logging => { });

            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<ICountriesService, CountriesService>();
            builder.Services.AddScoped<IPersonsService, PersonsService>();

            builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
            builder.Services.AddScoped<IPersonsRepository, PersonsRepository>();

            builder.Services.AddDbContext<ContactsManagerDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? "");
            });

            var app = builder.Build();

            app.UseHttpLogging();

            if(app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");

            app.UseStaticFiles();
            app.UseRouting();
            app.MapControllers();

            app.Run();
        }
    }
}
