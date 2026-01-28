using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Collections.Specialized;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel;
using System.Data.Common;
using System.IO.Pipelines;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Xml;
using Microsoft.VisualBasic.FileIO;
using System.Reflection.Metadata.Ecma335;
using System.Diagnostics;

namespace ITS_MaxTemp.Model
{
    public class TemperatureData
    {
        private string filePath;
        private string fileName;
        private List<string> rawData;

        public List<string> RawData { get {return rawData;}}

        public TemperatureData(string filePath, string fileName)
        {
            this.filePath = filePath;
            this.fileName = fileName;
        }

        private void ReadFile()
        {
            try
            {
                using (StreamReader sr = new StreamReader(filePath + fileName))
                {
                    string line; 
                    while((line = sr.ReadLine()) != null)
                    {
                        rawData.Add(line);
                    }
                }
            } catch(Exception e)
            {
                Console.WriteLine("Error reading Data: ");
                Console.WriteLine(e.Message);
            }
        }
    }
}