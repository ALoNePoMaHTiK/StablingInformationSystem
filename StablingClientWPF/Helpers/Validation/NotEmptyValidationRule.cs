using System.Globalization;
using System.Windows.Controls;

namespace StablingClientWPF.Helpers.Validation
{

    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString()) || value.ToString() == "0"
                ? new ValidationResult(false, "Поле не должно быть пустым")
                : ValidationResult.ValidResult;
        }
    }
}