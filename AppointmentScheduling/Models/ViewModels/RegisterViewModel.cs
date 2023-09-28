using System.ComponentModel.DataAnnotations;

namespace AppointmentScheduling.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage ="the{0} must be at least{2} characters long", MinimumLength =7)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [StringLength(100, ErrorMessage = "Password and ConfirmPassword not mach")]

        public string ConfirmPassword { get; set; }
        [Required]
        [Display(Name ="RollName")]
        public string RollName { get; set; }
    }
}
