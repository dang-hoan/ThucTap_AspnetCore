using LearnAspNetCoreMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnAspNetCoreMVC.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Company> Companies { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(b => b.Company)
                .WithMany(a => a.Products)
                .HasForeignKey(b => b.CompanyID);
        }
    }
}
