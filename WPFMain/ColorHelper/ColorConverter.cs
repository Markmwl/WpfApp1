using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace WPFMain.ColorHelper
{
    [ValueConversion(typeof(string), typeof(string))]
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type typeTarget, object param, CultureInfo culture)
        {

            decimal numValue = System.Convert.ToDecimal(value.ToString());
            if (numValue > 2000)
            {
                return "Red";
            }
            return "Black";


        }
        public object ConvertBack(object value, Type typeTarget, object param, CultureInfo culture)
        {
            return "";
        }
    }
}
