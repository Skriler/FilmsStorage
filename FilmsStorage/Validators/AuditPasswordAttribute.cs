using System.ComponentModel.DataAnnotations;
using FilmsStorage.SL;

namespace FilmsStorage.Validators
{
    public class AuditPasswordAttribute : ValidationAttribute
    {
        public int MinimumLength { get; set; } = 8;

        public bool RequireUpperLowerMix { get; set; } = false;

        public int MinimumSpecialSymbols { get; set; } = 0;

        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            if (!(value is string))
                return false;

            string password = value as string;

            if (password.Length < MinimumLength)
            {
                ErrorMessage = "Password is too short";

                return false;
            }

            if (RequireUpperLowerMix && !_SL.StringValidator.IsAlternationUpperAndLowerCaseSymbols((value as string)))
            {
                ErrorMessage = "Password does not have alternation of uppercase and lowercase letters";

                return false;
            }

            if (!_SL.StringValidator.IsContainMinimumSpecialSymbols((value as string), MinimumSpecialSymbols))
            {
                ErrorMessage = "Password does not contain minimum special symbols";

                return false;
            }

            return true;
        }
    }
}