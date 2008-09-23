﻿using System;
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
using DataAccessLayer;

namespace DataAccessLayer.PresentationLayer
{
    /// <summary>
    /// Interaction logic for TransformatorProperties.xaml
    /// </summary>
    public partial class TransformatorProperties : Window
    {
        DataSource dataSource;

        private string[] CoefValue=new string[3]{"235","225","0"};

        public TransformatorProperties()
        {
            InitializeComponent();
            dataSource = new DataSource(@"E:\root.xml");
            MainGrid.DataContext = dataSource.Root.TransformerProperties;
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ValueHVtextBox!=null)
            ValueHVtextBox.Text = CoefValue[comboBox1.SelectedIndex];
        }


        private void LVCoefComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ValueLVtextBox != null)
                ValueLVtextBox.Text = CoefValue[LVCoefComboBox.SelectedIndex];
        }
        /// <summary>
        /// При Validation Error на FrameworkElement компонента,
        /// да се врати последната валидна вредност (вредноста во binding source-от)
        /// </summary>
        private void OnValidationError(object sender, ValidationErrorEventArgs e)
        {

            ((FrameworkElement)e.OriginalSource).GetBindingExpression(TextBox.TextProperty).UpdateTarget();
        }
    }
}
