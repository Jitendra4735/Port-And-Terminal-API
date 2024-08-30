using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Data.Models
{
    public class PortDto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 5)]
        public string Code { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Name { get; set; }

        public DateTime AddedDate { get; set; } = DateTime.UtcNow;

        public DateTime? LastEditedDate { get; set; } = DateTime.UtcNow;

        public virtual ICollection<TerminalDto> Terminals { get; set; } = new List<TerminalDto>();
    }

}
