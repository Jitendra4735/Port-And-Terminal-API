using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Data.Models
{
    public class TerminalDto
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Name { get; set; }

        [Required]
        public int PortId { get; set; }

        [ForeignKey("PortId")]
        public virtual PortDto Port { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime AddedDate { get; set; } = DateTime.UtcNow;

        public DateTime? LastEditedDate { get; set; } = DateTime.UtcNow;
    }

}
