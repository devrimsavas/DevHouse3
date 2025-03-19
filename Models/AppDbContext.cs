using Microsoft.EntityFrameworkCore;
using DevHouse1.Models;


namespace DevHouse1.Models
{

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ProjectType> ProjectTypes { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ProjectType -> Projects (One-to-Many)
            modelBuilder.Entity<ProjectType>()
                .HasMany(pt => pt.Projects)
                .WithOne(p => p.ProjectType)
                .HasForeignKey(p => p.ProjectTypeId);

            // Team -> Projects (One-to-Many)
            modelBuilder.Entity<Team>()
                .HasMany(t => t.Projects)
                .WithOne(p => p.Team)
                .HasForeignKey(p => p.TeamId);

            // Team -> Developers (One-to-Many)
            modelBuilder.Entity<Team>()
                .HasMany(t => t.Developers)
                .WithOne(d => d.Team)
                .HasForeignKey(d => d.TeamId);

            // Role -> Developers (One-to-Many)
            modelBuilder.Entity<Role>()
                .HasMany(r => r.Developers)
                .WithOne(d => d.Role)
                .HasForeignKey(d => d.RoleId);
        }
    }

}

