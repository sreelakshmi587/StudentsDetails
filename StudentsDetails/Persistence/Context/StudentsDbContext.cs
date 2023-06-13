using Microsoft.EntityFrameworkCore;
using StudentsDetails.Model;

namespace StudentsDetails.Persistence.Context
{
    public class StudentsDbContext : DbContext
    {
        public StudentsDbContext(DbContextOptions<StudentsDbContext> options) : base(options)
        {

        }

        public DbSet<StudentDetails> StudentDetails { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentDetails>().HasKey(s => s.Id);
        }
    }
}
