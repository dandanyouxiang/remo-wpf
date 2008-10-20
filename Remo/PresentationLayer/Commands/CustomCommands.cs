using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PresentationLayer
{
    public class WorkPlacePathCommand
    {
        private static RoutedUICommand workPlacePathCommand;
        static WorkPlacePathCommand()
        {
            
            InputGestureCollection inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.W, ModifierKeys.Control, "Ctrl+W"));
            workPlacePathCommand = new RoutedUICommand(
              "WorkPlacePath", "WorkPlacePath", typeof(WorkPlacePathCommand), inputs);
             
        }

        public static RoutedUICommand Command
        {
            get { return workPlacePathCommand; }
        }
    }

    public class PrintCommand
    {
        private static RoutedUICommand printCommand;
        static PrintCommand()
        {
            InputGestureCollection inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.P, ModifierKeys.Control, "Ctrl+P"));
            printCommand = new RoutedUICommand(
              "PrintCommand", "PrintCommand", typeof(PrintCommand), inputs);
        }

        public static RoutedUICommand Command
        {
            get { return printCommand; }
        }
    }

    public class TrasformatorPropertiesCommand
    {
        private static RoutedUICommand printCommand;
        static TrasformatorPropertiesCommand()
        {
            InputGestureCollection inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.T, ModifierKeys.Control, "Ctrl+T"));
            printCommand = new RoutedUICommand(
              "TrasformatorProperties", "TrasformatorProperties", typeof(TrasformatorPropertiesCommand), inputs);
        }

        public static RoutedUICommand Command
        {
            get { return printCommand; }
        }
    }
}
