using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DNBSoft.WPF.WPFGraph
{
    public class WPFGraphConfigurationParameter
    {
        private String parameter = null;
        private Type type = null;

        public WPFGraphConfigurationParameter(String parameter, Type type)
        {
            this.parameter = parameter;
            this.type = type;
        }

        public String Parameter
        {
            get
            {
                return parameter;
            }
        }

        public Type Type
        {
            get
            {
                return type;
            }
        }
    }
}
