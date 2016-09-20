using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Host.UI.Home
{
    /// <summary>
    /// Customization [Create]
    /// </summary>
    public class NewPasswordViewModel
    {
        [Required(ErrorMessage = "Please enter your password!")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)[a-zA-Z\d._^%$#!~@,-]{8,}$", ErrorMessage = "Password should be containing at least 8 characters, 1 capital letter and 1 number")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter confirmed password!")]
        [Compare("Password", ErrorMessage = "Repeat password did not match!")]
        public string ConfirmPassword { get; set; }
        public string TokenKey { get; set; }
        public string ReturnUrl { get; set; }
    }
}
