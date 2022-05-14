using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Globalization;
using ClassLibrary;

namespace Lab_U1
{
    public class VMAccuracyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null)
                {
                    VMAccuracy val = (VMAccuracy)value;
                    return $"WML_HA value: {val.HA_value:0.0000000}, WML_EP value: {val.EP_value:0.0000000}, Arg: {val.max_dif_arg:0.0000000}";
                }
                return "";
            }
            catch (Exception error)
            {
                MessageBox.Show($"Unexpected error: {error.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return "WML_HA value: ERROR, WML_EP value: ERROR";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new VMTime();
        }
    }
}
