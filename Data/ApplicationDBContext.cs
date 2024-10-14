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
    } 
}
