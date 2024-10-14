using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace PART2PROG.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }
        public DbSet<PART2PROG.Models.Claim> Claim { get; set; } = default!;
    } 
}
