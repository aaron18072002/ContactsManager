using Entities;
using ServicesContracts.DTOs;
using ServicesContracts.Interfaces;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly ContactsManagerDbContext _db;
        public CountriesService(ContactsManagerDbContext contactsManagerDbContext)
        {
            this._db = contactsManagerDbContext; 
        }
        public CountryResponse AddCountry(CountryAddRequest? country)
        {
            if(country is null)
            {
                throw new ArgumentNullException(nameof(country));
            }

            if(country.CountryName is null)
            {
                throw new ArgumentException(nameof(country.CountryName));
            }

            if(this?._db.Countries?.Count(c => c.CountryName == country.CountryName) > 0)
            {
                throw new ArgumentException("This CountryName already exists");
            }

            var countryEntity = country.ToCountry();
            countryEntity.CountryId = Guid.NewGuid();

            this?._db?.Countries?.Add(countryEntity);
            this._db.SaveChanges();

            return countryEntity.ToCountryResponse();
        }
        public List<CountryResponse> GetAllCountries()
        {
            var result = this?._db?.Countries?.ToList()
                .Select(c => c.ToCountryResponse()).ToList();

            return result ?? new List<CountryResponse>();
        }
        public CountryResponse? GetCountryByCountryId(Guid? id)
        {
            if(id ==  null)
            {
                return null;
            }

            var countryEntity = this?._db?.Countries?.FirstOrDefault
                (c => c.CountryId == id);

            return countryEntity == null ? null : countryEntity.ToCountryResponse();
        }
    }
}
