using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyWebRazor_Temp.Models
{
    public class Category
    {
        [Key]
        public int Id{ get; set; }
        [Required]
        [DisplayName("Category Name")]
        [MaxLength(31)]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1,100,ErrorMessage = "The Value Must Be In The Range 1-100")]
        public int DisplayOrder { get; set; }
    }
}
