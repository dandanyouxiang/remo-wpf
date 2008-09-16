using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DNBSoft.WPF.WPFGraph
{
    public abstract class WPFGraphExceptions
    {
        public class ParameterFormatException : Exception
        {
            public ParameterFormatException()
                : base("Incorrect parameter format exception")
            {
            }
        }

        public class PropertyNotFoundException : Exception
        {
            public PropertyNotFoundException()
                : base("Property not part of object exception")
            {
            }
        }

        public class ParameterNullException : Exception
        {
            public ParameterNullException()
                : base("Value cannot be null")
            {
            }
        }
    }
}
