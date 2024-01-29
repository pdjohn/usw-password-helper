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
using System.Windows.Shapes;

namespace PasswordHelper
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        private PasswordManager _pm;
        private States _state;
        private PM_helper _pmh = null;
        public EditWindow(PasswordManager pm)
        {
            InitializeComponent();
            this._pm = pm;
            this._state = States.getInstance();
            this._pmh = new PM_helper();

            username.Text = pm.UserName;
            password.Text = this._pmh.DecryptPassword(this._pm.password, this._pm._master_key, this._pm.password.Length);
            this.VisibilityCondition();
        }

        private void VisibilityCondition()
        {
            websitePanel.Visibility = Visibility.Collapsed;
            gamePanel.Visibility = Visibility.Collapsed;
            desktopPanel.Visibility = Visibility.Collapsed;
            if(this._pm.Application == "website")
            {
                websitePanel.Visibility = Visibility.Visible;
            }
            else if(this._pm.Application == "game")
            {
                gamePanel.Visibility = Visibility.Visible;
            }else if(this._pm.Application == "desktop")
            {
                desktopPanel.Visibility= Visibility.Visible;
            }

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void UpdatePasswordEntry(object sender, RoutedEventArgs e)
        {
            string user_name = username.Text;
            string pass_word = password.Text;
            string website_name = websiteName.Text;
            string website_url = websiteUrl.Text;
            string game_name = gameName.Text;
            string game_author = gameAuthor.Text;
            string desktop_name = desktopName.Text;

            // global edit

            // before running the query encrypt the passwor
            string encrypted_password = this._pmh.EncryptPassword(pass_word, this._pm._master_key, pass_word.Length);
            this._state.db.cmd.CommandText = String.Format(@"
                UPDATE password_manager
                SET username = '{0}', password= '{1}' WHERE pm_id = {2} 
            ", user_name, encrypted_password, this._pm.pm_id);
            this._state.db.cmd.ExecuteNonQuery();

            if(this._pm.Application == "game")
            {
                // update password entry for game object.
            }else if(this._pm.Application == "desktop")
            {
                // update password entry for desktop object.
            }else if(this._pm.Application == "website")
            {
                // update password entry for website object.
            }
            this.Close();
        }
    }
}
