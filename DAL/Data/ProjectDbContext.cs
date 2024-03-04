using Microsoft.EntityFrameworkCore;
using DAL.Entities;
using DataTask = DAL.Entities.Task;

namespace DAL.Data
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Project> Projects {  get; set; }
        public DbSet<DataTask> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
                .HasMany(p => p.Tasks)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId);
        }
    }
}
