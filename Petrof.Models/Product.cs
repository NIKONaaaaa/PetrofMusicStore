namespace Petrof.Models
{
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        [Range(1, 25000)]
        public double Weight { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }
        public int BrandId { get; set; }
        [ForeignKey("BrandId")]
        [ValidateNever]
        public Brand Brand { get; set; }
        [ValidateNever]
        public List<ProductImage> ProductImages { get; set; }
        [DisplayName("Video URL")]
        public string? VideoUrl { get; set; }
        [DisplayName("Available")]
        public bool InStock { get; set; }
    }
}