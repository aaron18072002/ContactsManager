using Entities;
using ServicesContracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesContracts.DTOs
{
    public class PersonUpdateRequest
    {
        [Required(ErrorMessage = "PersonId cant be null or empty")]
        public Guid? PersonId { get; set; }

        [Required(ErrorMessage = "PersonName cant be null or empty")]
        public string? PersonName { get; set; }

        [EmailAddress(ErrorMessage = "Email value should be a valid email")]
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderOptions? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        public Person ToPerson()
        {
            return new Person 
            { 
                PersonId = this.PersonId, 
                PersonName = this.PersonName, 
                Email = this.Email, 
                DateOfBirth = this.DateOfBirth, 
                Gender = this.Gender.ToString(),
                CountryId = this.CountryId,
                Address = this.Address,
                ReceiveNewsLetters = this.ReceiveNewsLetters 
            };
        }
    }
}
