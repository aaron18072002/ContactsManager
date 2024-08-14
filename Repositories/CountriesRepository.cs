using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoriesContracts;

namespace Repositories
{
    public class CountriesRepository : ICountriesRepository
    {
        private readonly ContactsManagerDbContext _db;
        public CountriesRepository(ContactsManagerDbContext db)
        {
            this._db = db;
        }

        public async Task<Country> AddCountry(Country country)
        {
            if(this._db.Countries is not null)
            {
                this._db.Countries.Add(country);
                await this._db.SaveChangesAsync();
            }
            return country;
        }

        public async Task<List<Country>> GetAllCountries()
        {
            var result = this._db.Countries is not null ? 
                await this._db.Countries.ToListAsync() : new List<Country>();

            return result;
        }

        public async Task<Country?> GetCountryById(Guid countryId)
        {
            var result = this._db.Countries is not null ? 
                await this._db.Countries.FirstOrDefaultAsync(c => c.CountryId == countryId) : null;

            return result;
        }

        public async Task<Country?> GetCountryByName(string countryName)
        {
            var result = this._db.Countries is not null ?
                await this._db.Countries.FirstOrDefaultAsync(c => c.CountryName == countryName) : null;

            return result;
        }
    }
}
