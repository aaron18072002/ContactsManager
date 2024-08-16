using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoriesContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class PersonsRepository : IPersonsRepository
    {
        private readonly ContactsManagerDbContext _db;
        private readonly ILogger<PersonsRepository> _logger;

        public PersonsRepository
            (ContactsManagerDbContext db, ILogger<PersonsRepository> logger)
        {
            this._db = db;
            this._logger = logger;
        }

        public async Task<Person> AddPerson(Person person)
        {
            if(this._db.Persons is not null)
            {
                this._db.Persons.Add(person);
                await this._db.SaveChangesAsync();
            }

            return person;
        }

        public async Task<bool> DeletePerson(Guid personId)
        {
            if(this._db.Persons is not null)
            {
                var matchingPerson = await this._db.Persons.FirstOrDefaultAsync(p => p.PersonId == personId);
                if(matchingPerson is not null)
                {
                    this._db.Persons.Remove(matchingPerson);
                    var rowAffected = await this._db.SaveChangesAsync();

                    return rowAffected > 0;
                }
            }

            return false;
        }

        public async Task<List<Person>> GetAllPersons()
        {
            this._logger.LogInformation("GetAllPersons from PersonsRepository");

            return this._db.Persons is not null ?
                await this._db.Persons.Include("Country").ToListAsync() : new List<Person>();
        }

        public async Task<List<Person>?> GetFilteredPersons(Expression<Func<Person, bool>> predicate)
        {
            var result = this._db.Persons is not null ?
                await this._db.Persons.Where(predicate).ToListAsync() : new List<Person>();

            return result;
        }

        public async Task<Person?> GetPersonById(Guid personId)
        {
            var result = this._db.Persons is not null ? 
                await this._db.Persons.FirstOrDefaultAsync(p => p.PersonId == personId) : null;

            return result;
        }

        public async Task<Person> UpdatePerson(Person person)
        {
            var matchingPerson = this._db.Persons is not null ?
                await this._db.Persons.FirstOrDefaultAsync(p => p.PersonId == person.PersonId) : null;
            if(matchingPerson is not null)
            {
                matchingPerson.PersonName = person.PersonName;
                matchingPerson.Email = person.Email;
                matchingPerson.Gender = person.Gender;
                matchingPerson.Address = person.Address;
                matchingPerson.CountryId = person.CountryId;
                matchingPerson.DateOfBirth = person.DateOfBirth;
                matchingPerson.ReceiveNewsLetters = person.ReceiveNewsLetters;

                await this._db.SaveChangesAsync();

                return matchingPerson;
            } 

            return person;
        }
    }
}
