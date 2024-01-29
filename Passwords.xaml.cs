using PasswordHelper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
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
        private States _state;
        public Passwords(string type)
        {
            InitializeComponent();
            this._type = type;
            this._state = States.getInstance();
            userListBox.ItemsSource = GetData();
        }

        private List<PasswordManager> GetData()
        {
            if(this._type == "admin")
            {
                this._state.db.cmd.CommandText = @"
                    select pm.*, users.master_password from password_manager pm 
                    left join users on users.user_id = pm.user_id";
            }
            else
            {
                this._state.db.cmd.CommandText = String.Format(@"
                    select pm.*, users.master_password from password_manager pm 
                    left join users on users.user_id = pm.user_id
                    where pm.user_id = {0};
                    ", this._state.user.user_id);
            }
            SQLiteDataReader reader = this._state.db.cmd.ExecuteReader();
            List<PasswordManager> list = PasswordManager.GetPasswordManagreList(ref reader);
            return list;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int index = userListBox.Items.IndexOf(button.DataContext);
            PasswordManager selectedUser = (PasswordManager)userListBox.Items[index];
            Trace.WriteLine(selectedUser);

            if (selectedUser != null)
            {
                EditWindow editUserWindow = new EditWindow(selectedUser);
                editUserWindow.ShowDialog();
                userListBox.ItemsSource = GetData();
            }
        }
    }
}
