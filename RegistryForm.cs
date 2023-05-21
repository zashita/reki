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
    public partial class RegistryForm : Form
    {
        private RiverRegistry riverRegistry;
        private bool isAdmin;
        private string username;
        public RegistryForm(bool isAdmin, string name)
        {
            InitializeComponent();
            DatabaseCreator.CreateDatabase();
            this.isAdmin = isAdmin;
            this.username = name;
            riverRegistry = new RiverRegistry("rivers.db");
            label1.Text = username;
            // Загрузка регистра рек из базы данных
            riverRegistry.LoadRegistryFromDatabase();

            // Отображение рек в DataGridView
            RefreshDataGridView();
            CheckEditPermissions();
        }

        private void CheckEditPermissions()
        {
            // Проверка, является ли пользователь администратором
            if (!isAdmin)
            {
                // Отключение редактирования DataGridView
                dataGridView1.ReadOnly = true;
                btnSave.Enabled = false;
                button1.Enabled = false;
                button2.Enabled = false;

                MessageBox.Show("Доступ к редактированию регистра рек разрешен только для администратора.");
            }
        }

        private void RefreshDataGridView()
        {
            dataGridView1.Columns.Clear(); // Очистка существующих столбцов

            // Добавление столбцов в DataGridView
            dataGridView1.Columns.Add("Name", "Название");
            dataGridView1.Columns.Add("Location", "Местоположение");
            dataGridView1.Columns.Add("Length", "Длина");
            dataGridView1.Columns.Add("FlowRate", "Расход воды");
            dataGridView1.Columns.Add("ProvidesWaterSupply", "Обеспечение водоснабжения");
            dataGridView1.Columns.Add("UsedForIrrigation", "Использование в орошении");
            dataGridView1.Columns.Add("SupportsNavigation", "Поддержка навигации");
            dataGridView1.Columns.Add("GeneratesHydroelectricPower", "Генерация гидроэлектроэнергии");

            dataGridView1.Rows.Clear(); // Очистка существующих строк

            // Получение данных из базы данных
            string databasePath = "rivers.db";
            string connectionString = $"Data Source={databasePath};Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Rivers";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Чтение значений из результата запроса
                            string name = reader.GetString(1);
                            string location = reader.GetString(2);
                            double length = reader.GetDouble(3);
                            double flowRate = reader.GetDouble(4);
                            int providesWaterSupply = reader.GetInt32(5);
                            int usedForIrrigation = reader.GetInt32(6);
                            int supportsNavigation = reader.GetInt32(7);
                            int generatesHydroelectricPower = reader.GetInt32(8);

                            // Добавление строки в DataGridView
                            dataGridView1.Rows.Add(name, location, length, flowRate, providesWaterSupply, usedForIrrigation, supportsNavigation, generatesHydroelectricPower);
                        }
                    }
                }

                connection.Close();
            }
        }

        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Проверка, что выбрана конкретная ячейка (а не заголовок столбца)
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Проверка, что ячейка содержит текстовое значение
                if (cell is DataGridViewTextBoxCell)
                {
                    dataGridView1.CurrentCell = cell;
                    dataGridView1.BeginEdit(false);
                }
            }
        }

        private void dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Обновление данных реки при завершении редактирования ячейки
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                River river = riverRegistry.GetRivers()[e.RowIndex];

                river.Name = row.Cells[0].Value.ToString();
                river.Location = row.Cells[1].Value.ToString();
                river.Length = Convert.ToDouble(row.Cells[2].Value);
                river.FlowRate = Convert.ToDouble(row.Cells[3].Value);
                river.ProvidesWaterSupply = Convert.ToBoolean(row.Cells[4].Value);
                river.UsedForIrrigation = Convert.ToBoolean(row.Cells[5].Value);
                river.SupportsNavigation = Convert.ToBoolean(row.Cells[6].Value);
                river.GeneratesHydroelectricPower = Convert.ToBoolean(row.Cells[7].Value);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string connectionString = $"Data Source=rivers.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Создание команды SQL для вставки данных
                string insertQuery = "INSERT INTO Rivers (Name, Location, Length, FlowRate, ProvidesWaterSupply, UsedForIrrigation, SupportsNavigation, GeneratesHydroelectricPower) VALUES (@Name, @Location, @Length, @FlowRate, @ProvidesWaterSupply, @UsedForIrrigation, @SupportsNavigation, @GeneratesHydroelectricPower)";

                using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                {
                    // Добавление параметров к команде
                    command.Parameters.AddWithValue("@Name", "");
                    command.Parameters.AddWithValue("@Location", "");
                    command.Parameters.AddWithValue("@Length", 0);
                    command.Parameters.AddWithValue("@FlowRate", 0);
                    command.Parameters.AddWithValue("@ProvidesWaterSupply", 0);
                    command.Parameters.AddWithValue("@UsedForIrrigation", 0);
                    command.Parameters.AddWithValue("@SupportsNavigation", 0);
                    command.Parameters.AddWithValue("@GeneratesHydroelectricPower", 0);

                    // Проход по каждой строке в DataGridView и вставка данных в базу данных
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        // Игнорирование пустых строк и строки новой записи
                        if (!row.IsNewRow && row.Cells[0].Value != null)
                        {
                            // Установка значений параметров команды на основе данных из DataGridView
                            command.Parameters["@Name"].Value = row.Cells[0].Value.ToString();
                            command.Parameters["@Location"].Value = row.Cells[1].Value.ToString();
                            command.Parameters["@Length"].Value = Convert.ToDouble(row.Cells[2].Value);
                            command.Parameters["@FlowRate"].Value = Convert.ToDouble(row.Cells[3].Value);
                            command.Parameters["@ProvidesWaterSupply"].Value = Convert.ToInt32(row.Cells[4].Value);
                            command.Parameters["@UsedForIrrigation"].Value = Convert.ToInt32(row.Cells[5].Value);
                            command.Parameters["@SupportsNavigation"].Value = Convert.ToInt32(row.Cells[6].Value);
                            command.Parameters["@GeneratesHydroelectricPower"].Value = Convert.ToInt32(row.Cells[7].Value);

                            // Выполнение команды SQL для вставки данных
                            command.ExecuteNonQuery();
                        }
                    }
                }

                connection.Close();
            }
            MessageBox.Show("Регистр рек сохранен в базу данных.");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataSeeder.SeedData();
            RefreshDataGridView();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            riverRegistry.ClearRegistry();
            RefreshDataGridView();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
