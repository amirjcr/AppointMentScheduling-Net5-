using System;
using System.ComponentModel.DataAnnotations;


namespace Src.Models.ViewModels.Register
{
    public class RegisterVm
    {
        [Required]
        public string Name { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }


        [DataType(DataType.Password)]
        [Required]
        [Display(Name = " Your PassWord")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "{0} Should be less than {1} charcters and More than {2} charcters")]
        public string Password { get; set; }


        [Compare("Password", ErrorMessage = "Confirm password Is Not same With Your PassWord")]
        public string ConfirmPasswore { get; set; }

        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }
}