using Microsoft.EntityFrameworkCore;
using StudentManagement.Model;
using StudentManagement.Models;
using System.Collections.Generic;

namespace StudentManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Principal> Principals { get; set; }
    }
}
