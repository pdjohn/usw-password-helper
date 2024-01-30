using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Documents;

namespace PasswordHelper
{
    public class Website 
    {
        public Int64 website_id;
        public string website_name;
        public string website_url;

        public Website(Int64 website_id, string website_name, string website_url) { 
            this.website_id = website_id;
            this.website_name = website_name;
            this.website_url = website_url;
        }

        public static List<Website> GetWebsiteData (ref SQLiteDataReader reader)
        {
            List<Website> list = new List<Website>();

            if(reader.StepCount > 0)
            {
                while (reader.Read())
                {
                    list.Add(new Website(
                        (Int64)reader["website_id"],
                        (string)reader["website_name"],
                        (string)reader["website_url"]
                      ));
                }
            }
            reader.Close();
            return list;
        }

    }
}