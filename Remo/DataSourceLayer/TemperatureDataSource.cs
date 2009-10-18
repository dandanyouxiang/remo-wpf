using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlcProcess;
using System.Threading;

namespace DataSourceLayer
{
   
    public class TemperatureDataSource
    {
        public delegate void MeasurenmentDoneEvent(double t1, double t2, double t3, double t4);
        public delegate void MeasurenmentEndEvent();
        /// <summary>
        /// Крај на мерењата
        /// </summary>
        public event MeasurenmentDoneEvent MeasurenmentDone;
        /// <summary>
        /// Завршено е едно мерење
        /// </summary>
        public event MeasurenmentEndEvent MeasurenmentEnd;

        private double sampleRate;
        private int numberOfSamples;
        private int numberOfTempsRead;
        private ProcessManager pm;
        private ReadMemIntTask temp1;
        private ReadMemIntTask temp2;
        private ReadMemIntTask temp3;
        private ReadMemIntTask temp4;
        private double t1;
        private double t2;
        private double t3;
        private double t4;
        private DateTime time;
        private int samplesMeasured;
        public TemperatureDataSource(ProcessManager pm, double sampleRate, int numberOfSamples)
        {
            this.pm = pm;
            this.sampleRate = sampleRate;
            this.numberOfSamples = numberOfSamples;
            numberOfTempsRead = 0;
            temp1 = new ReadMemIntTask("read temp 1 task", 15);
            temp1.TaskExecutedEvent += new PlcRWTask.TaskExecutedEventHandler(tempAll_TaskExecutedEvent);
            temp2 = new ReadMemIntTask("read temp 2 task", 16);
            temp2.TaskExecutedEvent += new PlcRWTask.TaskExecutedEventHandler(tempAll_TaskExecutedEvent);
            temp3 = new ReadMemIntTask("read temp 3 task", 17);
            temp3.TaskExecutedEvent += new PlcRWTask.TaskExecutedEventHandler(tempAll_TaskExecutedEvent);
            temp4 = new ReadMemIntTask("read temp 4 task", 18);
            temp4.TaskExecutedEvent += new PlcRWTask.TaskExecutedEventHandler(tempAll_TaskExecutedEvent);
            pm.addPlcTask(temp1);
            pm.addPlcTask(temp2);
            pm.addPlcTask(temp3);
            pm.addPlcTask(temp4);
            samplesMeasured = 0;
        }
        public void startTempMeasurenments()
        {
            pm.start();
            time = DateTime.Now;
        }
        public void stopTempMeasurenments()
        {
            pm.stop();
            OnMesurenmentEnd();
        }

        public void tempAll_TaskExecutedEvent(object sender, EventArgs e)
        {
           switch(((ReadMemIntTask)sender).TaskName)
           {
               case "read temp 1 task": t1 = ((ReadMemIntTask)sender).Value; break;
               case "read temp 2 task": t2 = ((ReadMemIntTask)sender).Value; break;
               case "read temp 3 task": t3 = ((ReadMemIntTask)sender).Value; break;
               case "read temp 4 task": t4 = ((ReadMemIntTask)sender).Value; break;
           }
            numberOfTempsRead++;
            //Измерени се сите 4 канали
            if (numberOfTempsRead == 4 )
            {
                if ((t1 != 0) && (t2 != 0) && (t3 != 0) && (t4 != 0))
                {
                    OnMeasurenmentDone(t1, t2, t3, t4);
                    DateTime now = DateTime.Now;
                    TimeSpan elapsed = now - time;
                    if (elapsed.TotalSeconds <= sampleRate)
                        System.Threading.Thread.Sleep((int)(sampleRate - elapsed.TotalSeconds) * 1000);
                    time = DateTime.Now;
                    numberOfTempsRead = 0;
                    samplesMeasured++;

                    //Крај на мерењата
                    if (samplesMeasured == numberOfSamples)
                    {
                        OnMesurenmentEnd();
                        pm.stop();
                    }
                    else
                    {
                        pm.addPlcTask(temp1);
                        pm.addPlcTask(temp2);
                        pm.addPlcTask(temp3);
                        pm.addPlcTask(temp4);
                    }
                }
                else
                    numberOfTempsRead = 0;
            }
        }
        /// <summary>
        /// Завршено е едно мерење
        /// </summary>
        private void OnMeasurenmentDone(double t1, double t2, double t3, double t4)
        {
            if (MeasurenmentDone != null)
                MeasurenmentDone(t1 / 10, t2 / 10, t3 / 10, t4 / 10);
        }
        /// <summary>
        /// Крај на сите мерења
        /// </summary>
        private void OnMesurenmentEnd()
        {
            if (MeasurenmentEnd != null)
                MeasurenmentEnd();
        }
    }
}
