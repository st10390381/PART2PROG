using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PART2PROG.Models;
using System.Collections.Generic;

namespace PART2PROG.Data
{
    public class ApplicationDBContext : IdentityDbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }
        public DbSet<PART2PROG.Models.Claim> Claim { get; set; } = default!;
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Claim> Claims { get; set; }

        public DbSet<SupportingDocument> Doc { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Claim>()
                .HasOne(c => c.ApplicationUser)
                .WithMany(u => u.Claims)
                .HasForeignKey(c => c.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SupportingDocument>()
                .HasOne(d => d.Claim)
                .WithMany(c => c.Doc)
                .HasForeignKey(d => d.ClaimId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
