using API.KM58.Model;
using Microsoft.EntityFrameworkCore;

namespace API.KM58.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Site> Sites { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public DbSet<SMS> SMS { get; set; }
		public DbSet<LogAccount> LogAccounts { get; set; }
        public DbSet<SMSRawData> SMSRawData { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
