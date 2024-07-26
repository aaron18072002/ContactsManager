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

            //Assert
            Assert.True(response.CountryId != Guid.Empty);
        }
    }
}