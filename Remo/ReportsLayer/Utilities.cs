using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportsLayer
{
    class ReportStringFormater
    {
        /// <summary>
        /// Статичка метода за форматирање на температура.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string temperatureFormater(object value) 
        {
            if (value==null || value.ToString() == "NaN") 
            {
                return "---";
            }
            else 
            {
                return String.Format("{0:0.0 C}",value);
            }
        }
        /// <summary>
        /// Статичка метода за форматирање на отпор.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string resistanceFormater(object value)
        {
            if (value.ToString() == "NaN")
            {
                return "---";
            }
            else
            {
                return String.Format("{0:0.000000 Ohm}", value);
            }
        }
        /// <summary>
        /// Статичка метода за форматирање на струјата.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string curentFormater(object value)
        {
            if (value.ToString() == "NaN")
            {
                return "---";
            }
            else
            {
                return String.Format("{0:0.0 А}", value);
            }
        }
        /// <summary>
        /// Статичка метода за форматирање на секундите.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string secondFormater(object value)
        {
            if (value.ToString() == "NaN")
            {
                return "---";
            }
            else
            {
                return String.Format("{0:0.0 s}", value);
            }
        }
        /// <summary>
        /// Статичка метода за форматирање на проценти.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string percentFormater(object value)
        {
            if (value.ToString() == "NaN")
            {
                return "---";
            }
            else
            {
                return String.Format("{0:0.000 %}", value);
            }
        }
    }
}
