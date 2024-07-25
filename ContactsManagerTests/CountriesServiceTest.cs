using Services;
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
        public void Test1()
        {

        }
    }
}