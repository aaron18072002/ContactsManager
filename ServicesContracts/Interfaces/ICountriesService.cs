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
        CountryResponse AddCountry(CountryAddRequest country);
    }
}
