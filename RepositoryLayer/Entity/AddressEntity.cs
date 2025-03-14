using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    public class AddressEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, Phone]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        // Foreign Key Attribute
        [ForeignKey("User")]
        public int UserId { get; set; }

        // Navigation Property
        public virtual UserEntity User { get; set; }
    }
}
