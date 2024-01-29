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
                command.CommandText = @"
                    create table users(
                        user_id integer primary key autoincrement not null,
                        user_role varchar(10),
                        user_name varchar(255),
                        first_name varchar(255),
                        last_name varchar(255),
                        email varchar(255),
                        password text,
                        master_password text,
                        created_at timestamp default CURRENT_TIMESTAMP,
                        updated_at timestamp default CURRENT_TIMESTAMP
                    );
                ";
                var result = command.ExecuteNonQuery();
                var hash = BC.HashPassword("password");
                PM_helper pm = new PM_helper(); // Password Manager Helper
                command.CommandText = String.Format(@"
                    insert into users(user_role,user_name,email,password, master_password) 
                    values ('admin', 'admin', 'admin@email.com', '{0}', '{1}')",
                    hash, pm.GenerateMasterKey()
                );
                command.ExecuteNonQuery();
            }

            if (!checkIfExist("website"))
            {
                command.CommandText = @"
                    create table website(
                        website_id integer primary key autoincrement not null,
                        website_name varchar(255),
                        website_url varchar(255)
                    );
                ";
                command.ExecuteNonQuery();
            }

            if (!checkIfExist("desktop"))
            {
                command.CommandText = @"
                    create table desktop(
                        desktop_id integer primary key autoincrement not null,
                        desktop_name varchar(255)
                    );
                ";
                command.ExecuteNonQuery();
            }
            if (!checkIfExist("game"))
            {
                command.CommandText = @"
                    create table game(
                        game_id integer primary key autoincrement not null,
                        game_name varchar(255),
                        game_developer varchar(255)
                    );
                ";
                command.ExecuteNonQuery();
            }
            if (!checkIfExist("password_manager")){
                command.CommandText = @"
                    create table password_manager(
                        pm_id integer primary key autoincrement not null,
                        user_id int not null,
                        username varchar(255),
                        password varchar(255),
                        application varchar(20),
                        application_id int,
                        created_at timestamp default CURRENT_TIMESTAMP,
                        updated_at timestamp default CURRENT_TIMESTAMP,
                        foreign key (user_id) references users(user_id)
                    );
                ";
                command.ExecuteNonQuery();
            }



        }

        ~Database() {
            connection.Close();
        }
    }
}
