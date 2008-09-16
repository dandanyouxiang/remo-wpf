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
using System.Globalization;
using System.Windows.Controls.Primitives;

namespace WpfCustomControlLibrary
{
    public class MyTickBarWithNumbers : TickBar
    {
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            
            FormattedText minText = null;
            minText = new FormattedText(Minimum.ToString(), CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 8,  Brushes.Black);
            minText.TextAlignment = TextAlignment.Right;
            dc.DrawText(minText, new Point(-3, base.ActualHeight - 5)  );
            FormattedText maxText = null;
            maxText = new FormattedText(Maximum.ToString(), CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 8, Brushes.Black);
            maxText.TextAlignment = TextAlignment.Right;
            dc.DrawText(maxText, new Point(-3, - 5 ));
        }
}
}
