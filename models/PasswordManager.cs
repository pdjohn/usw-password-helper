using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordHelper
{
    internal class PasswordManager
    {
        public Int64 pm_id;
        public int user_id;
        public string user_name;
        public string password;
        public string application;
        public int application_id;
        public DateTime created_at;
        public DateTime updated_at;

        public string UserName { get { return user_name; } }
        public string Password { get { return password; } }
        public string Application { get { return application; } }

        public PasswordManager(
            Int64 pm_id, int user_id, string user_name, string password, 
            string application, int application_id, DateTime created_at, DateTime updated_at
        )
        {
            this.pm_id = pm_id;
            this.user_id = user_id;
            this.user_name = user_name;
            this.password = password;
            this.application = application;
            this.application_id = application_id;
            this.created_at = created_at;
            this.updated_at = updated_at;
        }

        public static List<PasswordManager> GetPasswordManagreList(ref SQLiteDataReader reader)
        {
            List<PasswordManager> list = new List<PasswordManager>();

            if (reader.StepCount > 0)
            {
                //for (int i = 0; i < reader.StepCount; i++)
                while(reader.Read())
                {
                    //reader.Read();
                    list.Add(new PasswordManager(
                        (Int64)reader["pm_id"],
                        (int)reader["user_id"],
                        (string)reader["username"],
                        (string)reader["password"],
                        (string)reader["application"],
                        (int)reader["application_id"],
                        (DateTime)reader["created_at"],
                        (DateTime)reader["updated_at"]
                      ));
                }
            }
            reader.Close();
            return list;

        }

    }

}
