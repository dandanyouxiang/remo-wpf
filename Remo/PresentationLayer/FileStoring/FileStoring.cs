using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace PresentationLayer
{
    [Serializable]
    public class FileStoring
    {
        public string FileExtension { get; set; }
        public string FileDescription { get; set; }
        public string FileName { get; set; }
        public string WorkplacePath { get; set; }

        private DirectoryInfo workplace;


        public FileStoring(string fileExtension,string fileDescription,string fileName, string workplacePath)
        {
            this.FileExtension = fileExtension;
            this.FileDescription = fileDescription;
            this.FileName = fileName;
            this.WorkplacePath = workplacePath;
            //workplace = new DirectoryInfo(workplacePath);
        }


        public FileStoring()
        {
        }

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
            workplace = new DirectoryInfo(WorkplacePath);
            string retValue = "";
            FileInfo[] files=workplace.GetFiles("*.remo");

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
            workplace = new DirectoryInfo(WorkplacePath);
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
            string retValue = "Data_" + dateTime.Minute +"_"+ dateTime.Hour + "_" + dateTime.Day + "_" + dateTime.Month + "_" + dateTime.Year + "." + FileExtension;

            return retValue;
        }
        /*
        public bool isValidFileName(string fileName) 
        {
            Regex fileRegex = new Regex(@"[A-Z,a-z,0-9,_]*\.[A-Z,a-z,0-9]*");
            //throw new Exception(String.Format( "File with name \"{0} \" allredy is created! Choose another name.",fileName));

            //Дали постои фајл со такво име.
            if (workplace.GetFiles("fileName").Count() > 0) return false;
            //Дали добро внесен стрингот
            if (!fileRegex.Match(fileName).Success) return false;
            return true;
        } 
         */
    }
}
