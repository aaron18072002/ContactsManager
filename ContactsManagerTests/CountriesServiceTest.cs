using Services;
using ServicesContracts.DTOs;
using ServicesContracts.Interfaces;

namespace ContactsManagerTests
{
    public class CountriesServiceTest
    {
        private ICountriesService _countriesService;
        public CountriesServiceTest()
        {
            this._countriesService = new CountriesService();
        }

        #region AddCountry
        [Fact]
        public void AddCountry_NullCountry()
        {
            //Arrange
            CountryAddRequest? request = null;

            //Act
            var action = () =>
            {
                this._countriesService.AddCountry(request);
            };

            //Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void AddCountry_CountryNameIsNull()
        {
            //Arrange
            var request = new CountryAddRequest()
            {
                CountryName = null
            };

            //Act
            var action = () =>
            {
                this._countriesService.AddCountry(request);
            };

            //Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void AddCountry_DuplicateCountryName()
        {
            //Arrange
            this._countriesService.AddCountry(new CountryAddRequest()
            {
                CountryName = "Japan"
            });
            var request = new CountryAddRequest()
            {
                CountryName = "Japan"
            };

            //Act
            var action = () =>
            {
                this._countriesService.AddCountry(request);
            };

            //Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void AddCountry_ProperCountryDetails()
        {
            //Arrange
            var request = new CountryAddRequest()
            {
                CountryName = "Japan"
            };

            //Act
            var response = this._countriesService.AddCountry(request);
            var allCountries = this._countriesService.GetAllCountries();

            //Assert
            Assert.True(response.CountryId != Guid.Empty);
            Assert.Contains(response, allCountries);
        }
        #endregion

        #region GetAllCountries
        [Fact]
        public void GetAllCountries_EmpltyList()
        {
            //Arrange
            
            //Act
            var response = this._countriesService.GetAllCountries();

            //Assert
            Assert.Empty(response);
        }

        [Fact]
        public void GetAllCountries_AddFewCountries()
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
                expectedValue.Add(this._countriesService.AddCountry(country));
            }
            var actualValue = this._countriesService.GetAllCountries();

            //Assert
            foreach (var expectedCountry in expectedValue)
            {
                Assert.Contains(expectedCountry, actualValue);
            }
        }
        #endregion
    }
}