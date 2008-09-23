using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessLayer
{

    public class Utils
    {
        public static bool isReduced(EntityLayer.TempMeasurenment tempMeasurenment)
        {
            if (tempMeasurenment.IsSampleReduced)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
