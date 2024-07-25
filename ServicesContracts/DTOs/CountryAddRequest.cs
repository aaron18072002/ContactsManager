using Entities;

namespace ServicesContracts.DTOs
{
    public class CountryAddRequest
    {
        /// <summary>
        /// DTO class for add a new country
        /// </summary>
        public string? CountryName { get; set; }
        public Country ToCountry()
        {
            return new Country()
            {
                CountryName = this.CountryName
            };
        }
    }
}
