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
            
            //TODO: Reagieren auf einen anderen Dateipfad
            tempData = new TemperatureData(@".\Data", "temps.csv");
            FilePath.Text = "temps.csv";
            
            DataAccess.InitializeDatabase();
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
        private void EvaluateDataClick(object sender, RoutedEventArgs e)
        {
            string selectedSensor = (SensorComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            DateTime? fromDate = FromDatePicker.SelectedDate;
            DateTime? toDate = ToDatePicker.SelectedDate;

            Console.WriteLine($"Sensor: {selectedSensor}\nFrom: {fromDate}\nTo: {toDate}", "Evaluate Data");
            // Filterung fuer Josia von Sensor und Zeit, also bitte noch hier Funktionen implementieren mit DB
            // -> Also auswerten button muss halt gemacht werden (sql statement mit den werten aus den vars von oben)

            TemperatureTextBlock.Text = "XXXXX";
            DateTextBlock.Text = "XXXXX";
        }

    }
}
