using System;
using Microsoft.EntityFrameworkCore;

namespace orbitgroup.api.Entities
{
    public class StudentDbContext: DbContext
    {
        public DbSet<Student> Students { get; set; }

        public StudentDbContext()
        {
        }

        public StudentDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
