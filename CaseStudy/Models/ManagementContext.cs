using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseStudy.Models
{
    public class ManagementContext:DbContext
    {
        public ManagementContext(DbContextOptions<ManagementContext> options) : base(options)
        {

        }
        public DbSet<Account> Account { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Error> Error { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasKey(ba => new { ba.AccountID, ba.CustomerID });
            modelBuilder.Entity<Transaction>().HasKey(ba => new { ba.TransactionID, ba.AccountID });
        }
    }
}
