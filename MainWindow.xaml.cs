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

namespace ITS_MaxTemp
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TemperatureData tempData;
        public MainWindow()
        {
            InitializeComponent();
            tempData = new TemperatureData(@".\Data", "temps.csv");
            DataAccess.InitializeDatabase();
        }

        private void EvaluateDataClick(object sender, RoutedEventArgs e)
        {
            string selectedSensor = (SensorComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            DateTime? fromDate = FromDatePicker.SelectedDate;
            DateTime? toDate = ToDatePicker.SelectedDate;

            Console.WriteLine($"Sensor: {selectedSensor}\nFrom: {fromDate}\nTo: {toDate}", "Evaluate Data");
            // Filterung fuer Josia von Sensor und Zeit, also bitte noch hier Funktionen implementieren mit DB
            // Vielleicht muessen beide Buttons die hier aufgelistet sind die gleiche Funktion aufrufen -> Also auswerten button muss halt gemacht werden (sql statement mit den werten aus den vars von oben)
        }


        private void MaxTemperatureClick(object sender, RoutedEventArgs e)
        {
            string selectedSensor = (SensorComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            DateTime? fromDate = FromDatePicker.SelectedDate;
            DateTime? toDate = ToDatePicker.SelectedDate;

            // Example for Josia -> SQL Befehl
            TemperatureTextBlock.Text = "XXXXX";
            DateTextBlock.Text = "XXXXX";
        }
    }
}
