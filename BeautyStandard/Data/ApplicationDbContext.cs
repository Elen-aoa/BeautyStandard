using Microsoft.EntityFrameworkCore;
using BeautyStandard.Models;
using global::BeautyStandard.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace BeautyStandard.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка связи Product-Category
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // Запрет каскадного удаления

            // Уникальность названия товара в рамках категории
            modelBuilder.Entity<Product>()
                .HasIndex(p => new { p.Name, p.CategoryId })
                .IsUnique()
                .HasDatabaseName("IX_Product_Name_CategoryId_Unique");

            // Начальные данные для тестирования
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Косметика" },
                new Category { Id = 2, Name = "Уход за волосами" },
                new Category { Id = 3, Name = "Аксессуары" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Тональный крем", Price = 850, Stock = 15, CategoryId = 1 },
                new Product { Id = 2, Name = "Шампунь", Price = 450, Stock = 30, CategoryId = 2 },
                new Product { Id = 3, Name = "Расческа", Price = 250, Stock = 50, CategoryId = 3 }
            );
        }
    }
}