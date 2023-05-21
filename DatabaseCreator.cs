using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace BobrovaKsiazka
{
    public static class DatabaseCreator
    {
        public static void CreateDatabase()
        {
            string databasePath = "rivers.db";
            string connectionString = $"Data Source={databasePath};Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Создание таблицы "Rivers"
                string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Rivers (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Location TEXT NOT NULL,
                    Length REAL NOT NULL,
                    FlowRate REAL NOT NULL,
                    ProvidesWaterSupply INTEGER NOT NULL,
                    UsedForIrrigation INTEGER NOT NULL,
                    SupportsNavigation INTEGER NOT NULL,
                    GeneratesHydroelectricPower INTEGER NOT NULL
                );";

                using (SQLiteCommand command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }


    }
}
