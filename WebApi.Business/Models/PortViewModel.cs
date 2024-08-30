namespace WebApi.Business.Models
{
    public class PortViewModel : Port
    {
        public int Id { get; set; }
        public DateTime AddedDate { get; set; } //= DateTime.UtcNow;
        public DateTime? LastEditedDate { get; set; }
        public ICollection<TerminalViewModel> Terminals { get; set; } = new List<TerminalViewModel>();
    }
}
