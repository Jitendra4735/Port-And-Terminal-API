using System.ComponentModel.DataAnnotations;

namespace WebApi.Business.Models
{
    public class Port
    {
        [Key]
        [Required]
        [StringLength(10, MinimumLength = 5)]
        public string Code { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Name { get; set; }
    }
}
