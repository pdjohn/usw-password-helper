using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Documents;

namespace PasswordHelper
{
    class Users
    {
        public Int64 user_id;
        public string username;
        public string user_type;
        public string password;
        private string master_key;

        public Users(Int64 user_id, string username, string password, string user_type) { 
            this.user_id = user_id;
            this.username = username; 
            this.password = password;
            this.user_type = user_type;
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
                        (Int64)reader["user_id"], (string)reader["user_name"], (string)reader["password"], (string)reader["user_type"]
                      ));
                }
            }
            reader.Close();
            return list;

        }

    }
}