using Microsoft.EntityFrameworkCore;

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
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<Order>().HasOne(o => o.User).
                WithMany(u => u.Orders).HasForeignKey(o => o.UserId).IsRequired().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Order>().HasOne(o => o.Product).
                WithMany(p => p.Orders).HasForeignKey(o => o.ProductId).IsRequired().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<TimeSheet>().HasOne(t => t.Business).
                WithMany(b => b.Workdays).HasForeignKey(t => t.BusinessID).IsRequired().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Business>().HasOne(b => b.User).WithMany(u => u.Businesses).HasForeignKey(b => b.UserID)
                .IsRequired().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Product>().HasOne(p => p.Business).WithMany(b => b.Products)
                .HasForeignKey(p => p.BusinessID).IsRequired().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Feedback>().HasOne(f => f.Business).WithMany(b => b.Feedbacks)
                .HasForeignKey(f => f.BusinessID).IsRequired().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Question>();
            base.OnModelCreating(modelBuilder);
        }

    }
}
