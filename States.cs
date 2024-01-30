using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PasswordHelper
{
    internal class States
    {
        public static States _instance;
        private Frame _frame = null;
        private Database _database = null;
        private Users _user = null;
   
        public Frame frame { get { return _frame; } set { _frame = value; } }
        public Database db { get { return _database; } set { _database = value; } }
        public Users user { get { return _user; } set { _user = value; } }
        public static States getInstance()
        {
            if(_instance == null)
            {
                _instance = new States();
            }
            return _instance;
        }

        public void Clear()
        {
            this._user = null;
        }
    }
}
