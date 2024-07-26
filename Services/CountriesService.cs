﻿using Entities;
using ServicesContracts.DTOs;
using ServicesContracts.Interfaces;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private List<Country> _countries;
        public CountriesService()
        {
            this._countries = new List<Country>();
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
    }
}
