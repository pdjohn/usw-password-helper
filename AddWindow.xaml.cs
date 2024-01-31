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
using System.Windows.Shapes;

namespace PasswordHelper
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        private PM_helper _pmh;
        private States _state;
        public AddWindow()
        {
            InitializeComponent();
            this._pmh = new PM_helper();
            this._state = States.getInstance();

            agamePanel.Visibility = Visibility.Visible;
            adesktopPanel.Visibility = Visibility.Collapsed;
            awebsitePanel.Visibility = Visibility.Collapsed;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (agamePanel == null || adesktopPanel == null || awebsitePanel == null) return;
            switch (comboBox.SelectedIndex)
            {
                case 0:
                    agamePanel.Visibility = Visibility.Visible;
                    adesktopPanel.Visibility = Visibility.Collapsed;
                    awebsitePanel.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    agamePanel.Visibility = Visibility.Collapsed;
                    adesktopPanel.Visibility = Visibility.Visible;
                    awebsitePanel.Visibility = Visibility.Collapsed;

                    break;
                case 2:
                    agamePanel.Visibility = Visibility.Collapsed;
                    adesktopPanel.Visibility = Visibility.Collapsed;
                    awebsitePanel.Visibility = Visibility.Visible;

                    break;
                default:
                    MessageBox.Show("Must have to select an item from the dropdown.");
                    break;
            }
        }

        private void CreatePasswordEntry(object sender, RoutedEventArgs e)
        {
            string application = "";
            Int64 application_id = 0;
           

            if (comboBox.SelectedIndex == 0)
            {
                this._state.db.cmd.CommandText = String.Format(@"
                    insert into game(game_name,game_developer)
                    values('{0}', '{1}');
                ", gameName.Text, gameAuthor.Text);
                this._state.db.cmd.ExecuteNonQuery();
                application = "game";
                this._state.db.cmd.CommandText = "select last_insert_rowid()";
                application_id = (Int64)this._state.db.cmd.ExecuteScalar();
            }
            else if (comboBox.SelectedIndex == 1)
            {
                this._state.db.cmd.CommandText = String.Format(@"
                    insert into desktop(desktop_name)
                    values('{0}');
                ", desktopName.Text);
                this._state.db.cmd.ExecuteNonQuery();
                application = "desktop";
                this._state.db.cmd.CommandText = "select last_insert_rowid()";
                application_id = (Int64)this._state.db.cmd.ExecuteScalar();

            }
            else if (comboBox.SelectedIndex == 2)
            {
                this._state.db.cmd.CommandText = String.Format(@"
                    insert into website(website_name,website_url) 
                    values('{0}','{1}');
                ", websiteName.Text, websiteUrl.Text);
                this._state.db.cmd.ExecuteNonQuery();
                application = "website";
                this._state.db.cmd.CommandText = "select last_insert_rowid()";
                application_id = (Int64)this._state.db.cmd.ExecuteScalar();
            }

            if(application_id == 0)
            {
                MessageBox.Show("Something went wrong");
                return;
            }

            string encrypted_password = this._pmh.EncryptPassword(password.Text, this._state.user.MasterPassword, password.Text.Length);
            this._state.db.cmd.CommandText = String.Format(@"
                insert into password_manager (username,password,user_id,application,application_id)
                values('{0}','{1}',{2},'{3}',{4}) 
            ", username.Text, encrypted_password, this._state.user.user_id, application, application_id);
            this._state.db.cmd.ExecuteNonQuery();

            this.Close();
        }
    }
}
