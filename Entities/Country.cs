using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Country
    {
        /// <summary>
        /// Domain model for Country
        /// </summary>
 
        [Key]             
        public Guid CountryId { get; set; }

        [StringLength(40)]
        public string? CountryName { get; set; }

        public virtual ICollection<Person>? Persons { get; set; }
    }
}
