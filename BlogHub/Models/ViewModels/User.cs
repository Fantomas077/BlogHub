using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogHub.Models.ViewModels
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Email { get; set; }

        public string? ProfileImageName { get; set; }
        [NotMapped]
        public IFormFile? IamgeFile { get; set; }
    }
}
