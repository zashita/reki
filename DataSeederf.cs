using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

public static class DataSeeder
{
    public static void SeedData()
    {
        string databasePath = "rivers.db";
        string connectionString = $"Data Source={databasePath};Version=3;";

        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            // Вставка данных рек в таблицу "Rivers"
            string insertQuery = @"
                INSERT INTO Rivers (Name, Location, Length, FlowRate, ProvidesWaterSupply, UsedForIrrigation, SupportsNavigation, GeneratesHydroelectricPower)
                VALUES
                    ('River 1', 'Location 1', 100.5, 50.2, 1, 0, 1, 1),
                    ('River 2', 'Location 2', 75.3, 40.1, 0, 1, 1, 0),
                    ('River 3', 'Location 3', 120.7, 80.6, 1, 1, 0, 1),
                    ('River 4', 'Location 4', 90.2, 30.8, 0, 0, 1, 0),
                    ('River 5', 'Location 5', 65.4, 45.7, 1, 1, 1, 0),
                    ('River 6', 'Location 6', 110.9, 55.3, 0, 1, 0, 1),
                    ('River 7', 'Location 7', 85.6, 70.2, 1, 0, 0, 0),
                    ('River 8', 'Location 8', 70.3, 25.9, 0, 0, 1, 0),
                    ('River 9', 'Location 9', 95.8, 60.4, 1, 1, 0, 1),
                    ('River 10', 'Location 10', 130.6, 90.1, 1, 1, 1, 1);";

            using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
}
