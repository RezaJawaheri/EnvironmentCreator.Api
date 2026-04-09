using EnvironmentCreator.Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EnvironmentCreator.Api.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Environment2D> Environments2D { get; set; }
        public DbSet<Object2D> Objects2D { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Environment2D>()
                .HasMany(e => e.Objects)
                .WithOne(o => o.Environment2D)
                .HasForeignKey(o => o.Environment2DId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Environment2D>()
                .HasIndex(e => new { e.UserId, e.Name })
                .IsUnique();
        }
    }
}