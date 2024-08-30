using Microsoft.EntityFrameworkCore;
using WebApi.Data.Models;
using WebApi.Infrastructure.Models;

namespace WebApi.Infrastructure.Repository.Interface
{
    public interface IApplicationDbContext
    {
        DbSet<UserInfo> UserInfo { get; set; }
        DbSet<PortDto> Ports { get; set; }
        DbSet<TerminalDto> Terminals { get; set; }
        Task<int> SaveChangesAsync();
        void SeedDataAsync();
    }
}
