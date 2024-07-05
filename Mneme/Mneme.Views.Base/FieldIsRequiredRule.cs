using System.Globalization;
using System.Windows.Controls;
using Mneme.Model.Sources;

namespace Mneme.Views.Base
{
	public class FieldIsRequiredRule : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			if(value == null)
				return new ValidationResult(false, $"Field is required");
			if (value is Source)
				return ValidationResult.ValidResult;
			if (((string)value).Length == 0)
				return new ValidationResult(false, $"Field is required");
			return ValidationResult.ValidResult;
		}
	}
}
