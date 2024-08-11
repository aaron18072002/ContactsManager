using Entities;
using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;
using Services;
using ServicesContracts.DTOs;
using ServicesContracts.Interfaces;

namespace ContactsManagerTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;
        public CountriesServiceTest()
        {
            var dbContextMock = new DbContextMock<ContactsManagerDbContext>
                (new DbContextOptionsBuilder<ContactsManagerDbContext>().Options);
            var dbContext = dbContextMock.Object;

            var coutriesInitialize = new List<Country>();
            dbContextMock.CreateDbSetMock(t => t.Countries, coutriesInitialize);

            this._countriesService = new CountriesService(dbContext);
        }

        #region AddCountry
        [Fact]
        public async Task AddCountry_NullCountry()
        {
            //Arrange
            CountryAddRequest? request = null;

            //Act
            var action = async () =>
            {
                await this._countriesService.AddCountry(request);
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(action);
        }

        [Fact]
        public async Task AddCountry_CountryNameIsNull()
        {
            //Arrange
            var request = new CountryAddRequest()
            {
                CountryName = null
            };

            //Act
            var action = async () =>
            {
                await this._countriesService.AddCountry(request);
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(action);
        }

        [Fact]
        public async Task AddCountry_DuplicateCountryName()
        {
            //Arrange
            await this._countriesService.AddCountry(new CountryAddRequest()
            {
                CountryName = "Japan"
            });
            var request = new CountryAddRequest()
            {
                CountryName = "Japan"
            };

            //Act
            var action = async () =>
            {
                await this._countriesService.AddCountry(request);
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(action);
        }

        [Fact]
        public async Task AddCountry_ProperCountryDetails()
        {
            //Arrange
            var request = new CountryAddRequest()
            {
                CountryName = "Japan"
            };

            //Act
            var response = await this._countriesService.AddCountry(request);
            var allCountries = await this._countriesService.GetAllCountries();

            //Assert
            Assert.True(response.CountryId != Guid.Empty);
            Assert.Contains(response, allCountries);
        }
        #endregion

        #region GetAllCountries
        [Fact]
        public async Task GetAllCountries_EmpltyList()
        {
            //Arrange
            
            //Act
            var response = await this._countriesService.GetAllCountries();

            //Assert
            Assert.Empty(response);
        }

        [Fact]
        public async Task GetAllCountries_AddFewCountries()
        {
            //Arrange
            var fewCountries = new List<CountryAddRequest>()
            {
                new CountryAddRequest() { CountryName = "UK" },
                new CountryAddRequest() { CountryName = "USA" }
            };
            var expectedValue = new List<CountryResponse>();

            //Act
            foreach (var country in fewCountries)
            {
                expectedValue.Add(await this._countriesService.AddCountry(country));
            }
            var actualValue = await this._countriesService.GetAllCountries();

            //Assert
            foreach (var expectedCountry in expectedValue)
            {
                Assert.Contains(expectedCountry, actualValue);
            }
        }
        #endregion

        #region GetCountryByCountryId
        [Fact]
        public async Task GetCountryByCountryId_NullCountryId()
        {
            //Arrange
            Guid? countryId = null;

            //Act
            var response = await this._countriesService.GetCountryByCountryId(countryId);

            //Assert
            Assert.Null(response);
        }

        [Fact]
        public async Task GetCountryById_ValidCountryId()
        {
            //Arrange
            var countryAddRequest = new CountryAddRequest()
            {
                CountryName = "Japan"
            };
            var expectedValue = await this._countriesService.AddCountry(countryAddRequest);
            var countryId = expectedValue.CountryId;

            //Act
            var actualValue = await this._countriesService.GetCountryByCountryId(countryId);

            //Assert
            Assert.Equal(expectedValue, actualValue);
        }
        #endregion
    }
}