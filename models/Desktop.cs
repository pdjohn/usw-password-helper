using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Documents;

namespace PasswordHelper
{
    public class Desktop 
    {
        public Int64 desktop_id;
        public string desktop_name;

        public Desktop(Int64 desktop_id, string desktop_name) { 
            this.desktop_id = desktop_id;
            this.desktop_name = desktop_name;
        }

        public static List<Desktop> GetDesktopData (ref SQLiteDataReader reader)
        {
            List<Desktop> list = new List<Desktop>();

            if(reader.StepCount > 0)
            {
                while (reader.Read())
                {
                    list.Add(new Desktop(
                        (Int64)reader["desktop_id"],
                        (string)reader["desktop_name"]
                      ));
                }
            }
            reader.Close();
            return list;
        }

    }
}