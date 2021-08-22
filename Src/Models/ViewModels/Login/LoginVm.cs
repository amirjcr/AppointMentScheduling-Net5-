using System;
using System.ComponentModel.DataAnnotations;


namespace Src.Models.ViewModels.Login
{
    public class LoginVm
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remmber Me ")]

        public bool RemmberMe { get; set; }
    }
}