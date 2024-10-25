using Microsoft.EntityFrameworkCore;

namespace Profile.Models
{
    public class ProfileContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-P5N97KE; Database=Profile; Trusted_Connection=True; TrustServerCertificate=True ");
            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UsersRoles { get; set; }
    }
}
