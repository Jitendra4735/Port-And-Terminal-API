using WebApi.Business.Models;

namespace WebApi.Business.Business.Interface
{
    public interface IPortProcessor
    {
        Task<IEnumerable<PortViewModel>> GetPorts();
        Task<PortViewModel> GetPort(int id);
        Task<PortViewModel> CreatePort(Port port);
        Task UpdatePort(PortEditModel port);
        Task DeletePort(int id);
    }
}
