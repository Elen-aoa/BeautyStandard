using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeautyStandard.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Название товара обязательно")]
        [Display(Name = "Наименование товара")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Название должно быть от 2 до 100 символов")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Цена обязательна")]
        [Display(Name = "Цена")]
        [Range(0.01, 1000000, ErrorMessage = "Цена должна быть от 0.01 до 1 000 000")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Остаток на складе обязателен")]
        [Display(Name = "Остаток на складе")]
        [Range(0, 10000, ErrorMessage = "Остаток должен быть от 0 до 10 000")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Категория обязательна")]
        [Display(Name = "Категория")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        [Display(Name = "Категория")]
        public virtual Category? Category { get; set; }
    }
}