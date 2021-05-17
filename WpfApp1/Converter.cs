using System;
using System.Globalization;
using System.Windows.Data;
using DataLibrary;

namespace WpfApp1
{
    [ValueConversion(typeof(V2DataOnGrid), typeof(string))]
    public class DataOnGridConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            V2DataOnGrid data = value as V2DataOnGrid;

            double max_mod = -1;
            double min_mod;

            if (data != null)
            {
                foreach (var item in data)
                {
                    if (Math.Sqrt(item.Compl.Real * item.Compl.Real + item.Compl.Imaginary * item.Compl.Imaginary) > max_mod)
                    {
                        max_mod = item.Compl.Real * item.Compl.Real + item.Compl.Imaginary * item.Compl.Imaginary;
                    }
                }

                min_mod = max_mod;

                foreach (var item in data)
                {
                    if (Math.Sqrt(item.Compl.Real * item.Compl.Real + item.Compl.Imaginary * item.Compl.Imaginary) < min_mod)
                    {
                        min_mod = item.Compl.Real * item.Compl.Real + item.Compl.Imaginary * item.Compl.Imaginary;
                    }
                }


                return
                    $"Max module value: {max_mod}\n" +
                    $"Min module value: {min_mod}\n" +
                    $"Number of Ox nodes: {data.Elem[0].Node}\n" +
                    $"Number of Ox steps: {data.Elem[0].Step}\n" +
                    $"Number of Oy nodes: {data.Elem[1].Node}\n" +
                    $"Number of Oy steps: {data.Elem[1].Step}\n";              
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    [ValueConversion(typeof(DataItem), typeof(string))]
    public class DataItemConverterCoord : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                DataItem data = (DataItem)value;
                return $"Vector2 = {data.Vect2}";
            }
            return "null occured";

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    [ValueConversion(typeof(DataItem), typeof(string))]
    public class DataItemConverterValue : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                DataItem data = (DataItem)value;
                return $"EMValue = {data.Compl}";
            }
            return "null occured";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    [ValueConversion(typeof(bool), typeof(string))]
    public class IsUnsavedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                bool isUnsaved = (bool)value;
                if (isUnsaved)
                {
                    return " Есть несохраненные изменения";
                }
                else
                {
                    return "Данные сохранены";
                }
            }
            return "";

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

}
