using System.ComponentModel.DataAnnotations;

namespace Allurerealstate.Models
{
    public class FormModel
    {
        [Required(ErrorMessage = "İsim girilmesi gerekmektedir!!")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Soyad girilmesi gerekmektedir!!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "E-Posta adresi girilmesi gerekmektedir!!")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Telefon numarası girilmesi gerekmektedir!!")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Sadece rakam giriniz!")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Mesaj alanı zorunludur!")]
        public string Message { get; set; }
    }

    public class CaptchaResponse
    {
        public bool Success { get; set; }
        public double Score { get; set; }
    }
}
