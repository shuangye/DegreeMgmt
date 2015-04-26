using DegreeMgmt.Models.Entities;
using System.Data.Entity;

namespace DegreeMgmt.Models
{
    public class EfDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }

        public DbSet<Degree> Degrees { get; set; }
    }
}