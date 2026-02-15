using System.ComponentModel.DataAnnotations;

namespace BeautyStandard.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "ФИО обязательно для заполнения")]
        [Display(Name = "ФИО клиента")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "ФИО должно содержать от 5 до 100 символов")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Телефон обязателен для заполнения")]
        [Display(Name = "Номер телефона")]
        [Phone(ErrorMessage = "Введите корректный номер телефона")]
        [StringLength(20, ErrorMessage = "Телефон не может превышать 20 символов")]
        public string Phone { get; set; } = string.Empty;

        [Display(Name = "Email адрес")]
        [EmailAddress(ErrorMessage = "Введите корректный email адрес")]
        [StringLength(100, ErrorMessage = "Email не может превышать 100 символов")]
        public string? Email { get; set; }

        [Display(Name = "Дата регистрации")]
        [DataType(DataType.Date)]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
    }
}