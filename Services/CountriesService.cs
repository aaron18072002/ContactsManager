using Entities;
using ServicesContracts.DTOs;
using ServicesContracts.Interfaces;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private List<Country> _countries;
        public CountriesService(bool initialize = true)
        {
            this._countries = new List<Country>();
            if(initialize)
            {
                this._countries.AddRange(new List<Country>()
                {
                    new Country()
                    {
                        CountryId = Guid.Parse("000C76EB-62E9-4465-96D1-2C41FDB64C3B"), 
                        CountryName = "USA"
                    },
                    new Country() 
                    { 
                        CountryId = Guid.Parse("32DA506B-3EBA-48A4-BD86-5F93A2E19E3F"), 
                        CountryName = "Canada" 
                    },
                    new Country() 
                    { 
                        CountryId = Guid.Parse("DF7C89CE-3341-4246-84AE-E01AB7BA476E"), 
                        CountryName = "UK" 
                    },
                    new Country() 
                    { 
                        CountryId = Guid.Parse("15889048-AF93-412C-B8F3-22103E943A6D"), 
                        CountryName = "India" 
                    },
                    new Country() 
                    { 
                        CountryId= Guid.Parse("80DF255C-EFE7-49E5-A7F9-C35D7C701CAB"), 
                        CountryName = "Australia" }
                });
            }
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

            if(this._countries.Where(c => c.CountryName == country.CountryName).Count() > 0)
            {
                throw new ArgumentException("This CountryName already exists");
            }

            var countryEntity = country.ToCountry();
            countryEntity.CountryId = Guid.NewGuid();

            this._countries.Add(countryEntity);

            return countryEntity.ToCountryResponse();
        }
        public List<CountryResponse> GetAllCountries()
        {
            var result = this._countries.Select(c => c.ToCountryResponse()).ToList();

            return result;
        }
        public CountryResponse? GetCountryByCountryId(Guid? id)
        {
            if(id ==  null)
            {
                return null;
            }

            var countryEntity = this._countries.FirstOrDefault
                (c => c.CountryId == id);

            return countryEntity == null ? null : countryEntity.ToCountryResponse();
        }
    }
}
