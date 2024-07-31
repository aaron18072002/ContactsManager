using Entities;
using Services.Helpers;
using ServicesContracts.DTOs;
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
        private List<Person> _people;
        private readonly ICountriesService _countriesService;
        public PersonsService()
        {
            this._people = new List<Person>();
            this._countriesService = new CountriesService();
        }
        private PersonResponse ConvertPersonToPersonResponse(Person person)
        {
            var personResponse = person.ToPersonResponse();
            personResponse.CountryName = this._countriesService.GetCountryByCountryId
                (personResponse.CountryId)?.CountryName;

            return personResponse;
        }

        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            if(personAddRequest is null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }

            ValidationsHelper.ModelVaidation(personAddRequest);

            var personEntity = personAddRequest.ToPerson();
            personEntity.PersonId = Guid.NewGuid();

            this._people.Add(personEntity);

            var personResponse = this.ConvertPersonToPersonResponse(personEntity);

            return personResponse;
        }

        public List<PersonResponse> GetAllPersons()
        {
            var response = this._people.Select(p => this.ConvertPersonToPersonResponse(p)).ToList();

            return response;
        }

        public PersonResponse? GetPersonByPersonId(Guid? personId)
        {
            if(personId == null)
            {
                return null;
            }

            var personEntity = this._people.FirstOrDefault(p => p.PersonId == personId);
            if(personEntity == null)
            {
                return null;
            }

            var personResponse = this.ConvertPersonToPersonResponse(personEntity);

            return personResponse;
        }

        public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
        {
            var allPersons = this.GetAllPersons();
            var matchingPersons = allPersons;

            if (searchBy is null || searchString is null)
            {
                return allPersons;
            }

            switch(searchBy)
            {
                case(nameof(Person.PersonName)):
                    matchingPersons = allPersons.Where
                        (p => !string.IsNullOrEmpty(p.PersonName) ? p.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case (nameof(Person.Email)):
                    matchingPersons = allPersons.Where
                        (p => !string.IsNullOrEmpty(p.Email) ? p.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case (nameof(Person.DateOfBirth)):
                    matchingPersons = allPersons.Where
                        (p => p.DateOfBirth is not null ? p.DateOfBirth.Value.ToString("dd MMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case (nameof(Person.Gender)):
                    matchingPersons = allPersons.Where
                        (p => !string.IsNullOrEmpty(p.Gender) ? p.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case (nameof(Person.CountryId)):
                    matchingPersons = allPersons.Where
                        (p => !string.IsNullOrEmpty(p.CountryName) ? p.CountryName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case (nameof(Person.Address)):
                    matchingPersons = allPersons.Where
                        (p => !string.IsNullOrEmpty(p.Address) ? p.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                default:
                    matchingPersons = allPersons;
                    break;
            }

            return matchingPersons;
        }
    }
}
