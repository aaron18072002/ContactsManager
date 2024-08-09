using Entities;
using Microsoft.EntityFrameworkCore;
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
        public async Task<CountryResponse> AddCountry(CountryAddRequest? country)
        {
            if(country is null)
            {
                throw new ArgumentNullException(nameof(country));
            }

            if(country.CountryName is null)
            {
                throw new ArgumentException(nameof(country.CountryName));
            }

            if(this._db.Countries is not null && 
                await this._db.Countries.CountAsync(c => c.CountryName == country.CountryName) > 0)
            {
                throw new ArgumentException("This CountryName already exists");
            }

            var countryEntity = country.ToCountry();
            countryEntity.CountryId = Guid.NewGuid();

            this?._db?.Countries?.Add(countryEntity);
            await this._db.SaveChangesAsync();

            return countryEntity.ToCountryResponse();
        }
        public async Task<List<CountryResponse>> GetAllCountries()
        {
            var persons = this._db.Countries is not null ? await this._db.Countries.ToListAsync() : new List<Country>();

            var result = persons.Select(c => c.ToCountryResponse()).ToList();

            return result ?? new List<CountryResponse>();
        }
        public async Task<CountryResponse?> GetCountryByCountryId(Guid? id)
        {
            if(id ==  null)
            {
                return null;
            }

            var countryEntity = this._db.Countries is not null ?  await this._db.Countries.FirstOrDefaultAsync
                (c => c.CountryId == id) : new Country();

            return countryEntity == null ? null : countryEntity.ToCountryResponse();
        }
    }
}
