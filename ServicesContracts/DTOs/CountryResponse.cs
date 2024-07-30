using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesContracts.DTOs
{
    /// <summary>
    /// DTO class used as a return type for method that needs to return country/countries
    /// </summary>
    public class CountryResponse
    {
        public Guid CountryId { get; set; }
        public string? CountryName { get; set; }
        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }
            if(!typeof(CountryResponse).Equals(obj.GetType()))
            {
                return false;
            }
            var objToCompare = obj as CountryResponse;

            return this.CountryId == objToCompare?.CountryId && 
                   this.CountryName == objToCompare?.CountryName;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
    public static class CountryExtensions
    {
        public static CountryResponse ToCountryResponse
            (this Country country)
        {
            return new CountryResponse()
            {
                CountryId = country.CountryId,
                CountryName = country.CountryName
            };
        }
    }
}
