using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls.Primitives;

namespace ITS_MaxTemp
{
    internal class DataAccess
    {

        public async static void InitializeDatabase()
        {

            string cwd = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            Properties.Settings.Default["LocalPath"] = cwd;
            Properties.Settings.Default["dbPath"] = Path.Combine(cwd, "maxTemp.db");
            Properties.Settings.Default.Save();

            using (var db = new SqliteConnection($"Filename={Properties.Settings.Default["dbPath"]}"))
            { 
                db.Open();

                string tableCommand = "CREATE TABLE IF NOT " +
                    "EXISTS ITS_MaxTempDB (Primary_Key INTEGER PRIMARY KEY, " +
                    "Sensor NVARCHAR(2) NOT NULL, " +
                    "Datetime DATETIME NOT NULL, " +
                    "Temperature DOUBLE NOT NULL)";

                var createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();
            }
        }

        public static void AddData(string sensor, DateTime dateTime, double temperature)
        {
            using (var db = new SqliteConnection($"Filename={Properties.Settings.Default["dbPath"]}"))
            {
                db.Open();

                var insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO ITS_MaxTempDB (Primary_Key, Sensor, Datetime, Temperature) VALUES (NULL, @Sensor, @Datetime, @Temperature);";
                insertCommand.Parameters.AddWithValue("@Sensor", sensor);
                insertCommand.Parameters.AddWithValue("@Datetime", dateTime);
                insertCommand.Parameters.AddWithValue("@Temperature", temperature);
                insertCommand.ExecuteReader();
            }
        }

        public static void GetData(string sensor = null)
        {
            //var entries = new List<string>();
            using (var db = new SqliteConnection($"Filename={Properties.Settings.Default["dbPath"]}"))
            {
                db.Open();
                string select = "SELECT Sensor, Datetime, Temperature FROM ITS_MaxTempDB";
                if (sensor != null) select = "SELECT Sensor, Datetime, Temperature FROM ITS_MaxTempDB WHERE Sensor = " + sensor;

                var selectCommand = new SqliteCommand(select, db);
                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    //entries.Add(query.GetString(0));
                }
            }
            //return entries;
        }
    }
}
