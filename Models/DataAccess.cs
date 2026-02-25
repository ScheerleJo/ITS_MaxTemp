using ITS_MaxTemp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Windows;
using DataSet = ITS_MaxTemp.Models.DataSet;

namespace ITS_MaxTemp
{
    internal class DataAccess
    {
        private const string DatabaseFile = @".\maxTemp.db";
        private static readonly string connectionString = $"Data Source={DatabaseFile};Version=3;Pooling=True;Max Pool Size=100;";

        private static void executeQueryVoid(string query)
        {
            using (var db = new SQLiteConnection(connectionString))
            {
                db.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(query, db))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
        
        public static void InitializeDatabase()
        {
            if (!File.Exists(DatabaseFile)) SQLiteConnection.CreateFile(DatabaseFile);

            using (var db = new SQLiteConnection(connectionString))
            {
                db.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(db))
                {
                    cmd.CommandText = @"CREATE TABLE IF NOT EXISTS tempData (
                        Primary_Key INTEGER PRIMARY KEY AUTOINCREMENT,
                        Sensor NVARCHAR(2) NOT NULL, 
                        Time DATETIME NOT NULL, 
                        Temperature DOUBLE NOT NULL
                    );";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "PRAGMA journal_mode=WAL;";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "PRAGMA synchronous=NORMAL;";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static bool AddData(TemperatureData tempData)
        {
            try
            {
                using (var db = new SQLiteConnection(connectionString))
                {
                    db.Open();
                    using (var transaction = db.BeginTransaction())
                    {
                        try
                        {
                            using (SQLiteCommand cmd = new SQLiteCommand(
                                @"INSERT INTO tempData (Sensor, Time, Temperature) VALUES (@Sensor, @Time, @Temperature);", db, transaction
                                ))
                            {
                                var sensorParam = cmd.Parameters.Add("@Sensor", DbType.String);
                                var timeParam = cmd.Parameters.Add("@Time", DbType.DateTime);
                                var tempParam = cmd.Parameters.Add("@Temperature", DbType.Double);

                                foreach (DataSet dataSet in tempData.DataSets )
                                {
                                    sensorParam.Value = dataSet.Sensor;
                                    timeParam.Value = dataSet.Time;
                                    tempParam.Value = dataSet.Temperature;

                                    cmd.ExecuteNonQuery();
                                }
                            }
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
                Console.WriteLine("Import Completed");
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error importing to Database: {e.Message}");
                return false;
            }
            return true;
        }



        public static void clearTable()
        {
            executeQueryVoid("DELETE FROM tempData;");
        }

        public static bool checkIfTableEmpty()
        {
            using (var db = new SQLiteConnection(connectionString))
            {
                db.Open();
                string select = "SELECT COUNT(*) FROM tempData";
                using (var cmd = new SQLiteCommand(select, db))
                {
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count == 0) return true;
                }
            }
            return false;
        }

        public static List<String> getSensorNames()
        {
            List<String> sensorNames = new List<string>();
            if (checkIfTableEmpty()) return null;
            using (var db = new SQLiteConnection(connectionString))
            {
                db.Open();
                string select = @"SELECT DISTINCT Sensor FROM tempData;";
                using (var cmd = new SQLiteCommand(select, db))
                {
                    using (SQLiteDataReader query = cmd.ExecuteReader())
                    {
                        while (query.Read())
                        {
                            string sensorName = query.GetString(0);
                            sensorNames.Add(sensorName);
                        }
                    }
                }
            }
            return sensorNames;
        }
      

        public static (float? sensorValue, DateTime? dateValue) getMaxSensorValue(string sensor)
        {
            using (var db = new SQLiteConnection(connectionString))
            {
                db.Open();
                string select = @"SELECT Temperature, Time FROM tempData
                    WHERE Sensor = @sensor
                    ORDER BY Temperature DESC
                    LIMIT 1;";
                using (var cmd = new SQLiteCommand(select, db))
                {
                    cmd.Parameters.AddWithValue("@sensor", sensor);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return (reader.GetFloat(0), reader.GetDateTime(1));
                        }
                    }
                }
            }
            return (null, null);
        }

        public static (float? sensorValue, DateTime? dateValue) getMaxSensorValue(string sensor, DateTime dateFrom, DateTime dateTo)
        {
            using (var db = new SQLiteConnection(connectionString))
            {
                db.Open();
                string select = @"SELECT Temperature, Time FROM tempData
                    WHERE Sensor = @sensor
                    AND Time BETWEEN @dateFrom AND @dateTo
                    ORDER BY Temperature DESC
                    LIMIT 1;";
                using (var cmd = new SQLiteCommand(select, db))
                {
                    cmd.Parameters.AddWithValue("@sensor", sensor);
                    cmd.Parameters.AddWithValue("@dateFrom", dateFrom);
                    cmd.Parameters.AddWithValue("@dateTo", dateTo);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return (reader.GetFloat(0), reader.GetDateTime(1));
                        }
                    }
                }
            }
            return (null, null);
        }
    }
}
