using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
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

        public async Task<int> UploadCountriesFromExcelFile(IFormFile formFile)
        {
            var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            int countriesInserted = 0;

            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets["Countries"];

                int rowCount = workSheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    string? cellValue = Convert.ToString(workSheet.Cells[row, 1].Value);

                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        string? countryName = cellValue;

                        if (this._db.Countries is not null && 
                            this._db.Countries.Where(temp => temp.CountryName == countryName).Count() == 0)
                        {
                            Country country = new Country() { CountryName = countryName };
                            this._db.Countries.Add(country);
                            await _db.SaveChangesAsync();

                            countriesInserted++;
                        }
                    }
                }
            }
            return countriesInserted;
        }
    }
}
