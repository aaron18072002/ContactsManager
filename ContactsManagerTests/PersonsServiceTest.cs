using Services;
using ServicesContracts.DTOs;
using ServicesContracts.Enums;
using ServicesContracts.Interfaces;

namespace ContactsManagerTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;
        public PersonsServiceTest()
        {
            this._personsService = new PersonsService();
            this._countriesService = new CountriesService();
        }
        #region AddPerson
        [Fact]
        public void AddPerson_NullPersonAddRequest()
        {
            //Arrange
            PersonAddRequest? personAddRequest = null;

            //Act
            var action = () =>
            {
                this._personsService.AddPerson(personAddRequest);
            };

            //Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void AddPerson_PersonNameIsNull()
        {
            //Arrange
            var personAddRequest = new PersonAddRequest()
            {
                PersonName = null
            };

            //Act
            var action = () =>
            {
                this._personsService.AddPerson(personAddRequest);
            };

            //Assert
            Assert.Throws<ArgumentException>(action);   
        }

        [Fact]        
        public void AddPerson_ProperPersonDetails()
        {
            //Arrange
            var countryAddRequest = new CountryAddRequest()
            {
                CountryName = "Japan"
            };
            var countryResponse = this._countriesService.AddCountry(countryAddRequest);
            var personAddRequest = new PersonAddRequest()
            {
                PersonName = "Person name...",
                Email = "person@example.com",
                Address = "sample address",
                CountryId = countryResponse.CountryId,
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2002-07-18"),
                ReceiveNewsLetters = true
            };
            var people = this._personsService.GetAllPersons();

            //Act
            var personResponse = this._personsService.AddPerson(personAddRequest);

            //Assert
            Assert.True(personResponse.PersonId != Guid.Empty);
            Assert.Contains(personResponse, people);
        }
        #endregion

        #region GetAllPersons
        [Fact]
        public void GetAllPersons_EmptyList()
        {
            //Arrange

            //Act
            var listOfPersons = this._personsService.GetAllPersons();

            //Assert
            Assert.Empty(listOfPersons);
        }

        [Fact]
        public void GetAllPersons_AddFewPersons()
        {
            //Arrange
            var countryAddRequest1 = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            var countryAddRequest2 = new CountryAddRequest()
            {
                CountryName = "India"
            };
            var countryResponse1 = this._countriesService.AddCountry(countryAddRequest1);
            var countryResponse2 = this._countriesService.AddCountry(countryAddRequest2);

            var personAddRequest1 = new PersonAddRequest()
            {
                PersonName = "Smith",
                Email = "smith@example.com",
                Gender = GenderOptions.Male,
                Address = "address of smith",
                CountryId = countryResponse1.CountryId,
                DateOfBirth = DateTime.Parse("2002-05-06"),
                ReceiveNewsLetters = true
            };
            var personAddRequest2 = new PersonAddRequest()
            {
                PersonName = "Mary",
                Email = "mary@example.com",
                Gender = GenderOptions.Female,
                Address = "address of mary",
                CountryId = countryResponse2.CountryId,
                DateOfBirth = DateTime.Parse("2000-02-02"),
                ReceiveNewsLetters = false
            };
            var personAddRequest3 = new PersonAddRequest()
            {
                PersonName = "Rahman",
                Email = "rahman@example.com",
                Gender = GenderOptions.Male,
                Address = "address of rahman",
                CountryId = countryResponse1.CountryId,
                DateOfBirth = DateTime.Parse("1999-03-03"),
                ReceiveNewsLetters = true
            };
            var actualValue = new List<PersonResponse>();
            var listOfPersonRequest = new List<PersonAddRequest>()
            {
                personAddRequest1,
                personAddRequest2,
                personAddRequest3
            };

            //Act
            foreach (var personAddRequest in listOfPersonRequest)
            {
                actualValue.Add(this._personsService.AddPerson(personAddRequest));
            }
            var expectedValue = this._personsService.GetAllPersons();

            //Assert
            foreach (var personResponse in expectedValue)
            {
                Assert.Contains(personResponse, actualValue);
            }
        }
        #endregion

        #region GetPersonByPersonId
        [Fact]
        public void GetPersonByPersonId_NullPersonId()
        {
            //Arrange
            Guid? personId = null;

            //Act
            var personResponse = this._personsService.GetPersonByPersonId(personId);

            //Assert
            Assert.Null(personResponse);
        }

        [Fact]
        public void GetPersonById_ProperPersonId()
        {
            //Arrange
            var countryAddRequest = new CountryAddRequest()
            {
                CountryName = "Japan"
            };
            var countryResponse = this._countriesService.AddCountry(countryAddRequest);

            var personAddRequest = new PersonAddRequest()
            {
                PersonName = "person name...",
                Email = "email@sample.com",
                Address = "address",
                CountryId = countryResponse.CountryId,
                DateOfBirth = DateTime.Parse("2002-07-18"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = false
            };
            var actualValue = this._personsService.AddPerson(personAddRequest);

            //Act
            var expectedValue = this._personsService.GetPersonByPersonId(actualValue.PersonId);

            //Assert
            Assert.Equal(expectedValue, actualValue);
        }
        #endregion
    }
}
