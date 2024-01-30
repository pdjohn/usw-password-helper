using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PasswordHelper
{
    public class PasswordManager : INotifyPropertyChanged
    {
        public Int64 pm_id;
        public int user_id;
        public string user_name;
        public string password;
        public string application;
        public int application_id;
        public DateTime created_at;
        public DateTime updated_at;
        public string _master_key;
        private bool _isPasswordVisible;
        public Game game = null;
        public Website website = null;
        public Desktop desktop = null;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string UserName { get { return user_name; } }
        public string Password
        {
            get {
                string decrypted_password = new PM_helper().DecryptPassword(password, this._master_key, password.Length);
                return _isPasswordVisible ? decrypted_password : "********"; 
            }
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public string Application { get { return application; } }

        public PasswordManager(
            Int64 pm_id, int user_id, string user_name, string password,
            string application, int application_id, DateTime created_at, 
            DateTime updated_at, string master_key
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
            this._master_key = master_key;

        }

        public static List<PasswordManager> GetPasswordManagreList(ref SQLiteDataReader reader)
        {
            List<PasswordManager> list = new List<PasswordManager>();

            if (reader.StepCount > 0)
            {
                //for (int i = 0; i < reader.StepCount; i++)
                while (reader.Read())
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
                        (DateTime)reader["updated_at"],
                        (string)reader["master_password"]
                      ));
                }
            }
            reader.Close();
            return list;

        }

        public bool IsPasswordVisible
        {
            get { return _isPasswordVisible; }
            set
            {
                _isPasswordVisible = value;
                OnPropertyChanged(nameof(IsPasswordVisible));
                OnPropertyChanged(nameof(Password));
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


