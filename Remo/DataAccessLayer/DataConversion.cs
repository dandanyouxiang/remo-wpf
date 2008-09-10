using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace DataAccessLayer
{
    /// <summary>
    /// Приказ во милиметри
    /// </summary>
    [ValueConversion(typeof(int), typeof(string))]
    public class MiliMetarConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString() + " mm";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int number = 0;
            string textMM = System.Convert.ToString(value);

            //Во случај да неможе да го конвертира во број внесениот стринг да врати -1
            try
            {
                if (textMM.Contains("mm"))
                {
                    number = System.Convert.ToInt32(System.Convert.ToString(value).Substring(0, textMM.Length - 3));
                }
                else 
                {
                    number = System.Convert.ToInt32(textMM);
                }

            }
            catch
            {
                number = -1;
            }

            return number;

        }
    }
    /// <summary>
    /// Приказ во секунди
    /// </summary>
    [ValueConversion(typeof(int), typeof(string))]
    public class SecundConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString() + " sec";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int number = 0;
            string textMM = System.Convert.ToString(value);

            //Во случај да неможе да го конвертира во број внесениот стринг да врати -1
            try
            {
                if (textMM.Contains("sec"))
                {
                    number = System.Convert.ToInt32(System.Convert.ToString(value).Substring(0, textMM.Length - 4));
                }
                else
                {
                    number = System.Convert.ToInt32(textMM);
                }

            }
            catch
            {
                number = -1;
            }

            return number;

        }
    }
    /// <summary>
    /// Приказ во amпери
    /// </summary>
    [ValueConversion(typeof(int), typeof(string))]
    public class AmperConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString() + " A";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int number = 0;
            string textMM = System.Convert.ToString(value);

            //Во случај да неможе да го конвертира во број внесениот стринг да врати -1
            try
            {
                if (textMM.Contains("A"))
                {
                    number = System.Convert.ToInt32(System.Convert.ToString(value).Substring(0, textMM.Length - 2));
                }
                else
                {
                    number = System.Convert.ToInt32(textMM);
                }

            }
            catch
            {
                number = -1;
            }

            return number;

        }
    }
    /// <summary>
    /// Приказ во минути
    /// </summary>
    [ValueConversion(typeof(int), typeof(string))]
    public class MinuteConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString() + " min";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int number = 0;
            string textMM = System.Convert.ToString(value);

            //Во случај да неможе да го конвертира во број внесениот стринг да врати -1
            try
            {
                if (textMM.Contains("min"))
                {
                    number = System.Convert.ToInt32(System.Convert.ToString(value).Substring(0, textMM.Length - 4));
                }
                else
                {
                    number = System.Convert.ToInt32(textMM);
                }

            }
            catch
            {
                number = -1;
            }

            return number;

        }
    }
    /// <summary>
    /// Приказ во оми
    /// </summary>
    [ValueConversion(typeof(int), typeof(string))]
    public class OhmConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString() + " Ohm";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int number = 0;
            string textMM = System.Convert.ToString(value);

            //Во случај да неможе да го конвертира во број внесениот стринг да врати -1
            try
            {
                if (textMM.Contains("Ohm"))
                {
                    number = System.Convert.ToInt32(System.Convert.ToString(value).Substring(0, textMM.Length - 4));
                }
                else
                {
                    number = System.Convert.ToInt32(textMM);
                }

            }
            catch
            {
                number = -1;
            }

            return number;

        }
    }
    /// <summary>
    /// Приказ во целзиусови
    /// </summary>
    [ValueConversion(typeof(int), typeof(string))]
    public class CelsiusConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString() + " C";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int number = 0;
            string textMM = System.Convert.ToString(value);

            //Во случај да неможе да го конвертира во број внесениот стринг да врати -1
            try
            {
                if (textMM.Contains(" C"))
                {
                    number = System.Convert.ToInt32(System.Convert.ToString(value).Substring(0, textMM.Length - 2));
                }
                else
                {
                    number = System.Convert.ToInt32(textMM);
                }

            }
            catch
            {
                number = -1;
            }

            return number;

        }
    }
}
