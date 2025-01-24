using System.ComponentModel.DataAnnotations;

namespace RestoranOtomasyonu.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kategori adı zorunludur")]
        [Display(Name = "Kategori Adı")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Display(Name = "Sıra No")]
        public int DisplayOrder { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
} 