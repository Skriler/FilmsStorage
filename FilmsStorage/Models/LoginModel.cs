using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FilmsStorage.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "LoginName", ResourceType = typeof(Resources.Views.Account.Login))]
        public string LoginName { get; set; }

        [Required]
        [Display(Name = "Password", ResourceType = typeof(Resources.Views.Account.Login))]
        public string Password { get; set; }
    }
}