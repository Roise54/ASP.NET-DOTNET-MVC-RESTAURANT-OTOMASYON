using System.ComponentModel.DataAnnotations;

namespace RestoranOtomasyonu.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ürün adı zorunludur")]
        [Display(Name = "Ürün Adı")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Fiyat zorunludur")]
        [Display(Name = "Fiyat")]
        [Range(0, 10000, ErrorMessage = "Fiyat 0-10000 TL arasında olmalıdır")]
        public decimal Price { get; set; }

        [Display(Name = "Resim URL")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Porsiyon")]
        public string? PortionSize { get; set; }

        [Display(Name = "Acılı mı?")]
        public bool IsSpicy { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
} 