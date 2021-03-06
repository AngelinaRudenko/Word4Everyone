using System.ComponentModel.DataAnnotations;

namespace Word4Everyone.Model
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Не указан адрес электронной почты.")]
        [EmailAddress(ErrorMessage = "Неверный Email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указан пароль.")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Пароль должен содержать от {2} до {1} символов.")]
        public string Password { get; set; }
    }
}
