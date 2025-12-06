using Microsoft.EntityFrameworkCore;
using Demo3DAPI.Models;

namespace Demo3DAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<PlayerAccount> PlayerAccounts { get; set; }
        public DbSet<PlayerCharacter> PlayerCharacters { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<BillDetail> BillDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            var adminRole = new Role { Id = 1, Name = "Admin" };
            var userRole = new Role { Id = 2, Name = "User" };
            modelBuilder.Entity<Role>().HasData(adminRole, userRole);

            
            var adminPassword = BCrypt.Net.BCrypt.HashPassword("abc@123");

            modelBuilder.Entity<PlayerAccount>().HasData(new PlayerAccount
            {
                ID = 1,
                UserName = "admin",
                Password = adminPassword,
                FullName = "Admin",
                RoleID = 1,
                PhoneNumber = null 
            });

            
            modelBuilder.Entity<PlayerAccount>(entity =>
            {
                entity.Property(a => a.RoleID).HasDefaultValue(2);

                entity.HasMany(a => a.Characters)
                      .WithOne(c => c.PlayerAccount)
                      .HasForeignKey(c => c.PlayerAccountID)
                      .OnDelete(DeleteBehavior.Cascade); 

                entity.HasOne(a => a.Role)
                       .WithMany(r => r.PlayerAccounts)
                       .HasForeignKey(a => a.RoleID)
                       .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);
            });
            
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasOne(p => p.Category)
                      .WithMany(c => c.Products)
                      .HasForeignKey(p => p.CategoryID)
                      .OnDelete(DeleteBehavior.Restrict); 
            });

            
            modelBuilder.Entity<BillDetail>(entity =>
            {
                
                entity.HasOne(bd => bd.Bill)
                      .WithMany(b => b.BillDetails)
                      .HasForeignKey(bd => bd.BillId)
                      .OnDelete(DeleteBehavior.Cascade); 

                
                entity.HasOne(bd => bd.Product)
                      .WithMany(p => p.BillDetails)
                      .HasForeignKey(bd => bd.ProductId)
                      .OnDelete(DeleteBehavior.Restrict); 
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}