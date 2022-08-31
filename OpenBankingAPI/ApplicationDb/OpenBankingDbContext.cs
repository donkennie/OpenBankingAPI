using Microsoft.EntityFrameworkCore;
using OpenBankingAPI.Models;

namespace OpenBankingAPI.ApplicationDb
{
    public class OpenBankingDbContext : DbContext
    {

        public OpenBankingDbContext(DbContextOptions<OpenBankingDbContext> options) : base(options)
        {

        }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

    }
}
