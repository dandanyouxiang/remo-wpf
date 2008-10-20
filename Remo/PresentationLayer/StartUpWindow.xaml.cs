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

namespace PresentationLayer
{
    
    /// <summary>
    /// Interaction logic for StartUpWindow.xaml
    /// </summary>
    public partial class StartUpWindow : Window
    {
        public FileCommand fileCommand { get; set; }

        public StartUpWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            fileCommand = FileCommand.New;
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            fileCommand = FileCommand.Open;
            this.Close();
        }

    }
}
