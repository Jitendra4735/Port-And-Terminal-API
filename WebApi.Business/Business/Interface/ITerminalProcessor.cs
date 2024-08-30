using WebApi.Business.Models;

namespace WebApi.Business.Business.Interface
{
    public interface ITerminalProcessor
    {
        Task<IEnumerable<TerminalViewModel>> GetTerminals();
        Task<TerminalViewModel> GetTerminal(int id);
        Task<TerminalViewModel> CreateTerminal(Terminal terminal);
        Task UpdateTerminal(TerminalEditModel terminal);
        Task DeleteTerminal(int id);
    }
}
