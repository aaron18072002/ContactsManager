using Entities;
using Services;
using ServicesContracts.DTOs;
using ServicesContracts.Enums;
using ServicesContracts.Interfaces;
using Xunit.Abstractions;

namespace ContactsManagerTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;
        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            this._personsService = new PersonsService();
            this._countriesService = new CountriesService();
            this._testOutputHelper = testOutputHelper;
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

            //Act
            var personResponse = this._personsService.AddPerson(personAddRequest);
            var people = this._personsService.GetAllPersons();

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
            var expectedValue = new List<PersonResponse>();
            var listOfPersonRequest = new List<PersonAddRequest>()
            {
                personAddRequest1,
                personAddRequest2,
                personAddRequest3
            };

            //Act
            this._testOutputHelper.WriteLine("Expected Value:");
            foreach (var personAddRequest in listOfPersonRequest)
            {
                expectedValue.Add(this._personsService.AddPerson(personAddRequest));
            }
            foreach (var personResponse in expectedValue)
            {
                this._testOutputHelper.WriteLine(personResponse.ToString());
            }

            this._testOutputHelper.WriteLine("Actual Value: ");
            var actualValue = this._personsService.GetAllPersons();
            foreach (var personResponse in actualValue)
            {
                this._testOutputHelper.WriteLine(personResponse.ToString());
            }

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

        #region GetFilteredPersons
        [Fact]
        public void GetFilteredPersons_EmptySearchString()
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
            var expectedValue = new List<PersonResponse>();
            var listOfPersonRequest = new List<PersonAddRequest>()
            {
                personAddRequest1,
                personAddRequest2,
                personAddRequest3
            };

            //Act
            this._testOutputHelper.WriteLine("Expected Value:");
            foreach (var personAddRequest in listOfPersonRequest)
            {
                expectedValue.Add(this._personsService.AddPerson(personAddRequest));
            }
            foreach (var personResponse in expectedValue)
            {
                this._testOutputHelper.WriteLine(personResponse.ToString());
            }

            this._testOutputHelper.WriteLine("Actual Value: ");
            var actualValue = this._personsService.GetFilteredPersons(nameof(Person.PersonName), "");
            foreach (var personResponse in actualValue)
            {
                this._testOutputHelper.WriteLine(personResponse.ToString());
            }

            //Assert
            foreach (var personResponse in expectedValue)
            {
                Assert.Contains(personResponse, actualValue);
            }
        }

        [Fact]
        public void GetFilteredPersons_SearchByPersonName()
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
            var expectedValue = new List<PersonResponse>();
            var listOfPersonRequest = new List<PersonAddRequest>()
            {
                personAddRequest1,
                personAddRequest2,
                personAddRequest3
            };

            //Act
            this._testOutputHelper.WriteLine("Expected Value:");
            foreach (var personAddRequest in listOfPersonRequest)
            {
                expectedValue.Add(this._personsService.AddPerson(personAddRequest));
            }
            foreach (var personResponse in expectedValue)
            {
                this._testOutputHelper.WriteLine(personResponse.ToString());
            }

            this._testOutputHelper.WriteLine("Actual Value: ");
            var actualValue = this._personsService.GetFilteredPersons(nameof(Person.PersonName), "ma");
            foreach (var personResponse in actualValue)
            {
                this._testOutputHelper.WriteLine(personResponse.ToString());
            }

            //Assert
            foreach (var personResponse in expectedValue)
            {
                if(personResponse.PersonName is not null)
                {
                    if(personResponse.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.Contains(personResponse, actualValue);
                    }
                }
            }
        }
        #endregion

        #region GetSortedPersons
        [Fact]
        public void GetSortedPersons_DescendingPersons()
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
            var allPersons = new List<PersonResponse>();
            var listOfPersonRequest = new List<PersonAddRequest>()
            {
                personAddRequest1,
                personAddRequest2,
                personAddRequest3
            };

            //Act
            foreach (var personAddRequest in listOfPersonRequest)
            {
                allPersons.Add(this._personsService.AddPerson(personAddRequest));
            }

            this._testOutputHelper.WriteLine("Actual Value: ");
            var actualValue = this._personsService.GetSortedPersons
                (allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);
            foreach (var personResponse in actualValue)
            {
                this._testOutputHelper.WriteLine(personResponse.ToString());
            }

            var expectedValue = allPersons.OrderByDescending(p => p.PersonName).ToList();
            this._testOutputHelper.WriteLine("ExpectedValue: ");
            foreach (var personResponse in expectedValue)
            {
                this._testOutputHelper.WriteLine(personResponse.ToString());
            }

            //Assert
            for (int i = 0; i < expectedValue.Count(); i++)
            {
                Assert.Equal(expectedValue[i], actualValue[i]);
            }
        }
        #endregion

        #region UpdatePerson

        [Fact]
        public void UpdatePerson_NullPersonUpdateRequest()
        {
            //Arrange
            PersonUpdateRequest? personUpdateRequest = null;

            //Act
            Action action = () =>
            {
                this._personsService.UpdatePerson(personUpdateRequest);
            };

            //Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void UpdatePerson_InvalidPersonId()
        {
            //Arrange
            var personUpdateRequest = new PersonUpdateRequest()
            {
                PersonId = Guid.NewGuid(),
            };

            //Act
            Action action = () =>
            {
                this._personsService.UpdatePerson(personUpdateRequest);
            };

            //Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void UpdatePerson_NullPersonName()
        {
            //Arrange
            var countryAddRequest = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            var countryResponse = this._countriesService.AddCountry(countryAddRequest);

            var personAddRequest = new PersonAddRequest()
            {
                PersonName = "John",
                CountryId = countryResponse.CountryId,
                Email = "abc@gmail.com",
                Gender = GenderOptions.Male
            };
            var personResponse = this._personsService.AddPerson(personAddRequest);
            var personUpdateRequest = personResponse.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = null;

            //Act
            Action action = () =>
            {
                this._personsService.UpdatePerson(personUpdateRequest);
            };

            //Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void UpdatePerson_ProperPersonUpdateRequest()
        {
            //Arrange
            var countryAddRequest = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            var countryResponse = this._countriesService.AddCountry(countryAddRequest);

            var personAddRequest = new PersonAddRequest()
            {
                PersonName = "John",
                CountryId = countryResponse.CountryId,
                Address = "Abc road",
                DateOfBirth = DateTime.Parse("2002-07-18"),
                Email = "abc@example.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            };
            var personResponse = this._personsService.AddPerson(personAddRequest);
            var personUpdateRequest = personResponse.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = "Nemo";
            personUpdateRequest.Email = "nemo@gmail.com";

            //Act
            var actualValue = this._personsService.UpdatePerson(personUpdateRequest);
            var expectedValue = this._personsService.GetPersonByPersonId(personUpdateRequest.PersonId);

            //Assert
            Assert.Equal(expectedValue, actualValue);
        }
        #endregion

        #region DeletePerson

        [Fact]
        public void DeletePerson_ValidPersonId()
        {
            //Arrange
            var countryAddRequest = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            var countryResponse = this._countriesService.AddCountry(countryAddRequest);

            var personAddRequest = new PersonAddRequest()
            {
                PersonName = "Jones",
                Address = "address",
                CountryId = countryResponse.CountryId,
                DateOfBirth = Convert.ToDateTime("2002-07-18"),
                Email = "jones@example.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            };
            var personResponse = this._personsService.AddPerson(personAddRequest);

            //Act
            var isSucess = this._personsService.DeletePerson(personResponse.PersonId); 

            //Assert
            Assert.True(isSucess);
        }

        [Fact]
        public void DeletePerson_InvalidPersonId()
        {
            //Arrange
            var invalidId = Guid.NewGuid();

            //Act
            var isValid = this._personsService.DeletePerson(invalidId);

            //Assert
            Assert.False(isValid);
        }

        #endregion
    }
}
