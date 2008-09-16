using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DNBSoft.Generics;

namespace DNBSoft.WPF.WPFGraph
{
    public class WPFRenderParameters
    {
        private Canvas canvas;
        private double offsetX;
        private double offsetY;
        private double scaleX;
        private double scaleY;
        private double height;

        internal WPFRenderParameters(Canvas canvas, 
            double offsetX, double offsetY,
            double scaleX, double scaleY,
            double height)
        {
            this.canvas = canvas;
            this.offsetX = offsetX;
            this.offsetY = offsetY;
            this.scaleX = scaleX;
            this.scaleY = scaleY;
            this.height = height;
        }

        public double transpose(WPFGraphEnumerations.Axis axis, double value)
        {
            if (axis == WPFGraphEnumerations.Axis.X)
            {
                return (value - offsetX) * scaleX;
            }
            else
            {
                return (((value - offsetY) * scaleY) * -1.0) + height;
            }
        }

        public double untranspose(WPFGraphEnumerations.Axis axis, double value)
        {
            if (axis == WPFGraphEnumerations.Axis.X)
            {
                return (value / scaleX) + offsetX;
            }
            else
            {
                return (((value - height) / -1.0) / scaleY) + offsetY;
            }
        }

        public Canvas Canvas 
        {
            get
            {
                return canvas;
            }
        }
    }
}
