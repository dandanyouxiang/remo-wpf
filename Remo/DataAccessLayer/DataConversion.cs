using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace DataAccessLayer
{
    /// <summary>
    /// PrePostFixConvertor
    /// </summary>
    [ValueConversion(typeof(double), typeof(string))]
    [ValueConversion(typeof(int), typeof(string))]
    [ValueConversion(typeof(decimal), typeof(string))]
    public class PrePostFixConverter : IValueConverter
    {
        public enum ConversionTypesEnum { _double, _int, _decimal };
        private ConversionTypesEnum _conversionType = ConversionTypesEnum._double;
        public ConversionTypesEnum ConversionType { get { return _conversionType; } set { _conversionType = value; } }

        private int _decimals = 1;
        /// <summary>
        /// Број на децимални места
        /// </summary>
        public int Decimals { get { return _decimals; } set { _decimals = value; } }

        private string _prefix = "";
        /// <summary>
        /// Префикс
        /// </summary>
        public string Prefix { get { return _prefix; } set { _prefix = value; } }

        private string _postfix = "";
        /// <summary>
        /// Постфикс
        /// </summary>
        public string Postfix { get { return _postfix; } set { _postfix = value; } }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Prefix + " " + ConvertToString(value, culture) + " " + Postfix;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = System.Convert.ToString(value);
            //Во случај да неможе да го конвертира во број внесениот стринг да врати -1
            try
            {
                if (text.Contains(Prefix))
                {
                    text = text.Replace(Prefix, "");
                }
                if (text.Contains(Postfix))
                {
                    text = text.Replace(Postfix, "");
                }
                object retValue = ConvertToType(text, culture);
                return retValue;
            }
            catch
            {
                return "";
            }
        }
        protected virtual object ConvertToType(string s, CultureInfo culture)
        {
            switch (ConversionType)
            {
                case ConversionTypesEnum._double:   return Math.Round(Double.Parse(s, NumberStyles.Any, culture), Decimals);
                case ConversionTypesEnum._int:      return Int32.Parse(s);
                case ConversionTypesEnum._decimal:  return Math.Round(Decimal.Parse(s, NumberStyles.Any, culture), Decimals);
            }
            return null;
        }
        protected virtual string ConvertToString(object o, CultureInfo culture)
        {
            switch (ConversionType)
            {
                case ConversionTypesEnum._double: return Math.Round((double)o, Decimals).ToString("F"+Decimals, culture);
                case ConversionTypesEnum._int: return o.ToString();
                case ConversionTypesEnum._decimal: return Math.Round((decimal)o, Decimals).ToString("F" + Decimals, culture);
            }
            return "";

        }
    }
    [ValueConversion(typeof(DateTime), typeof(string))]
    public class DateTimeConverter : PrePostFixConverter
    {
        /// <summary>
        /*  Short Date d M/d/yyyy For example: 10/30/2008
            Long Date D dddd, MMMM dd, yyyy For example: Wednesday, January 30, 2008
            Long Date and Short Time f dddd, MMMM dd, yyyy HH:mm aa For example: Wednesday, January 30, 2008 10:00 AM
            Long Date and Long Time F dddd, MMMM dd, yyyy HH:mm:ss aa For example: Wednesday, January 30, 2008 10:00:23 AM
            ISO Sortable Standard s yyyy-MM-dd HH:mm:ss For example: 2008-01-30 10:00:23
            Month and Day M MMMM dd For example: January 30
            General G M/d/yyyy HH:mm:ss aa  (depends on locale-specific settings) For example: 10/30/2008 10:00:23 AM*/
        /// </summary>
        public string FormatString { get; set; }
        protected override string ConvertToString(object o, CultureInfo culture)
        {
            return ((DateTime)o).ToString(FormatString, culture);
        }
        protected override object ConvertToType(string s, CultureInfo culture)
        {
            return DateTime.Parse(s);
        }
    }


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
