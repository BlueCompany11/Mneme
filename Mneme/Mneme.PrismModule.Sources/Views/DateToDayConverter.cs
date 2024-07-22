using System;
using System.Globalization;
using System.Windows.Data;

namespace Mneme.PrismModule.Sources.Views;

public class DateToDayConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is DateTime dateTime ? dateTime.Date : value;

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
