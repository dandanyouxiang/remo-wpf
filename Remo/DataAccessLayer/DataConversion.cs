using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace DataAccessLayer
{
    public enum ConversionType 
    {
        Milimetar,
        Second,
        Minute,
        Amper,
        Ohm,
        Celsius
    }

    /// <summary>
    /// Приказ во милиметри
    /// </summary>
    [ValueConversion(typeof(int), typeof(string))]
    public class everythingConvertor : IValueConverter
    {
        public ConversionType ConvType { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string retValue=System.Convert.ToString(value);

            switch (ConvType) 
            {
                case ConversionType.Amper: retValue += " A"; break;
                case ConversionType.Ohm: retValue += " Ohm"; break;
                case ConversionType.Celsius: retValue += " C"; break;
                case ConversionType.Second: retValue += " sec"; break;
                case ConversionType.Minute: retValue += " min"; break;
                case ConversionType.Milimetar: retValue += " mm"; break;
            
            }

            return value.ToString() + " mm";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object retValue = 0;
            string metric = " S";
            string textMM = System.Convert.ToString(value);

            switch (ConvType)
            {
                case ConversionType.Amper: metric= " A"; break;
                case ConversionType.Ohm: metric = " Ohm"; break;
                case ConversionType.Celsius: metric = " C"; break;
                case ConversionType.Second: metric = " sec"; break;
                case ConversionType.Minute: metric = " min"; break;
                case ConversionType.Milimetar: metric = " mm"; break;

            }

            //Во случај да неможе да го конвертира во број внесениот стринг да врати -1
            try
            {
                if (textMM.Contains(metric))
                {
                    retValue = System.Convert.ToInt32(System.Convert.ToString(value).Substring(0, textMM.Length - metric.Length));
                }
                else
                {
                    retValue = System.Convert.ToInt32(textMM);
                }

            }
            catch
            {
                retValue = null ;
            }

            return retValue;

        }
    }

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
    public class SecondConvertor : IValueConverter
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
