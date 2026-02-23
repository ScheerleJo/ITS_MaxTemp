using ITS_MaxTemp.Models;
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
            tempData = new TemperatureData(@"C:\Users\Patrick\Documents\Gruppenprojekt\ITS_MaxTemp\Data", "temps.csv");
            DataAccess.InitializeDatabase();

        }
    }
}
