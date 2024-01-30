using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Documents;

namespace PasswordHelper
{
     public class Game 
    {
        public Int64 game_id;
        public string game_name;
        public string game_author;

        public Game(Int64 game_id, string game_name, string game_author) { 
            this.game_id = game_id;
            this.game_name = game_name;
            this.game_author = game_author;
        }

        public static List<Game> GetGameData (ref SQLiteDataReader reader)
        {
            List<Game> list = new List<Game>();

            if(reader.StepCount > 0)
            {
                while(reader.Read())
                {
                    list.Add(new Game(
                        (Int64)reader["game_id"],
                        (string)reader["game_name"],
                        (string)reader["game_developer"]
                      ));
                }
            }
            reader.Close();
            return list;
        }

    }
}