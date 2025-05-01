using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ACBackendAPI.Domain.Entities;

namespace ACBackendAPI.Persistence.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Student>()
                .HasOne(s => s.User)
                .WithOne(u => u.StudentProfile)
                .HasForeignKey<Student>(s => s.UserId);

            builder.Entity<Teacher>()
                .HasOne(t => t.User)
                .WithOne(u => u.TeacherProfile)
                .HasForeignKey<Teacher>(t => t.UserId);

            builder.Entity<Admin>()
                .HasOne(a => a.User)
                .WithOne(u => u.AdminProfile)
                .HasForeignKey<Admin>(a => a.UserId);
        }
    }
}
