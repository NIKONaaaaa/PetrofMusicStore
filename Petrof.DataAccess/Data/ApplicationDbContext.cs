namespace Petrof.DataAccess.Data
{
    using Petrof.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Accordion", DisplayOrder = 1 },
                new Category { Id = 2, Name = "Violin", DisplayOrder = 2 },
                new Category { Id = 3, Name = "Guitar", DisplayOrder = 3 },
                new Category { Id = 100, Name = "Accessories", DisplayOrder = 100 });
            modelBuilder.Entity<Brand>().HasData(
                new Brand { Id = 1, Name = "Weltmeister", DisplayOrder = 1 },
                new Brand { Id = 2, Name = "Yamaha", DisplayOrder = 2 },
                new Brand { Id = 3, Name = "Fender", DisplayOrder = 3 });
        }
    }
}