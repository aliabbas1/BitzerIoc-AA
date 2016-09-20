using System.ComponentModel.DataAnnotations;

namespace Host.UI.Login
{
    public class LoginInputModel
    {
        [Required(ErrorMessage = "Please enter your email!")]
        [EmailAddress(ErrorMessage = "Please enter a valid email!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter your password!")]
        public string Password { get; set; }
        public bool RememberLogin { get; set; }
        public string ReturnUrl { get; set; }
    }
}