using Entities;
using ServicesContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesContracts.DTOs
{
    public class PersonResponse
    {
        public Guid? PersonId { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public string? CountryName { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        public int? Age { get; set; }
        public override bool Equals(object? obj)
        {
            if(obj is null)
            {
                return false;
            }
            if(obj.GetType() != typeof(PersonResponse))
            {
                return false;
            }
            PersonResponse objToCompare = (PersonResponse)obj;
            return objToCompare.PersonId == this.PersonId &&
                   objToCompare.PersonName == this.PersonName &&
                   objToCompare.Email == this.Email &&
                   objToCompare.DateOfBirth == this.DateOfBirth &&
                   objToCompare.Gender == this.Gender &&
                   objToCompare.CountryId == this.CountryId &&
                   objToCompare.Address == this.Address &&
                   objToCompare.ReceiveNewsLetters == this.ReceiveNewsLetters;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return $"PersonId: {this.PersonId}, PersonName: {this.PersonName}, " +
                   $"Email: {this.Email}, DateOfBirth: {this.DateOfBirth?.ToString("dd MM yyyy")}, " +
                   $"Gender: {this.Gender}, CountryId: {this.CountryId}, " +
                   $"Address: {this.Address}, ReceiveNewsLetters: {this.ReceiveNewsLetters}";
        }
        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest()
            {
                PersonId = this.PersonId,
                PersonName = this.PersonName,
                Email = this.Email,
                DateOfBirth = this.DateOfBirth,
                Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions), this.Gender, true),
                Address = this.Address,
                CountryId = this.CountryId,
                ReceiveNewsLetters = this.ReceiveNewsLetters
            };
        }
    }
    public static class PersonExtensions
    {
        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse()
            {
                PersonId = person.PersonId,
                PersonName = person.PersonName,
                Email = person.Email,
                DateOfBirth = person.DateOfBirth,
                Gender = person.Gender,
                CountryId = person.CountryId,
                Address = person.Address,
                ReceiveNewsLetters = person.ReceiveNewsLetters,
                Age = person.DateOfBirth is not null ? 
                    (int)Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null
            };
        }
    }
}
