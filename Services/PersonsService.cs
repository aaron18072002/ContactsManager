using Entities;
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
        private List<Person> _people;
        private readonly ICountriesService _countriesService;
        public PersonsService(bool initialize = true)
        {
            this._people = new List<Person>();
            this._countriesService = new CountriesService();

            if(initialize)
            {
                this._people.Add(new Person()
                {
                    PersonId = Guid.Parse("8082ED0C-396D-4162-AD1D-29A13F929824"),
                    PersonName = "Aguste",
                    Email = "aleddy0@booking.com",
                    DateOfBirth = DateTime.Parse("1993-01-02"),
                    Gender = "Male",
                    Address = "0858 Novick Terrace",
                    ReceiveNewsLetters = false,
                    CountryId = Guid.Parse("000C76EB-62E9-4465-96D1-2C41FDB64C3B")
                });
                this._people.Add(new Person()
                {
                    PersonId = Guid.Parse("06D15BAD-52F4-498E-B478-ACAD847ABFAA"),
                    PersonName = "Jasmina",
                    Email = "jsyddie1@miibeian.gov.cn",
                    DateOfBirth = DateTime.Parse("1991-06-24"),
                    Gender = "Female",
                    Address = "0742 Fieldstone Lane",
                    ReceiveNewsLetters = true,
                    CountryId = Guid.Parse("32DA506B-3EBA-48A4-BD86-5F93A2E19E3F")
                });
                this._people.Add(new Person()
                {
                    PersonId = Guid.Parse("D3EA677A-0F5B-41EA-8FEF-EA2FC41900FD"),
                    PersonName = "Kendall",
                    Email = "khaquard2@arstechnica.com",
                    DateOfBirth = DateTime.Parse("1993-08-13"),
                    Gender = "Male",
                    Address = "7050 Pawling Alley",
                    ReceiveNewsLetters = false,
                    CountryId = Guid.Parse("32DA506B-3EBA-48A4-BD86-5F93A2E19E3F")
                });
                this._people.Add(new Person()
                {
                    PersonId = Guid.Parse("89452EDB-BF8C-4283-9BA4-8259FD4A7A76"),
                    PersonName = "Kilian",
                    Email = "kaizikowitz3@joomla.org",
                    DateOfBirth = DateTime.Parse("1991-06-17"),
                    Gender = "Male",
                    Address = "233 Buhler Junction",
                    ReceiveNewsLetters = true,
                    CountryId = Guid.Parse("DF7C89CE-3341-4246-84AE-E01AB7BA476E")
                });
                this._people.Add(new Person()
                {
                    PersonId = Guid.Parse("F5BD5979-1DC1-432C-B1F1-DB5BCCB0E56D"),
                    PersonName = "Dulcinea",
                    Email = "dbus4@pbs.org",
                    DateOfBirth = DateTime.Parse("1996-09-02"),
                    Gender = "Female",
                    Address = "56 Sundown Point",
                    ReceiveNewsLetters = false,
                    CountryId = Guid.Parse("DF7C89CE-3341-4246-84AE-E01AB7BA476E")
                });
                this._people.Add(new Person()
                {
                    PersonId = Guid.Parse("A795E22D-FAED-42F0-B134-F3B89B8683E5"),
                    PersonName = "Corabelle",
                    Email = "cadams5@t-online.de",
                    DateOfBirth = DateTime.Parse("1993-10-23"),
                    Gender = "Female",
                    Address = "4489 Hazelcrest Place",
                    ReceiveNewsLetters = false,
                    CountryId = Guid.Parse("15889048-AF93-412C-B8F3-22103E943A6D")
                });
                this._people.Add(new Person()
                {
                    PersonId = Guid.Parse("3C12D8E8-3C1C-4F57-B6A4-C8CAAC893D7A"),
                    PersonName = "Faydra",
                    Email = "fbischof6@boston.com",
                    DateOfBirth = DateTime.Parse("1996-02-14"),
                    Gender = "Female",
                    Address = "2010 Farragut Pass",
                    ReceiveNewsLetters = true,
                    CountryId = Guid.Parse("80DF255C-EFE7-49E5-A7F9-C35D7C701CAB")
                });
                this._people.Add(new Person()
                {
                    PersonId = Guid.Parse("7B75097B-BFF2-459F-8EA8-63742BBD7AFB"),
                    PersonName = "Oby",
                    Email = "oclutheram7@foxnews.com",
                    DateOfBirth = DateTime.Parse("1992-05-31"),
                    Gender = "Male",
                    Address = "2 Fallview Plaza",
                    ReceiveNewsLetters = false,
                    CountryId = Guid.Parse("80DF255C-EFE7-49E5-A7F9-C35D7C701CAB")
                });
                this._people.Add(new Person()
                {
                    PersonId = Guid.Parse("6717C42D-16EC-4F15-80D8-4C7413E250CB"),
                    PersonName = "Seumas",
                    Email = "ssimonitto8@biglobe.ne.jp",
                    DateOfBirth = DateTime.Parse("1999-02-02"),
                    Gender = "Male",
                    Address = "76779 Norway Maple Crossing",
                    ReceiveNewsLetters = false,
                    CountryId = Guid.Parse("80DF255C-EFE7-49E5-A7F9-C35D7C701CAB")
                });
                this._people.Add(new Person()
                {
                    PersonId = Guid.Parse("6E789C86-C8A6-4F18-821C-2ABDB2E95982"),
                    PersonName = "Freemon",
                    Email = "faugustin9@vimeo.com",
                    DateOfBirth = DateTime.Parse("1996-04-27"),
                    Gender = "Male",
                    Address = "8754 Becker Street",
                    ReceiveNewsLetters = false,
                    CountryId = Guid.Parse("80DF255C-EFE7-49E5-A7F9-C35D7C701CAB")
                });
            }
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
            if (personAddRequest is null)
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
            if (personId == null)
            {
                return null;
            }

            var personEntity = this._people.FirstOrDefault(p => p.PersonId == personId);
            if (personEntity == null)
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
            (List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrderOptions)
        {
            if (sortBy is null)
            {
                return allPersons;
            }

            var sortedPersons = (sortBy, sortOrderOptions) switch
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

        public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if(personUpdateRequest is null)
            {
                throw new ArgumentNullException(nameof(personUpdateRequest));
            }

            ValidationsHelper.ModelVaidation(personUpdateRequest);

            var matchingPerson = this._people.FirstOrDefault(p => p.PersonId == personUpdateRequest.PersonId);
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

            var personResponse = matchingPerson.ToPersonResponse();

            return personResponse;
        }

        public bool DeletePerson(Guid? personId)
        {
            if(personId is null)
            {
                throw new ArgumentNullException(nameof(personId));  
            }

            var matchingPerson = this._people.FirstOrDefault(p => p.PersonId == personId);
            if(matchingPerson is null)
            {
                return false;
            }

            var isSucess = this._people.Remove(matchingPerson);

            return isSucess;
        }
    }
}
