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
    public class PersonAddRequest
    {
        [Required(ErrorMessage = "PersonName cant be null or empty")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Email cant be null or empty")]
        [EmailAddress(ErrorMessage = "Email value must be a valid email")]
        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public GenderOptions? Gender { get; set; }

        public Guid? CountryId { get; set; }

        public string? Address { get; set; }

        public bool ReceiveNewsLetters { get; set; }

        public Person ToPerson()
        {
            return new Person()
            {
                PersonName = this.PersonName,
                Email = this.Email,
                DateOfBirth = this.DateOfBirth,
                Gender = this.Gender.ToString(),
                Address = this.Address,
                ReceiveNewsLetters = this.ReceiveNewsLetters,
            };
        }
    }
}
