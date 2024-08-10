using Microsoft.AspNetCore.Http;
using ServicesContracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesContracts.Interfaces
{
    /// <summary>
    /// Represents business logics for manipulating with Country entity
    /// </summary>
    public interface ICountriesService
    {
        /// <summary>
        /// Add a country object to list of countries
        /// </summary>
        /// <param name="country">Country object to add</param>
        /// <returns>Returns the country object after adding it (including newly generated country id)</returns>
        Task<CountryResponse> AddCountry(CountryAddRequest? country);

        /// <summary>
        /// Return all countries from datasource
        /// </summary>
        /// <returns>Return a list of CountryResponse</returns>
        Task<List<CountryResponse>> GetAllCountries();
        
        /// <summary>
        /// Return a country object from datasource
        /// </summary>
        /// <param name="id">CountryId to search</param>
        /// <returns>Returns a CountryResponse or null</returns>
        Task<CountryResponse?> GetCountryByCountryId(Guid? id);

        Task<int> UploadCountriesFromExcelFile(IFormFile formFile);
    }
}
