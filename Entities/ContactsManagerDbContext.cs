using Microsoft.Data.SqlClient;
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
        public ContactsManagerDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }
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
            using (var streamReader = new StreamReader("countries.json"))
            {
                string? line;
                while ((line = streamReader.ReadLine()) is not null)
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

            //Fluent API
            modelBuilder.Entity<Person>().Property(p => p.TIN)
                .HasColumnName("TIN")
                .HasColumnType("varchar(8)")
                .HasDefaultValue("ABC12345")
                .IsRequired(false);

            modelBuilder.Entity<Person>()
                .HasCheckConstraint("CHK_TIN", "len([TIN])=8");

            //Tables relationship
            modelBuilder.Entity<Person>()
                .HasOne<Country>(p => p.Country)
                .WithMany(c => c.Persons)
                .HasForeignKey(p => p.CountryId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public List<Person> sp_GetAllPersons()
        {
            var result = this?.Persons?.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();

            return result ?? new List<Person>();
        }

        public int sp_InsertPerson(Person person)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@PersonId", person.PersonId),
                new SqlParameter("@PersonName", person.PersonName),
                new SqlParameter("@Email", person.Email),
                new SqlParameter("@DateOfBirth", person.DateOfBirth),
                new SqlParameter("@Gender", person.Gender),
                new SqlParameter("@CountryId", person.CountryId),
                new SqlParameter("@Address", person.Address),
                new SqlParameter("@ReceiveNewsLetters", person.ReceiveNewsLetters),
            };

            var result = this.Database.ExecuteSqlRaw
                ("EXECUTE [dbo].[InsertPerson] @PersonId, @PersonName, @Email, @DateOfBirth, @Gender, @CountryId, @Address, @ReceiveNewsLetters", parameters);

            return result;
        }
    }
}
