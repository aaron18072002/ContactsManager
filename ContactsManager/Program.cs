using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoriesContracts;
using Services;
using ServicesContracts.Interfaces;
using Serilog;
using ContactsManager.Filters.ActionFilters;
using ContactsManager.Filters.ResultFilters;
using ContactsManager.StartupExtensions;
using ContactsManager.Middlewares;

namespace ContactsManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //builder.Host.ConfigureLogging(logBuilder =>
            //{
            //    logBuilder.ClearProviders();
            //    logBuilder.AddConsole();
            //    logBuilder.AddDebug();
            //});

            builder.Host.UseSerilog((context, services, loggerConfiguration) =>
            {
                loggerConfiguration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services); //read out current app's services and make them available to serilog
            });

            //builder.Services.AddHttpLogging(logging =>
            //{
            //    logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
            //    logging.RequestHeaders.Add("sec-ch-ua");
            //    logging.ResponseHeaders.Add("MyResponseHeader");
            //    logging.MediaTypeOptions.AddText("application/javascript");
            //    logging.RequestBodyLogLimit = 4096;
            //    logging.ResponseBodyLogLimit = 4096;
            //    logging.CombineLogs = true;
            //});

            //builder.Services.AddTransient<ResponseHeaderActionFilter>();

            //builder.Services.AddControllersWithViews(options =>
            //{
            //    //options.Filters.Add<ResponseHeaderActionFilter>(5); without any parameters

            //    var logger = builder.Services.BuildServiceProvider()
            //        .GetRequiredService<ILogger<ResponseHeaderActionFilter>>();

            //    options.Filters.Add(new ResponseHeaderActionFilter(logger)
            //    {
            //        Key = "My-Key-From-Global",
            //        Value = "My-Key-From-Global",
            //        Order = 2
            //    });

            //    options.Filters.Add<PersonAlwaysRunResultFilter>();
            //});

            //builder.Services.AddScoped<ICountriesService, CountriesService>();
            //builder.Services.AddScoped<IPersonsService, PersonsService>();

            //builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
            //builder.Services.AddScoped<IPersonsRepository, PersonsRepository>();

            //builder.Services.AddDbContext<ContactsManagerDbContext>(options =>
            //{
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? "");
            //});

            builder.Services.ConfigureServices(builder.Configuration);

            var app = builder.Build();

            app.UseHttpLogging();

            if(app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } else
            {
                app.UseExeptionHandlingMiddleware();
            }

            Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");

            app.UseStaticFiles();
            app.UseRouting();
            app.MapControllers();

            app.Run();
        }
    }
}
