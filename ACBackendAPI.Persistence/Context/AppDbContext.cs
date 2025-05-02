using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ACBackendAPI.Domain.Entities;
using Microsoft.AspNetCore.Identity;


public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Student> Students { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Guardian> Guardians { get; set; }
    public DbSet<AcademicInformation> AcademicInformations { get; set; }
    public DbSet<Programme> Programmes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure relationships
        modelBuilder.Entity<Student>()
            .HasOne(s => s.Guardian)
            .WithMany(g => g.Students)
            .HasForeignKey(s => s.GuardianId);

        modelBuilder.Entity<Student>()
            .HasOne(s => s.ApplicationUser)
            .WithOne(au => au.Student)
            .HasForeignKey<Student>(s => s.ApplicationUserId);

        modelBuilder.Entity<Admin>()
            .HasOne(a => a.ApplicationUser)
            .WithOne(au => au.Admin)
            .HasForeignKey<Admin>(a => a.ApplicationUserId);

        modelBuilder.Entity<AcademicInformation>()
            .HasOne(ai => ai.Programme)
            .WithMany(p => p.AcademicInformation)
            .HasForeignKey(ai => ai.ProgrammeId);

        modelBuilder.Entity<AcademicInformation>()
            .HasOne(ai => ai.Student)
            .WithOne(s => s.AcademicInformation)
            .HasForeignKey<AcademicInformation>(ai => ai.StudentId);

        // Unique constraints
        modelBuilder.Entity<Student>().HasIndex(s => s.Email).IsUnique();
        modelBuilder.Entity<Admin>().HasIndex(a => a.Email).IsUnique();
        modelBuilder.Entity<Guardian>().HasIndex(g => g.Email).IsUnique();
    }
}
