using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS_MaxTemp.Models
{
    internal class DataSet
    {
        private string sensor;
        private DateTime time;
        private double temperature;

        public string Sensor { get => sensor; }
        public DateTime Time { get => time; }
        public double Temperature { get => temperature; }

        public DataSet(string sensor, DateTime time, double temperature)
        {
            this.sensor = sensor;
            this.time = time;
            this.temperature = temperature;
        }
    }
}
