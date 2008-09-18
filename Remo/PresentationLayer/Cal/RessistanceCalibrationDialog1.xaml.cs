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
using System.Windows.Shapes;

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for RessistanceCalibrationDialog.xaml
    /// </summary>
    public partial class RessistanceCalibrationDialog1 : Window
    {
        /// <summary>
        /// Извршеното мерење
        /// </summary>
        public EntityLayer.RessistanceCalMeasurenment RessistanceCalMeasurenment { get; set; }

        class StdRessistorCurrent
        {
            public double Current { get; set; }
            public double RStd { get; set; }
        }
        private List<StdRessistorCurrent> list;
        public RessistanceCalibrationDialog1()
        {
            InitializeComponent();
            list = new List<StdRessistorCurrent>();
            list.Add(new StdRessistorCurrent { Current = 0.3, RStd = 100.0 });
            list.Add(new StdRessistorCurrent { Current = 0.3, RStd = 10.0 });
            list.Add(new StdRessistorCurrent { Current = 0.5, RStd = 1.0 });
            list.Add(new StdRessistorCurrent { Current = 1.0, RStd = 0.1 });
            list.Add(new StdRessistorCurrent { Current = 2.0, RStd = 0.01 });
            list.Add(new StdRessistorCurrent { Current = 10.0, RStd = 0.001 });

            StdRessistorsCurrentList.ItemsSource = list;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (StdRessistorsCurrentList.SelectedItem != null)
            {
                RessistanceCalMeasurenment = new EntityLayer.RessistanceCalMeasurenment();
                RessistanceCalMeasurenment.Current = ((StdRessistorCurrent)StdRessistorsCurrentList.SelectedItem).Current;
                RessistanceCalMeasurenment.RRef = ((StdRessistorCurrent)StdRessistorsCurrentList.SelectedItem).RStd;

                RessistanceCalibrationDialog2 d2 = new RessistanceCalibrationDialog2(RessistanceCalMeasurenment);
                if (d2.ShowDialog() == true)
                {
                    RessistanceCalMeasurenment.Time = DateTime.Now;
                    this.DialogResult = true;
                }
                else
                {
                    this.DialogResult = false;
                }
            }

        }
    }
}
