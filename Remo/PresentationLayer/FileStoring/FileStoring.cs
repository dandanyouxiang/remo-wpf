using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PresentationLayer
{
    class FileStoring
    {
        private DirectoryInfo workplace;

        public FileStoring(string workplacePath) 
        {
            workplace = new DirectoryInfo(workplacePath);
        }
        /// <summary>
        /// Метода да го врати првиот фајл во директориумот.
        /// </summary>
        /// <returns></returns>
        public string getFirstFile() 
        {
            string retValue = "";
            FileInfo[] files=workplace.GetFiles();

            if (files.Length > 0) 
            {
                retValue = files.First().Name;
            }

            return retValue;
        }
        /// <summary>
        /// Дали посто фајл со такво име.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool existFile(string fileName)
        {
            bool retValue = false;

            FileInfo[] files = workplace.GetFiles(fileName);

            if (files.Length > 0)
            {
                retValue = true;
            }

            return retValue;
        }
        public string sugestFileName(DateTime dateTime) 
        {
            //Ако нема со име дата да предлози
            string retValue = "Data_"+dateTime.Hour+"_"+dateTime.TimeOfDay+"_"+dateTime.Day+"."+dateTime.Month+"."+dateTime.Year;

            return retValue;
        }
    }
}
