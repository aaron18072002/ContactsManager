using Entities;
using Microsoft.EntityFrameworkCore;
using Services;
using ServicesContracts.Interfaces;

namespace ContactsManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddSingleton<ICountriesService, CountriesService>();
            builder.Services.AddSingleton<IPersonsService, PersonsService>();

            builder.Services.AddDbContext<ContactsManagerDbContext>(options =>
            {
                options.UseSqlServer();
            });

            var app = builder.Build();

            if(app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.MapControllers();

            app.Run();
        }
    }
}
