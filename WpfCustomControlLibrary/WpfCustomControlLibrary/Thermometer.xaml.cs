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

namespace WpfCustomControlLibrary
{
    /// <summary>
    /// Interaction logic for Thermometer.xaml
    /// </summary>
    public partial class Thermometer : UserControl
    {
        public static DependencyProperty MinimumProperty;
        public static DependencyProperty MaximumProperty; 
        public static DependencyProperty ValueProperty;
        public static DependencyProperty TicksProperty;
        public static DependencyProperty DecimalsProperty;


        public double EclipseHeight { get { return (this.Width - 30) / 2; } }
        public double EclipseWidth  { get { return this.Width - 30; } }

        static Thermometer()
        {
            FrameworkPropertyMetadata metadata = new FrameworkPropertyMetadata(new DoubleCollection() { 0, 50, 100 });
            metadata.AffectsRender = true;
            TicksProperty = DependencyProperty.Register("TicksProperty",
                typeof(DoubleCollection), typeof(Thermometer), metadata);

            FrameworkPropertyMetadata metadataMax = new FrameworkPropertyMetadata(100.0);
            metadataMax.AffectsRender = true;
            MaximumProperty = DependencyProperty.Register("MaximumProperty",
                typeof(double), typeof(Thermometer), metadataMax);

            FrameworkPropertyMetadata metadataMin = new FrameworkPropertyMetadata(0.0);
            metadataMin.AffectsRender = true;
            MinimumProperty = DependencyProperty.Register("MinimumProperty",
                typeof(double), typeof(Thermometer), metadataMin);

            FrameworkPropertyMetadata metadataValue = new FrameworkPropertyMetadata(0.0);
            metadataValue.AffectsRender = true;
            ValueProperty = DependencyProperty.Register("ValueProperty",
                typeof(double), typeof(Thermometer), metadataValue);

            FrameworkPropertyMetadata metadataDecimals = new FrameworkPropertyMetadata(0);
            metadataDecimals.AffectsRender = true;
            DecimalsProperty = DependencyProperty.Register("DecimalsProperty",
                typeof(int), typeof(Thermometer), metadataDecimals);
        }
        public Thermometer()
        {
            InitializeComponent(); 
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            TickBarsStackPanel.Children.Clear();
            TickBarsStackPanel.Margin = new Thickness(TickBarsStackPanel.Margin.Left, EclipseHeight, TickBarsStackPanel.Margin.Right, EclipseHeight);
            
            for (int i = Ticks.Count - 1; i >= 1; i--)
            {
                MyTickBarWithNumbers tick = new MyTickBarWithNumbers();
                tick.Decimals = Decimals;
                tick.Minimum = Ticks[i - 1];
                tick.Maximum = Ticks[i];
                tick.HorizontalAlignment = HorizontalAlignment.Right;
               
                //Ne se primeneti Marginite
                if(TickBarsStackPanel.ActualHeight == this.ActualHeight)
                    tick.Height = (TickBarsStackPanel.ActualHeight - 2* EclipseHeight) / (Ticks.Count - 1);
                if(TickBarsStackPanel.ActualHeight == this.ActualHeight - 2* EclipseHeight)
                    tick.Height = (TickBarsStackPanel.ActualHeight) / (Ticks.Count - 1);
                tick.Width = TickBarsStackPanel.ActualWidth * 0.4;

                double step = Ticks[i] - Ticks[i - 1];
                tick.Ticks = new DoubleCollection() { Ticks[i - 1] + step / 4, Ticks[i - 1] + step / 2, Ticks[i - 1] + 3 * step / 4 };
                tick.Fill = Brushes.Black;
                tick.Placement = System.Windows.Controls.Primitives.TickBarPlacement.Left;

                TickBarsStackPanel.Children.Add(tick);
            }
            base.OnRender(drawingContext); 
        }

        public void refreshFill()
        {
            double maxHeight = TickBarsStackPanel.ActualHeight;
            fillRectangle.Height = maxHeight * (Value - Minimum) / (Maximum - Minimum);
        }

        public double Minimum
        {
            get
            {
                return (double)GetValue(MinimumProperty);
            }
            set
            {
                SetValue(MinimumProperty, value);
                refreshFill();
            }
        }
        public double Maximum
        {
            get
            {
                return (double)GetValue(MaximumProperty);
            }
            set
            {
                SetValue(MaximumProperty, value);
                refreshFill();
            }
        }
        public double Value
        {
            get
            {
                return (double)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
                refreshFill();
            }
        }

        public int Decimals
        {
            get
            {
                return (int)GetValue(DecimalsProperty);
            }
            set
            {
                SetValue(DecimalsProperty, value);
            }
        }

        public DoubleCollection Ticks
        {
            get
            {
                return (DoubleCollection)GetValue(TicksProperty);
            }
            set
            {
                SetValue(TicksProperty, value);
            }
        }

        private void UserControl_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (((Thermometer)sender).IsEnabled == false)
            {
                this.EnabledBorder.Opacity = 0.4;
            }
            else
            {
                this.EnabledBorder.Opacity = 0;
            }
        }
    }
}
