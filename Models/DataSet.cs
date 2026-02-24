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
        private float temperature;

        public string Sensor { get => sensor; }
        public DateTime Time { get => time; }
        public float Temperature { get => temperature; }

        public DataSet(string sensor, DateTime time, float temperature)
        {
            this.sensor = sensor;
            this.time = time;
            this.temperature = temperature;
        }
    }
}
