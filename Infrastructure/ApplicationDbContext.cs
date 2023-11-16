using Microsoft.EntityFrameworkCore;

namespace HierarchicalDirectoryStructure.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Models.Directory> Directories { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Models.Directory>()
                .HasOne(c => c.Parent)
                .WithMany(c => c.Children)
                .HasForeignKey(c => c.ParentId);

            modelBuilder.Entity<Models.Directory>().HasData(
                new Models.Directory { Id = 1, Name = "Creating Digital Images", ParentId = null },
                new Models.Directory { Id = 2, Name = "Resources", ParentId = 1 },
                new Models.Directory { Id = 3, Name = "Evidence", ParentId = 1 },
                new Models.Directory { Id = 4, Name = "Graphic Products", ParentId = 1 },
                new Models.Directory { Id = 5, Name = "Primary Sources", ParentId = 2 },
                new Models.Directory { Id = 6, Name = "Secondary Sources", ParentId = 2 },
                new Models.Directory { Id = 7, Name = "Process", ParentId = 4 },
                new Models.Directory { Id = 8, Name = "Final Product", ParentId = 4 }
            );
        }
    }
}
