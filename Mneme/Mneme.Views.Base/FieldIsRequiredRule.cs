using Mneme.Model;
using System.Globalization;
using System.Windows.Controls;

namespace Mneme.Views.Base;

public class FieldIsRequiredRule : ValidationRule
{
	public override ValidationResult Validate(object value, CultureInfo cultureInfo)
	{
		return value == null
			? new ValidationResult(false, $"Field is required")
			: value is Source
			? ValidationResult.ValidResult
			: ((string)value).Length == 0 ? new ValidationResult(false, $"Field is required") : ValidationResult.ValidResult;
	}
}
