using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using RepositoriesContracts;
using ServicesContracts.DTOs;
using ServicesContracts.Interfaces;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly ICountriesRepository _countriesRepository;
        public CountriesService(ICountriesRepository countriesRepository)
        {
            this._countriesRepository = countriesRepository; 
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

            if(await this._countriesRepository.GetCountryByName(country.CountryName) is not null)
            {
                throw new ArgumentException("This CountryName already exists");
            }

            var countryEntity = country.ToCountry();
            countryEntity.CountryId = Guid.NewGuid();

            await this._countriesRepository.AddCountry(countryEntity);

            return countryEntity.ToCountryResponse();
        }
        public async Task<List<CountryResponse>> GetAllCountries()
        {
            var persons = await this._countriesRepository.GetAllCountries();

            var result = persons.Select(c => c.ToCountryResponse()).ToList();

            return result ?? new List<CountryResponse>();
        }
        public async Task<CountryResponse?> GetCountryByCountryId(Guid? id)
        {
            if(id ==  null)
            {
                return null;
            }

            var countryEntity = await this._countriesRepository.GetCountryById(id.Value);

            return countryEntity is null ? null : countryEntity.ToCountryResponse();
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

                        if (await this._countriesRepository.GetCountryByName(countryName) is null)
                        {
                            Country country = new Country() { CountryName = countryName };
                            await this._countriesRepository.AddCountry(country);

                            countriesInserted++;
                        }
                    }
                }
            }
            return countriesInserted;
        }
    }
}
