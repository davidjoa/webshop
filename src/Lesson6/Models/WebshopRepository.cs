using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lesson6.Models
{
    public class WebshopRepository : DbContext
    {

        public WebshopRepository(DbContextOptions<WebshopRepository> options) 
            : base(options)
        {}

        public DbSet<Product> Products {get; set;}
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductTranslation> ProductTranslations { get; set; }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ProductTranslation>().HasKey(x => new { x.ProductId, x.Language });
        }

    }
}
