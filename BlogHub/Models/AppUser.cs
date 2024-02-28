using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BlogHub.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        [StringLength(100)]
        public string? Name { get; set; }
    }
}
