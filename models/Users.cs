using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Documents;

namespace PasswordHelper
{
    class Users
    {
        public Int64 user_id;
        public string user_name;
        public string user_role;
        public string password;
        private string? master_password;

        public string UserName { get { return user_name; } }
        public string UserRole { get { return user_role; } }

        public Users(Int64 user_id, string user_name, string password, string user_role, string? master_password) { 
            this.user_id = user_id;
            this.user_name = user_name;
            this.user_role = user_role;
            this.password = password;
            this.master_password = master_password;
        }

        public static List<Users> GetUserData (ref SQLiteDataReader reader)
        {
            List<Users> list = new List<Users>();

            if(reader.StepCount > 0)
            {
                for(int i = 0; i < reader.StepCount; i++)
                {
                    reader.Read();
                    list.Add(new Users(
                        (Int64)reader["user_id"],
                        (string)reader["user_name"],
                        (string)reader["password"],
                        (string)reader["user_role"],
                        (string)reader["master_password"]
                      ));
                }
            }
            reader.Close();
            return list;

        }

    }
}