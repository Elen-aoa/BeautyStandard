using System.ComponentModel.DataAnnotations;

namespace BeautyStandard.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Название категории обязательно")]
        [Display(Name = "Название категории")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Название должно быть от 2 до 50 символов")]
        public string Name { get; set; } = string.Empty;

        // Навигационное свойство
        public virtual ICollection<Product>? Products { get; set; }
    }
}