using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Globalization;

namespace DataAccessLayer
{
    public class RegexValidator : ValidationRule
    {
        public string RegularExpression { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            Regex regex = new Regex(RegularExpression);
            if (regex.IsMatch(value.ToString()))
            {
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false, "Validation failed");
            }
        }
    }

    class ConvertInput 
    {
        static public void TryConvertInteger(object value,string metric) 
        {
            string StringValue = System.Convert.ToString(value);
            if (StringValue.Contains(metric)) StringValue = StringValue.Substring(0,StringValue.Length-metric.Length+1);
            System.Convert.ToInt32(StringValue);
        }
        static public void TryConvertDouble(object value, string metric)
        {
            string StringValue = System.Convert.ToString(value);
            if (StringValue.Contains(metric)) StringValue = StringValue.Substring(0, StringValue.Length - metric.Length + 1);
            System.Convert.ToDouble(StringValue);
        }
        static public void CheckLength(object value, int length)
        {
            string StringValue = System.Convert.ToString(value);
            if (StringValue.Length>length) throw new System.OverflowException();
        }
        static public void Interval(object value,int min,int max) 
        {
           int IntValue = System.Convert.ToInt32(value);
           if (min>IntValue || max<IntValue) throw new Exception();
        }
        static public void IntervalDouble(object value, double min, double max)
        {
            double IntValue = System.Convert.ToInt32(value);
            if (min > IntValue || max < IntValue) throw new Exception();
        }
    }
    class IntegerValidate : ValidationRule
    {
        /// <summary>
        /// Минимална вредност што може да ја прими.
        /// </summary>
        int Min = Int32.MinValue;
        /// <summary>
        /// Минимална вредност што може даја има.
        /// </summary>
        int Max = Int32.MaxValue;
        /// <summary>
        /// Единици во кој се мери.
        /// </summary>
        string Metric = "";
        /// <summary>
        /// Конструктор со еден параметар.
        /// </summary>
        /// <param name="metric">Во какви единици се мери.</param>
        public IntegerValidate(string metric)
        {
            this.Metric = metric;
        }
        /// <summary>
        /// Конструктор со три параметри.
        /// </summary>
        /// <param name="metric">Во какви единици се мери.</param>
        /// <param name="min">Минимална вредност што може да ја има.</param>
        /// <param name="max">Максимална вредност што може да ја има.</param>
        public IntegerValidate(string metric, int min, int max):this(metric)
        {
            this.Min = min;
            this.Max = max;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            try
            {
                ConvertInput.TryConvertInteger(value, Metric);
            }
            //За празен стринг
            catch (System.ArgumentNullException)
            {
                return new ValidationResult(false, "You must insert value!");
            }
            //Ако не е внесен број
            catch (System.FormatException)
            {
                return new ValidationResult(false, "You must insert cardinal value!");
            }

            try
            {
                ConvertInput.Interval(value, Min,Max );
            }
            catch (Exception)
            {
                return new ValidationResult(false, "The value is out of range!");
            }

            return new ValidationResult(true, null);
        }
    }
    /// <summary>
    /// Валидација на дабл вредности.
    /// </summary>
    class DoubleValidate : ValidationRule
    {
        /// <summary>
        /// Минимална вредност што може да ја прими.
        /// </summary>
        double Min = Double.MinValue;
        /// <summary>
        /// Максимална вредност што може да ја има.
        /// </summary>
        double Max = Double.MaxValue;
        /// <summary>
        /// Единици во кој се мери.
        /// </summary>
        string Metric = "";
        /// <summary>
        /// Конструктор со еден параметар.
        /// </summary>
        /// <param name="metric">Во какви единици се мери.</param>
        public DoubleValidate(string metric)
        {
            this.Metric = metric;
        }
        /// <summary>
        /// Конструктор со три параметри.
        /// </summary>
        /// <param name="metric">Во какви единици се мери.</param>
        /// <param name="min">Минимална вредност што може да ја има.</param>
        /// <param name="max">Максимална вредност што може да ја има.</param>
        public DoubleValidate(string metric, double min, double max): this(metric)
        {
            this.Min = min;
            this.Max = max;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            try
            {
                ConvertInput.TryConvertDouble(value, Metric);
            }
            //За празен стринг
            catch (System.ArgumentNullException)
            {
                return new ValidationResult(false, "You must insert value!");
            }
            //Ако не е внесен број
            catch (System.FormatException)
            {
                return new ValidationResult(false, "You must insert cardinal value!");
            }

            try
            {
                ConvertInput.IntervalDouble(value, Min, Max);
            }
            catch (Exception)
            {
                return new ValidationResult(false, "The value is out of range!");
            }

            return new ValidationResult(true, null);
        }
    }
}
