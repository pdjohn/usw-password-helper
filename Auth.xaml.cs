using System;
using System.Collections.Generic;
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
            if(username.Text.Equals("admin") && password.Password.Equals("password"))
            {
                state.frame.Navigate(new Passwords());
            }
            else
            {
                MessageBox.Show("Username password not correct!");
            }
        }
    }
}
