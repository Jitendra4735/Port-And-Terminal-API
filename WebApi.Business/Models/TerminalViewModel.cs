using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Business.Models
{
    public class TerminalViewModel : Terminal
    {
        public int Id { get; set; }

        [ForeignKey("PortId")]
        public PortViewModel Port { get; set; }

        public DateTime AddedDate { get; set; } //= DateTime.UtcNow;

        public DateTime? LastEditedDate { get; set; } //= DateTime.UtcNow;
    }
}
