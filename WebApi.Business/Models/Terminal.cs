using System.ComponentModel.DataAnnotations;

namespace WebApi.Business.Models
{
    public class Terminal
    {
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Name { get; set; }

        [Required]
        public int PortId { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
