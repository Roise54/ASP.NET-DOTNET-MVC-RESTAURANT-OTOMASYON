using Microsoft.EntityFrameworkCore;
using RestoranOtomasyonu.Models;

namespace RestoranOtomasyonu.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Kebapçı kategorileri
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Kebaplar", Description = "Lezzetli kebap çeşitlerimiz", DisplayOrder = 1 },
                new Category { Id = 2, Name = "Pideler", Description = "Fırından taze pideler", DisplayOrder = 2 },
                new Category { Id = 3, Name = "Çorbalar", Description = "Sıcak çorbalar", DisplayOrder = 3 },
                new Category { Id = 4, Name = "Salatalar", Description = "Taze salatalar", DisplayOrder = 4 },
                new Category { Id = 5, Name = "İçecekler", Description = "Soğuk ve sıcak içecekler", DisplayOrder = 5 },
                new Category { Id = 6, Name = "Tatlılar", Description = "Geleneksel tatlılar", DisplayOrder = 6 }
            );

            // Örnek ürünler
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Adana Kebap", Description = "Zırh ile çekilmiş et ve kuyruk yağı", Price = 120.00M, CategoryId = 1, PortionSize = "1 Porsiyon", IsSpicy = true },
                new Product { Id = 2, Name = "Urfa Kebap", Description = "Zırh ile çekilmiş et", Price = 120.00M, CategoryId = 1, PortionSize = "1 Porsiyon", IsSpicy = false },
                new Product { Id = 3, Name = "Kuşbaşı Kebap", Description = "Kuşbaşı doğranmış dana eti", Price = 140.00M, CategoryId = 1, PortionSize = "1 Porsiyon", IsSpicy = false },
                new Product { Id = 4, Name = "Kıymalı Pide", Description = "El açması hamur ile", Price = 80.00M, CategoryId = 2, PortionSize = "1 Adet", IsSpicy = false },
                new Product { Id = 5, Name = "Kuşbaşılı Pide", Description = "El açması hamur ile", Price = 90.00M, CategoryId = 2, PortionSize = "1 Adet", IsSpicy = false },
                new Product { Id = 6, Name = "Mercimek Çorbası", Description = "Geleneksel mercimek çorbası", Price = 35.00M, CategoryId = 3, PortionSize = "1 Kase", IsSpicy = false },
                new Product { Id = 7, Name = "Mevsim Salata", Description = "Taze mevsim yeşillikleri", Price = 40.00M, CategoryId = 4, PortionSize = "1 Porsiyon", IsSpicy = false },
                new Product { Id = 8, Name = "Ayran", Description = "Taze ayran", Price = 15.00M, CategoryId = 5, PortionSize = "1 Bardak", IsSpicy = false },
                new Product { Id = 9, Name = "Künefe", Description = "Antep fıstıklı künefe", Price = 70.00M, CategoryId = 6, PortionSize = "1 Porsiyon", IsSpicy = false }
            );
        }
    }
} 