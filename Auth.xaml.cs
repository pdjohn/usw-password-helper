using System;
using System.Collections.Generic;
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

using BC = BCrypt.Net.BCrypt;

namespace PasswordHelper
{
    /// <summary>
    /// Interaction logic for Auth.xaml
    /// </summary>
    public partial class Auth : Page
    {
        States state = null; 
        public Auth()
        {
            InitializeComponent();
            state = States.getInstance();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void LgoinButton(object sender, RoutedEventArgs e)
        {
            state.db.cmd.CommandText = "select * from users where user_name='" + username.Text.ToString() + "'";
            SQLiteDataReader reader = state.db.cmd.ExecuteReader();
            List<Users> users = Users.GetUserData(ref reader);

            // test
            if(users.Count > 0)
            {
                var user = users[0];
                // do stronog password comparison and storing here.
                if(!BC.Verify(password.Password.ToString(), user.password))
                {
                    MessageBox.Show("Password is invalid!");
                    return;
                }
                if(user.user_role == "admin")
                {
                    // go to admin page.
                    state.frame.Navigate(new Passwords("admin"));
                }
                else
                {
                    // go to some other page.
                    state.frame.Navigate(new Passwords("users"));
                }

            }
            else
            {
                MessageBox.Show("No User found!");
                return;
            }
        }

        private void RegisterButton(object sender, RoutedEventArgs e)
        {
            string user_name = username.Text.ToString();
            string passwd = password.Password.ToString();

            state.db.cmd.CommandText = String.Format("select count(user_id) from users where user_name='{0}'", user_name);
            var result = Convert.ToInt32(state.db.cmd.ExecuteScalar());
            if(result > 0)
            {
                MessageBox.Show("username unavailable");
                return;
            }
            else
            {
                var hash = BC.HashPassword(passwd);
                PM_helper pm = new PM_helper();
                state.db.cmd.CommandText = String.Format("insert into users(user_role, user_name, password, master_password) values ('user', '{0}', '{1}', '{2}')", user_name, hash, pm.GenerateMasterKey());
                state.db.cmd.ExecuteNonQuery();
                MessageBox.Show("user registered successfully! please login.");
            }

        }
    }
}
