using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    public class UserEntity
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required, EmailAddress]

        public string Email { get; set; }

        [Required]

        public string Password { get; set; }

        
        public virtual ICollection<AddressEntity> AddressBookEntries { get; set; }

    }
}
