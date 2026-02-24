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
            
            ((Button)sender).Focus();

            FromDatePicker.GetBindingExpression(System.Windows.Controls.DatePicker.SelectedDateProperty)?.UpdateSource();
            ToDatePicker.GetBindingExpression(System.Windows.Controls.DatePicker.SelectedDateProperty)?.UpdateSource();

            string selectedSensor = SensorComboBox.SelectedItem as string;

            
            if (string.IsNullOrEmpty(selectedSensor))
            {
                MessageBox.Show("Please select a sensor first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            
            DateTime? fromDate = FromDatePicker.SelectedDate;
            DateTime? toDate = ToDatePicker.SelectedDate;

           
            if (string.IsNullOrWhiteSpace(FromDatePicker.Text))
            {
                fromDate = null;
                FromDatePicker.SelectedDate = null;
            }
            if (string.IsNullOrWhiteSpace(ToDatePicker.Text))
            {
                toDate = null;
                ToDatePicker.SelectedDate = null;
            }

            Console.WriteLine($"Sensor: {selectedSensor}\nFrom: {fromDate}\nTo: {toDate}");
            float? sensorValue;
            DateTime? dateValue;

            if (fromDate.HasValue && toDate.HasValue)
            {
                if (fromDate.Value > toDate.Value)
                {
                    MessageBox.Show("'From' date must be before 'To' date.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                DateTime toDateEndOfDay = toDate.Value.Date.AddDays(1).AddSeconds(-1);
                (sensorValue, dateValue) = DataAccess.getMaxSensorValue(selectedSensor, fromDate.Value, toDateEndOfDay);
            }
            else if (fromDate.HasValue || toDate.HasValue)
            {
                MessageBox.Show("Please set both 'From' and 'To' dates, or leave both empty for all data.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                (sensorValue, dateValue) = DataAccess.getMaxSensorValue(selectedSensor);
            }

            if (sensorValue.HasValue && dateValue.HasValue)
            {
                TemperatureTextBlock.Text = $"{sensorValue.Value:F1} °C";
                DateTextBlock.Text = $"gemessen am {dateValue.Value:dd.MM.yyyy HH:mm}";
            }
            else
            {
                TemperatureTextBlock.Text = "Keine Daten";
                DateTextBlock.Text = "Keine Daten für den gewählten Zeitraum gefunden";
            }

            
            TemperatureTextBlock.UpdateLayout();
            DateTextBlock.UpdateLayout();
        }

        private void ClearFilter_Click(object sender, RoutedEventArgs e)
        {
            FromDatePicker.SelectedDate = null;
            ToDatePicker.SelectedDate = null;
            FromDatePicker.Text = "";
            ToDatePicker.Text = "";
        }

        private void AddSensorsToComboBox()
        {
            SensorComboBox.Items.Clear(); // clear items, to add actual sensors
            SensorComboBox.ItemsSource = DataAccess.getSensorNames();
            SensorComboBox.SelectedIndex = 0;
        }
    }
}
