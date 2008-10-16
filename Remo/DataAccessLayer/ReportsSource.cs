﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessLayer
{
    public partial class DataSource
    {
        /// <summary>
        /// R1AtStdTemp
        /// </summary>
        /// <returns></returns>
        public double retR1AtStdTemp(int selectedChannel)
        {
            double r1TempCoeff = Root.TransformerProperties.HvTempCoefficient;
            double value = retR1Cold(selectedChannel) * (StdTemp + r1TempCoeff) / (TCold + r1TempCoeff);
            return value == 0 ? double.NaN : value;
        }
        /// <summary>
        /// R2AtStdTemp
        /// </summary>
        /// <returns></returns>
        public double retR2AtStdTemp(int selectedChannel)
        {
            double r2TempCoeff = Root.TransformerProperties.LvTempCoefficient;
            double value = retR2Cold(selectedChannel) * (StdTemp + r2TempCoeff) / (TCold + r2TempCoeff);
            return value == 0 ? double.NaN : value;
        }
        /// <summary>
        /// R1Phase
        /// </summary>
        /// <returns></returns>
        public double retR1Phase(int selectedChannel)
        {
            double val1 = 0.5;
            if (Root.TransformerProperties.HV == EntityLayer.TransformerProperties.ConnectionType.D)
                val1 = 1.5;
            double value = retR1AtStdTemp(selectedChannel) * val1;
            return value == 0 ? double.NaN : value;
        }
        /// <summary>
        /// R2Phase
        /// </summary>
        /// <returns></returns>
        public double retR2Phase(int selectedChannel)
        {
            double val1 = 0.5;
            if (Root.TransformerProperties.LV == EntityLayer.TransformerProperties.ConnectionType.D)
                val1 = 1.5;
            double value = retR2AtStdTemp(selectedChannel) * val1;
            return value == 0 ? double.NaN : value;
        }
        /// <summary>
        /// retR1Cold за даден канал
        /// </summary>
        /// <param name="selectedChannel"></param>
        /// <returns></returns>
        public double retR1Cold(int selectedChannel)
        {
            return evalR1Cold(selectedChannel);
        }

        public double retR2Cold(int selectedChannel)
        {
            return evalR2Cold(selectedChannel);
        }

        public double retTCold()
        {
            return evalTCold();
        }
        //cooling
        /// <summary>
        /// End AC Temp
        /// </summary>
        /// <returns></returns>
        public double retEndAcTemp()
        {
            return evalEndAcTemp(); 
        }
        /// <summary>
        /// R1 Cold Resistance
        /// </summary>
        /// <returns></returns>
        public double retR1ColdAtDcCool()
        {
            return evalR1ColdAtDcCool();
        }
        /// <summary>
        /// R2 Cold Resistance
        /// </summary>
        /// <returns></returns>
        public double retR2ColdAtDcCool()
        {
            return evalR2ColdAtDcCool();
        }
        /// <summary>
        /// R1 at time t=0.
        /// </summary>
        /// <returns></returns>
        public double retT1T0()
        {
            return evalT1T0();
        }
        /// <summary>
        /// Temp Rise at time t=0.
        /// </summary>
        /// <returns></returns>
        public double retT1Rise()
        {
            return evalT1Rise();
        }
        /// <summary>
        /// Exponential Curve f(t).
        /// </summary>
        /// <returns></returns>
        public double retFT1()
        {
            return evalFT2();
        }
        /// <summary>
        /// R2 at time t=0.
        /// </summary>
        /// <returns></returns>
        public double retT2T0()
        {
            return evalT2T0();
        }
        /// <summary>
        /// Temp Rise at time t=0.
        /// </summary>
        /// <returns></returns>
        public double retT2Rise()
        {
            return evalT2Rise();
        }
        /// <summary>
        /// Exponential Curve f(t).
        /// </summary>
        /// <returns></returns>
        public double retFT2()
        {
            return evalFT2();
        }
    }
}