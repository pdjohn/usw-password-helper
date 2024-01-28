using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Data.Common;

using BC = BCrypt.Net.BCrypt;
using System.Windows;

namespace PasswordHelper
{
    internal class Database
    {
        SQLiteConnection connection;
        SQLiteCommand command;
        string dbPath = System.Environment.CurrentDirectory + "\\DB";
        string dbFilePath;


        public Database() {
            CreateDbFile();
            CreateDatabaseConnection();
        }

        public SQLiteCommand cmd { get { return command; } }
        public void CreateDatabaseConnection()
        {
            string strCon = string.Format("Data Source={0};", dbFilePath);
            connection = new SQLiteConnection(strCon);
            connection.Open();
            command = connection.CreateCommand();
        }

        public void CloseDbConnection()
        {
            this.connection.Close();
        }

        public bool checkIfExist(string tableName)
        {
            command.CommandText = "SELECT name FROM sqlite_master WHERE name='" + tableName + "'";
            var result = command.ExecuteScalar();
            return result != null && result.ToString() == tableName ? true : false;
        }

        public void CreateDbFile()
        {
            if (!string.IsNullOrEmpty(dbPath) && !Directory.Exists(dbPath))
                Directory.CreateDirectory(dbPath);
            dbFilePath = dbPath + "\\passwordb.sqlite3";
            if (!System.IO.File.Exists(dbFilePath))
            {
                SQLiteConnection.CreateFile(dbFilePath);
            }

        }

        public void MigrateDatabase()
        {
            if (!checkIfExist("users"))
            {
                command.CommandText = "create table users(user_id integer primary key autoincrement not null, user_type text, user_name text, email text, password text, master_key text);";
                var result = command.ExecuteNonQuery();
                var hash = BC.HashPassword("password");
                command.CommandText = String.Format("insert into users(user_type,user_name,email,password,master_key) values ('admin', 'admin', 'admin@email.com', '{0}', 'master-key')", hash);
                command.ExecuteNonQuery();
                


                
            }
        }

        ~Database() {
            connection.Close();
        }
    }
}
