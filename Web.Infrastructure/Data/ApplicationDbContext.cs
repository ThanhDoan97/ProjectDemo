using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Entities;

namespace Web.Infrastructure.Data   
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Product> Product { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /// Config FluentAPI 

            #region User
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd()
                .IsRequired();
            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Id).IsUnique();
            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Name)
                .HasColumnType("nvarchar")
                .HasMaxLength(50);
            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.UserName).IsUnique();
            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserName)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(50);
            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(50);
               
            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Role)
                .IsRequired();
            });

            #endregion
            #region
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd()
                .IsRequired();
            });
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Price)
                .IsRequired()
                .HasColumnType("decimal(12,2)");
            });
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Quantity)
                .IsRequired();
            });
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Description)
                .HasColumnType("nvarchar(500)");
            });
            #endregion
        }
    }
}
