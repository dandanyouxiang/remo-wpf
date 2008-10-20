using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PresentationLayer
{
    public class DataCommands
    {
        private static RoutedUICommand workPlacePathCommand;
        static DataCommands()
        {
            
            InputGestureCollection inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.P, ModifierKeys.Control, "Ctrl+P"));
            workPlacePathCommand = new RoutedUICommand(
              "WorkPlacePath", "WorkPlacePath", typeof(DataCommands), inputs);
             
        }

        public static RoutedUICommand WorkPlacePathCommand
        {
            get { return workPlacePathCommand; }
        }
    }
}
