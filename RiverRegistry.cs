using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace BobrovaKsiazka
{
    public class RiverRegistry
    {
        private List<River> rivers;
        private string connectionString;

        public RiverRegistry(string databasePath)
        {
            rivers = new List<River>();
            connectionString = $"Data Source={databasePath};Version=3;";
        }

        public void AddRiver(River river)
        {
            rivers.Add(river);
        }

        public void RemoveRiver(River river)
        {
            rivers.Remove(river);
        }

        public River FindRiverByName(string name)
        {
            return rivers.Find(river => river.Name == name);
        }

        public List<River> GetRivers()
        {
            return rivers;
        }

        public void SaveRegistryToDatabase()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Создание таблицы, если она не существует
                string createTableQuery = "CREATE TABLE IF NOT EXISTS Rivers (Name TEXT, Location TEXT, Length REAL, FlowRate REAL, ProvidesWaterSupply INTEGER, UsedForIrrigation INTEGER, SupportsNavigation INTEGER, GeneratesHydroelectricPower INTEGER)";
                SQLiteCommand createTableCommand = new SQLiteCommand(createTableQuery, connection);
                createTableCommand.ExecuteNonQuery();

                // Удаление всех записей из таблицы
                string deleteAllQuery = "DELETE FROM Rivers";
                SQLiteCommand deleteAllCommand = new SQLiteCommand(deleteAllQuery, connection);
                deleteAllCommand.ExecuteNonQuery();

                // Внесение рек в базу данных
                string insertQuery = "INSERT INTO Rivers (Name, Location, Length, FlowRate, ProvidesWaterSupply, UsedForIrrigation, SupportsNavigation, GeneratesHydroelectricPower) VALUES (@Name, @Location, @Length, @FlowRate, @ProvidesWaterSupply, @UsedForIrrigation, @SupportsNavigation, @GeneratesHydroelectricPower)";
                foreach (River river in rivers)
                {
                    SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@Name", river.Name);
                    insertCommand.Parameters.AddWithValue("@Location", river.Location);
                    insertCommand.Parameters.AddWithValue("@Length", river.Length);
                    insertCommand.Parameters.AddWithValue("@FlowRate", river.FlowRate);
                    insertCommand.Parameters.AddWithValue("@ProvidesWaterSupply", river.ProvidesWaterSupply ? 1 : 0);
                    insertCommand.Parameters.AddWithValue("@UsedForIrrigation", river.UsedForIrrigation ? 1 : 0);
                    insertCommand.Parameters.AddWithValue("@SupportsNavigation", river.SupportsNavigation ? 1 : 0);
                    insertCommand.Parameters.AddWithValue("@GeneratesHydroelectricPower", river.GeneratesHydroelectricPower ? 1 : 0);

                    insertCommand.ExecuteNonQuery();


                }

                connection.Close();
            }
        }

        public void LoadRegistryFromDatabase()
        {
            rivers.Clear();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT Name, Location, Length, FlowRate, ProvidesWaterSupply, UsedForIrrigation, SupportsNavigation, GeneratesHydroelectricPower FROM Rivers";
                SQLiteCommand selectCommand = new SQLiteCommand(selectQuery, connection);
                SQLiteDataReader reader = selectCommand.ExecuteReader();

                while (
    reader.Read())
                {
                    string name = reader.GetString(0);
                    string location = reader.GetString(1);
                    double length = reader.GetDouble(2);
                    double flowRate = reader.GetDouble(3);
                    bool providesWaterSupply = reader.GetInt32(4) == 1;
                    bool usedForIrrigation = reader.GetInt32(5) == 1;
                    bool supportsNavigation = reader.GetInt32(6) == 1;
                    bool generatesHydroelectricPower = reader.GetInt32(7) == 1;
                    River river = new River(name, location, length, flowRate, providesWaterSupply, usedForIrrigation, supportsNavigation, generatesHydroelectricPower);
                    rivers.Add(river);
                }

                connection.Close();
            }
        }

        public void ClearRegistry()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Удаление всех записей из таблицы "Rivers"
                string deleteQuery = "DELETE FROM Rivers";

                using (SQLiteCommand command = new SQLiteCommand(deleteQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Сброс автоинкрементного значения столбца "Id"
                string resetQuery = "DELETE FROM sqlite_sequence WHERE name='Rivers'";
                using (SQLiteCommand command = new SQLiteCommand(resetQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
    }
}
