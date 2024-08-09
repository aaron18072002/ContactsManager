using Entities;
using Microsoft.EntityFrameworkCore;
using Services.Helpers;
using ServicesContracts.DTOs;
using ServicesContracts.Enums;
using ServicesContracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        private readonly ContactsManagerDbContext _db;
        private readonly ICountriesService _countriesService;
        public PersonsService(ContactsManagerDbContext contactsManagerDbContext)
        {
            this._db = contactsManagerDbContext;
            this._countriesService = new CountriesService(contactsManagerDbContext);
        }
        //private PersonResponse ConvertPersonToPersonResponse(Person person)
        //{
        //    var personResponse = person.ToPersonResponse();
        //    personResponse.CountryName = this._countriesService.GetCountryByCountryId
        //        (personResponse.CountryId)?.CountryName;

        //    return personResponse;
        //}

        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
        {
            if (personAddRequest is null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }

            ValidationsHelper.ModelVaidation(personAddRequest);

            var personEntity = personAddRequest.ToPerson();
            personEntity.PersonId = Guid.NewGuid();

            //this?._db?.Persons?.Add(personEntity);
            //this._db.SaveChanges();

            await this._db.sp_InsertPerson(personEntity);

            var personResponse = personEntity.ToPersonResponse();

            return personResponse;
        }

        public async Task<List<PersonResponse>> GetAllPersons()
        {
            var persons = this._db.Persons is not null ? 
                await this._db.Persons.Include("Country").ToListAsync() : new List<Person>();
            var personsResponse = persons.Select(p => p.ToPersonResponse()).ToList();

            return personsResponse;

            //return this._db.sp_GetAllPersons().Select(p => this.ConvertPersonToPersonResponse(p)).ToList();
        }

        public async Task<PersonResponse?> GetPersonByPersonId(Guid? personId)
        {
            if (personId == null)
            {
                return null;
            }

            var personEntity = this._db.Persons is not null ? 
                await this._db.Persons.Include("Country").FirstOrDefaultAsync(p => p.PersonId == personId) : null;
            if (personEntity == null)
            {
                return null;
            }

            var personResponse = personEntity.ToPersonResponse();

            return personResponse;
        }

        public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
        {
            var allPersons = await this.GetAllPersons();
            var matchingPersons = allPersons;

            if (searchBy is null || searchString is null)
            {
                return allPersons;
            }

            switch (searchBy)
            {
                case (nameof(PersonResponse.PersonName)):
                    matchingPersons = allPersons.Where
                        (p => !string.IsNullOrEmpty(p.PersonName) ? p.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case (nameof(PersonResponse.Email)):
                    matchingPersons = allPersons.Where
                        (p => !string.IsNullOrEmpty(p.Email) ? p.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case (nameof(PersonResponse.DateOfBirth)):
                    matchingPersons = allPersons.Where
                        (p => p.DateOfBirth is not null ? p.DateOfBirth.Value.ToString("dd MMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case (nameof(PersonResponse.Gender)):
                    matchingPersons = allPersons.Where
                        (p => !string.IsNullOrEmpty(p.Gender) ? p.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case (nameof(PersonResponse.CountryId)):
                    matchingPersons = allPersons.Where
                        (p => !string.IsNullOrEmpty(p.CountryName) ? p.CountryName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case (nameof(PersonResponse.Address)):
                    matchingPersons = allPersons.Where
                        (p => !string.IsNullOrEmpty(p.Address) ? p.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                default:
                    matchingPersons = allPersons;
                    break;
            }

            return matchingPersons;
        }

        public List<PersonResponse> GetSortedPersons
            (List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrderOption)
        {
            if (sortBy is null)
            {
                return allPersons;
            }

            var sortedPersons = (sortBy, sortOrderOption) switch
            {
                (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) =>
                    allPersons.OrderBy(p => p.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) =>
                    allPersons.OrderByDescending(p => p.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Email), SortOrderOptions.ASC) =>
                    allPersons.OrderBy(p => p.Email, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Email), SortOrderOptions.DESC) =>
                    allPersons.OrderByDescending(p => p.Email, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) =>
                    allPersons.OrderBy(p => p.DateOfBirth).ToList(),
                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) =>
                    allPersons.OrderByDescending(p => p.DateOfBirth).ToList(),
                (nameof(PersonResponse.Age), SortOrderOptions.ASC) =>
                    allPersons.OrderBy(p => p.Age).ToList(),
                (nameof(PersonResponse.Age), SortOrderOptions.DESC) =>
                    allPersons.OrderByDescending(p => p.Age).ToList(),
                (nameof(PersonResponse.Gender), SortOrderOptions.ASC) =>
                    allPersons.OrderBy(p => p.Gender, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Gender), SortOrderOptions.DESC) =>
                    allPersons.OrderByDescending(p => p.Gender, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.CountryName), SortOrderOptions.ASC) =>
                    allPersons.OrderBy(p => p.CountryName, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.CountryName), SortOrderOptions.DESC) =>
                    allPersons.OrderByDescending(p => p.CountryName, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Address), SortOrderOptions.ASC) =>
                    allPersons.OrderBy(p => p.Address, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Address), SortOrderOptions.DESC) =>
                    allPersons.OrderByDescending(p => p.Address, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC) =>
                    allPersons.OrderBy(p => p.ReceiveNewsLetters).ToList(),
                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC) =>
                    allPersons.OrderByDescending(p => p.ReceiveNewsLetters).ToList(),
                (_,_) => allPersons
            };

            return sortedPersons;
        }

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if(personUpdateRequest is null)
            {
                throw new ArgumentNullException(nameof(personUpdateRequest));
            }

            ValidationsHelper.ModelVaidation(personUpdateRequest);

            var matchingPerson = this._db.Persons is not null ? 
                await this._db.Persons.FirstOrDefaultAsync(p => p.PersonId == personUpdateRequest.PersonId) : null;
            if(matchingPerson is null)
            {
                throw new ArgumentException("This PersonId doesnt exists in datasource");
            }

            var personEntityToUpdate = personUpdateRequest.ToPerson();

            matchingPerson.PersonName = personEntityToUpdate.PersonName;
            matchingPerson.Email = personEntityToUpdate.Email;
            matchingPerson.Address = personEntityToUpdate.Address;
            matchingPerson.Gender = personEntityToUpdate.Gender;
            matchingPerson.CountryId = personEntityToUpdate.CountryId;
            matchingPerson.DateOfBirth = personEntityToUpdate.DateOfBirth;
            matchingPerson.ReceiveNewsLetters = personEntityToUpdate.ReceiveNewsLetters;

            await this._db.SaveChangesAsync();

            var personResponse = personEntityToUpdate.ToPersonResponse();

            return personResponse;
        }

        public async Task<bool> DeletePerson(Guid? personId)
        {
            if(personId is null)
            {
                throw new ArgumentNullException(nameof(personId));  
            }

            var matchingPerson = this._db.Persons is not null ?
                await this._db.Persons.FirstOrDefaultAsync(p => p.PersonId == personId) : null;
            if(matchingPerson is null)
            {
                return false;
            }

            var isSucess = this?._db?.Persons?.Remove(matchingPerson);
            await this._db.SaveChangesAsync();

            return true;
        }
    }
}
