using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ITS_MaxTemp.Models;
using Microsoft.Win32;

namespace ITS_MaxTemp
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TemperatureData tempData;

        string path = "";
        public MainWindow()
        {
            InitializeComponent();
            DataAccess.InitializeDatabase();

            //Add sensor names to ComboBox, if Data is already available
            AddSensorsToComboBox();
        }

        private void setFilePath() {
            if (path == "")
            {
                tempData = new TemperatureData(@".\Data", "temps.csv");
                return;
            }
            FilePath.Text = path;
            tempData = new TemperatureData(path);
        }

        private void UploadFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog(){
                Filter = "CSV files (*.csv)|*.csv"
            };

            if(openFileDialog.ShowDialog() == true)
            {
                path = openFileDialog.FileName;
            }
            setFilePath();

            //Add Data to SQLite DB
            DataAccess.clearTable(); // clear table -> no duplicates
            DataAccess.AddData(tempData);

            AddSensorsToComboBox();
        }

        private void EvaluateDataClick(object sender, RoutedEventArgs e)
        {
            if(DataAccess.checkIfTableEmpty())
            {
                MessageBox.Show("No data available. Please upload a CSV file first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string selectedSensor = SensorComboBox.SelectedItem as string;
            DateTime? fromDate = FromDatePicker.SelectedDate;
            DateTime? toDate = ToDatePicker.SelectedDate;

            Console.WriteLine($"Sensor: {selectedSensor}\nFrom: {fromDate}\nTo: {toDate}", "Evaluate Data");
            float? sensorValue;
            DateTime? dateValue;

            if (fromDate.HasValue && toDate.HasValue)
            {
                (sensorValue, dateValue) = DataAccess.getMaxSensorValue(selectedSensor, fromDate.Value, toDate.Value);
            }
            else
            {
                (sensorValue, dateValue) = DataAccess.getMaxSensorValue(selectedSensor);
            }

            TemperatureTextBlock.Text = $"{sensorValue} °C";
            DateTextBlock.Text = dateValue.ToString();
        }

        private void AddSensorsToComboBox()
        { 
            if (DataAccess.getSensorNames() != null)
            {
                SensorComboBox.Items.Clear(); // clear items, to add actual sensors
                SensorComboBox.ItemsSource = DataAccess.getSensorNames();
                SensorComboBox.SelectedIndex = 0;
                FilePath.Text = "Daten wurden aus der Datenbank geladen.";
            } 
        }
    }
}
