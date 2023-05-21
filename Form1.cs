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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
               this.Hide();
               Registration registration = new Registration();
            registration.Show();
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

            // Создание SQL-запроса для выборки пользователя по имени и паролю
            string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password";

            // Создание команды с параметрами
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", password);

            // Выполнение SQL-запроса
            object resultObj = command.ExecuteScalar();
            int result = Convert.ToInt32(resultObj);

            connection.Close();

            // Проверка результата запроса
            if (result != 0)
            {
                this.Hide();
                User user = new User(username, password, true);
                RegistryForm register = new RegistryForm(true, user.Login);
                register.Show();
            }
            else
            {
                // Если пользователь не найден, выведите сообщение об ошибке
                MessageBox.Show("Ошибка авторизации. Проверьте имя пользователя и пароль.");
            }
        }
    }
}
