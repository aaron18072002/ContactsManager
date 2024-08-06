using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Entities
{
    public class ContactsManagerDbContext : DbContext
    {
        public DbSet<Country>? Countries { get; set; }
        public DbSet<Person>? Persons { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Person>().ToTable("Persons");

            //Seed data for Persons table
            var personsFromJson = new StringBuilder();
            using(var streamReader = new StreamReader("persons.json"))
            {
                string? line;
                while ((line = streamReader.ReadLine()) is not null)
                {
                    personsFromJson.AppendLine(line);
                }
            }
            var listPersons = JsonSerializer.Deserialize<List<Person>>(personsFromJson.ToString());
            if(listPersons is not null)
            {
                foreach (var person in listPersons)
                {
                    modelBuilder.Entity<Person>().HasData(person); 
                }
            }

            //Seed data for Countries table
            var countriesFromJson = new StringBuilder();
            using(var streamReader = new StreamReader("countries.json"))
            {
                string? line;
                while((line = streamReader.ReadLine()) is not null)
                {
                    countriesFromJson.AppendLine(line);
                }
            }
            var listCountries = JsonSerializer.Deserialize<List<Country>>(countriesFromJson.ToString());
            if( listCountries is not null)
            {
                foreach (var country in listCountries)
                {
                    modelBuilder.Entity<Country>().HasData(country);
                }
            }
        }
    }
}
