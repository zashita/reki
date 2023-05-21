using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace BobrovaKsiazka
{
    public partial class Registration : Form
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Form1 form1 = new Form1();
            form1.Show();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            UserDatabaseManager.CreateDatabase();

            // Путь к базе данных SQLite
            string databasePath = "users.db";

            // Параметры для подключения к базе данных SQLite
            string connectionString = $"Data Source={databasePath};Version=3;";
            SQLiteConnection connection = new SQLiteConnection(connectionString);

            // Открытие подключения к базе данных
            connection.Open();

            // Получение введенных пользователем данных
            string username = textBox1.Text;
            string password = textBox2.Text;

            // Создание SQL-запроса для добавления пользователя
            string query = "INSERT INTO Users (Username, Password) VALUES (@Username, @Password)";

            // Создание команды с параметрами
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", password);

            // Выполнение SQL-запроса
            command.ExecuteNonQuery();

            // Закрытие подключения к базе данных
            connection.Close();

            // Очистка полей ввода
            textBox1.Text = "";
            textBox2.Text = "";

            // Вывод сообщения об успешной регистрации
            MessageBox.Show("Пользователь успешно зарегистрирован.");


        }
    }
}
