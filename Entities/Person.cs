using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Person
    {
        [Key]
        public Guid? PersonId { get; set; }

        [StringLength(40)]
        [Column("PersonName")]
        public string? PersonName { get; set; }

        [StringLength(40)]
        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; }

        //uniqueindentifier
        public Guid? CountryId { get; set; }

        [StringLength(100)]
        public string? Address { get; set; }

        //bit
        public bool ReceiveNewsLetters { get; set; }

        [Column(TypeName = "varchar(8)")] //TextIdentificationNumber
        public string? TIN { get; set; }

        [ForeignKey(nameof(CountryId))]
        public virtual Country? Country { get; set; }   
    }
}
