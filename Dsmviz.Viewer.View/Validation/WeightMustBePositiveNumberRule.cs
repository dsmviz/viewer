using System.Windows.Controls;

namespace Dsmviz.Viewer.View.Validation
{
    public class WeightMustBePositiveNumberRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            ValidationResult result;
            if (int.TryParse(value.ToString(), out var weight))
            {
                result = new ValidationResult(false, "Please enter a valid integer value.");
            }
            else
            {
                result = weight <= 0 ? new ValidationResult(false, "Please enter a positive integer value.") : new ValidationResult(true, null);
            }
            return result;
        }
    }
}
