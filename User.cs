using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;

namespace BobrovaKsiazka
{
    internal class User
    {
        private string id;
        private string login;
        private string password;
        private bool role;
        private string db_path = "user_table.db";
        private string cs = @"URI=file:" + Application.StartupPath + "\\user_table.db";
        SQLiteConnection con;
        SQLiteCommand cmd;
        SQLiteDataReader reader;
        public User(string login, string password, bool role)
        {
            this.login = login;
            this.password = password;
            this.role = role;
        }

        public string Login
        {
        get { return login; }
            set { login = value; }
        }

        public void data_show()
        {
            var con = new SQLiteConnection(cs);
        }
    }
}
