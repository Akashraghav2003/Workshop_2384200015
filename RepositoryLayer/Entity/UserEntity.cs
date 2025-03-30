using RepositoryLayer.Entity;
using System.ComponentModel.DataAnnotations;

public class UserEntity
{
    [Key]
    public int UserId { get; set; }

    [Required]
    public string? UserName { get; set; }

    [Required, EmailAddress]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }

    [Required]
    public string Role { get; set; } = "User"; // Default role is "User"

    public virtual ICollection<AddressEntity>? AddressBookEntries { get; set; }
}
