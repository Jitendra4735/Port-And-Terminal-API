using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApi.Data.Models;
using WebApi.Infrastructure.Models;
using WebApi.Infrastructure.Repository.Interface;

namespace WebApi.Infrastructure.Repository
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly IPasswordHasher<UserInfo> _passwordHasher;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IPasswordHasher<UserInfo> passwordHasher) : base(options)
        {
            _passwordHasher = passwordHasher;
        }

        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<PortDto> Ports { get; set; }
        public DbSet<TerminalDto> Terminals { get; set; }

        public async Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync().Result;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PortDto>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<PortDto>()
                .HasIndex(p => p.Code)
                .IsUnique();


            modelBuilder.Entity<TerminalDto>()
                .HasIndex(t => new { t.Name, t.PortId })
                .IsUnique();

            modelBuilder.Entity<TerminalDto>()
            .HasOne(t => t.Port)            // Each Terminal has one Port
            .WithMany(p => p.Terminals)     // Each Port has many Terminals
            .HasForeignKey(t => t.PortId);  // Foreign key on Terminal entity


            modelBuilder.Entity<PortDto>().HasData(
                new PortDto { Id = 1, Code = "ABCDE", Name = "Port A", AddedDate = DateTime.UtcNow, LastEditedDate = DateTime.UtcNow },
                new PortDto { Id = 2, Code = "FGHIJ", Name = "Port B", AddedDate = DateTime.UtcNow, LastEditedDate = DateTime.UtcNow }
            );

            modelBuilder.Entity<TerminalDto>().HasData(
                new TerminalDto { Id = 1, Name = "Terminal 1", PortId = 1, Latitude = 40.7128, Longitude = -74.0060, AddedDate = DateTime.UtcNow, LastEditedDate = DateTime.UtcNow },
                new TerminalDto { Id = 2, Name = "Terminal 2", PortId = 2, Latitude = 34.0522, Longitude = -118.2437, AddedDate = DateTime.UtcNow, LastEditedDate = DateTime.UtcNow }
            );
        }

        public void SeedDataAsync()
        {
            // Ensure the database is created
            Database.EnsureCreated();

            if (!UserInfo.Any())
            {
                var user = new UserInfo { Username = "testuser", Email = "testuser@gmail.com" };
                user.PasswordHash = _passwordHasher.HashPassword(user, "1234");
                UserInfo.Add(user);
                SaveChanges();
            }
        }
    }
}
