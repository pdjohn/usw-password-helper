using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PasswordHelper
{
    /// <summary>
    /// Interaction logic for Passwords.xaml
    /// </summary>
    public partial class Passwords : Page
    {

        private string _type = "";
        public Passwords(string type)
        {
            InitializeComponent();
            List<Users> UsersList = new List<Users> {
                new Users(1, "admin", "admin", "admin", "admin"),
                new Users(2, "admin2", "admin", "admin", "admin")
            };

            Trace.WriteLine("" + UsersList[0].user_role + " " + UsersList[0].user_name);

            this._type = type;
            userListBox.ItemsSource = UsersList;
        }
    }
}
