using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlogHub.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The Name field is Required.")]
        [MaxLength(30, ErrorMessage = "maxlength is 30")]
        [DisplayName("Category Name")]
        public string? Name { get; set; }


    }
}
