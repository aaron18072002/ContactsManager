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
            throw new NotImplementedException();
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
    }
}
