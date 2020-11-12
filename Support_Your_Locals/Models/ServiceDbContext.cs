using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using BusinessEntity = Support_Your_Locals.Models.Business;

namespace Support_Your_Locals.Models
{
    public class ServiceDbContext : DbContext
    {

        public ServiceDbContext(DbContextOptions<ServiceDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Business> Business { get; set; }
        public DbSet<TimeSheet> TimeSheets { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TimeSheet>().HasOne(t => t.Business).
                WithMany(b => b.Workdays).HasForeignKey(t => t.BusinessID).IsRequired().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Business>().HasOne(b => b.User).WithMany(u => u.Businesses).HasForeignKey(b => b.UserID)
                .IsRequired().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Product>().HasOne(p => p.Business).WithMany(b => b.Products)
                .HasForeignKey(p => p.BusinessID).IsRequired().OnDelete(DeleteBehavior.Cascade);
            base.OnModelCreating(modelBuilder);
        }

    }
}
