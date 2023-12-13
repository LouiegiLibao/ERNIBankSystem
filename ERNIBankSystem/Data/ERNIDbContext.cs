using ERNIBankSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ERNIBankSystem.Data
{
    public class ERNIDbContext:DbContext
    {
        public ERNIDbContext(DbContextOptions<ERNIDbContext> options) : base(options)
        {

        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
