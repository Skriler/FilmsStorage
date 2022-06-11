using System.ComponentModel.DataAnnotations;
using FilmsStorage.Validators;

namespace FilmsStorage.Models
{
    public class RegisterModel
    {
        [Required]
        [Display(Name = "UserName", ResourceType = typeof(Resources.Views.Account.Register))]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "LoginName", ResourceType = typeof(Resources.Views.Account.Register))]
        public string LoginName { get; set; }

        [Required]
        [AuditPassword(MinimumLength = 5, RequireUpperLowerMix = true, MinimumSpecialSymbols = 4)]
        [Display(Name = "Password", ResourceType = typeof(Resources.Views.Account.Register))]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        [Display(Name = "PasswordRepetition", ResourceType = typeof(Resources.Views.Account.Register))]
        public string PasswordRepetition { get; set; }
    }
}