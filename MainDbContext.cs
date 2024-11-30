using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teether.OperationalObjects;

namespace Teether
{
    public class MainDbContext(DbContextOptions<MainDbContext> options) : IdentityDbContext<User, IdentityRole<long>, long>(options)
    {
        public DbSet<Visit> Visits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Visit>(entity =>
            {
                entity.HasOne(v => v.Patient)
                      .WithMany()
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(v => v.Doctor)
                      .WithMany()
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }

        private sealed class RoleConfiguration : IEntityTypeConfiguration<IdentityRole<long>>
        {
            public void Configure(EntityTypeBuilder<IdentityRole<long>> builder)
            {
                builder.HasData(new IdentityRole<long> { Id = 1, Name = "Patient", NormalizedName = "patient" },
                                new IdentityRole<long> { Id = 2, Name = "Doctor", NormalizedName = "doctor" });
            }
        }
    }
}
