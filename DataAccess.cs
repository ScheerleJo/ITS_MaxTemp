using System;
using System.Data.SQLite;
using System.IO;
using System.Diagnostics;

namespace ITS_MaxTemp
{
    internal class DataAccess
    {
        private static string connectionString = @"Data Source=.\maxTemp.db;Version=3;";

        public static void InitializeDatabase()
        { 
            if (!File.Exists(@".\maxTemp.db"))
            {
                SQLiteConnection.CreateFile(@".\maxTemp.db");
            }
            using (var db = new SQLiteConnection(connectionString))
            {
                db.Open();
                string tableCommand = @"CREATE TABLE IF NOT EXISTS tempData (
                    Primary_Key INTEGER PRIMARY KEY AUTOINCREMENT,
                    Sensor NVARCHAR(2) NOT NULL, 
                    Datetime DATETIME NOT NULL, 
                    Temperature DOUBLE NOT NULL
                );";

                using (var cmd = new SQLiteCommand(db))
                {
                    cmd.CommandText = tableCommand;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void AddData(string sensor, DateTime dateTime, double temperature)
        {
            using (var db = new SQLiteConnection(connectionString))
            {
                db.Open();

                using (SQLiteCommand cmd = new SQLiteCommand(db))
                { 
                    cmd.CommandText = @"INSERT INTO tempData (Sensor, Datetime, Temperature) VALUES (@Sensor, @Datetime, @Temperature);";
                    cmd.Parameters.AddWithValue("@Sensor", sensor);
                    cmd.Parameters.AddWithValue("@Datetime", dateTime);
                    cmd.Parameters.AddWithValue("@Temperature", temperature);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
            }
        }

        public static void getData()
        {
            using (var db = new SQLiteConnection(connectionString))
            {
                db.Open();
                string select = "SELECT Sensor, Datetime, Temperature FROM tempData";
                using (var cmd = new SQLiteCommand(select, db))
                {
                    using (SQLiteDataReader query = cmd.ExecuteReader())
                    {
                        while (query.Read())
                        {
                            Debug.WriteLine($"Sensor: {query.GetString(0)}, Datetime: {query.GetDateTime(1)}, Temperature: {query.GetDouble(2)}");
                        }
                    }
                }
            }
        }
    }
}
