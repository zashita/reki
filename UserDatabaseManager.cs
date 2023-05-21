using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace BobrovaKsiazka
{
    static class UserDatabaseManager
    {
        static string databasePath = "users.db";

        public static void CreateDatabase()
        {
            if (!DatabaseExists())
            {
                SQLiteConnection.CreateFile(databasePath);
                CreateTable();
            }
        }

        private static bool DatabaseExists()
        {
            return File.Exists(databasePath);
        }

        private static void CreateTable()
        {
            using (SQLiteConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                connection.Open();
                string query = "CREATE TABLE Users (Id INTEGER PRIMARY KEY AUTOINCREMENT, Username TEXT NOT NULL, Password TEXT NOT NULL)";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        private static string GetConnectionString()
        {
            return $"Data Source={databasePath};Version=3;";
        }
    }
}
