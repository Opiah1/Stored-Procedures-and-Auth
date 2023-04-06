using Microsoft.EntityFrameworkCore;
using SPandAuth.Models;
using SPandAuth.ResponseObj;

namespace SPandAuth.Data
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
        }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=bankdb;Trusted_Connection=true;TrustServerCertificate=true;");
        }
        public DbSet<User> Users => Set<User>();
        public DbSet<BalanceResponse> BalanceResponses => Set<BalanceResponse>();
    }
}
