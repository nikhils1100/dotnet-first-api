using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD_NwDb.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            // Here, configuration paramaters are passed for configuration of our database
        }

        public DbSet<CRUD_NwDb.Models.Customer> Customer { get; set; }
        public DbSet<CRUD_NwDb.Models.Order> Order { get; set; }
    }
}
