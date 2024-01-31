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
            if(type == "admin")
            {
                addBtn.Visibility = Visibility.Collapsed; 
            }
            this._state = States.getInstance();
            userListBox.ItemsSource = GetData();
        }

        private List<PasswordManager> GetData()
        {
            if(this._type == "admin")
            {
                this._state.db.cmd.CommandText = @"
                    select pm.*, users.master_password, g.game_name, d.desktop_name, w.website_name  from password_manager pm 
                    left join users on users.user_id = pm.user_id
                    left join game g on g.game_id = pm.application_id and pm.application = 'game'
                    left join desktop d on d.desktop_id = pm.application_id and pm.application = 'desktop'
                    left join website w on w.website_id = pm.application_id and pm.application = 'website';
                ";
            }
            else
            {
                this._state.db.cmd.CommandText = @"
                    select pm.*, users.master_password, g.game_name, d.desktop_name, w.website_name  from password_manager pm 
                    left join users on users.user_id = pm.user_id
                    left join game g on g.game_id = pm.application_id and pm.application = 'game'
                    left join desktop d on d.desktop_id = pm.application_id and pm.application = 'desktop'
                    left join website w on w.website_id = pm.application_id and pm.application = 'website'
                    where pm.user_id=@user_id;
                ";
                this._state.db.cmd.Parameters.AddWithValue("@user_id", this._state.user.user_id);
            }
            SQLiteDataReader reader = this._state.db.cmd.ExecuteReader();
            List<PasswordManager> list = PasswordManager.GetPasswordManagreList(ref reader);
            return list;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int index = userListBox.Items.IndexOf(button.DataContext);
            PasswordManager selectedEntry = (PasswordManager)userListBox.Items[index];
            Trace.WriteLine(selectedEntry);

            FetchApplicationInfo(ref selectedEntry);

            if (selectedEntry != null)
            {
                EditWindow editUserWindow = new EditWindow(selectedEntry);
                editUserWindow.ShowDialog();
                userListBox.ItemsSource = GetData();
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int index = userListBox.Items.IndexOf(button.DataContext);
            PasswordManager selectedEntry = (PasswordManager)userListBox.Items[index];
            Trace.WriteLine(selectedEntry);

            FetchApplicationInfo(ref selectedEntry);

            if (selectedEntry != null)
            {
                this._state.db.cmd.CommandText = String.Format(@"delete from password_manager where pm_id={0}", selectedEntry.pm_id);
                this._state.db.cmd.ExecuteNonQuery();
                userListBox.ItemsSource = GetData();
            }
        }

        public void FetchApplicationInfo(ref PasswordManager pm)
        {
            if(pm.Application == "game")
            {
                this._state.db.cmd.CommandText = String.Format(@"
                    select * from game  where game.game_id={0};
                ", pm.application_id);
                SQLiteDataReader reader = this._state.db.cmd.ExecuteReader();
                List<Game> list = Game.GetGameData(ref reader);
                if(list.Count > 0)
                {
                    pm.game = list[0];
                }
            }else if(pm.Application == "website")
            {
                this._state.db.cmd.CommandText = String.Format(@"
                    select * from website w where w.website_id={0};
                ", pm.application_id);
                SQLiteDataReader reader = this._state.db.cmd.ExecuteReader();
                List<Website> list = Website.GetWebsiteData(ref reader);
                if(list.Count > 0)
                {
                    pm.website = list[0];
                }

            }else if(pm.Application == "desktop")
            {
                this._state.db.cmd.CommandText = String.Format(@"
                    select * from desktop d where d.desktop_id={0};
                ", pm.application_id);
                SQLiteDataReader reader = this._state.db.cmd.ExecuteReader();
                List<Desktop> list = Desktop.GetDesktopData(ref reader);
                if(list.Count > 0)
                {
                    pm.desktop = list[0];
                }


                
            }
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            this._state.Clear();
            this._state.frame.Navigate(new Auth());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddWindow addWindow = new AddWindow();
            addWindow.ShowDialog();
            userListBox.ItemsSource = GetData();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Search Button
            userListBox.ItemsSource = GetSearchData();
        }

        private List<PasswordManager> GetSearchData()
        {
            string search_text = searchBox.Text;
            if (this._type == "admin")
            {
                this._state.db.cmd.CommandText = @"
                    select pm.*, users.master_password, g.game_name, d.desktop_name, w.website_name  from password_manager pm 
                    left join users on users.user_id = pm.user_id
                    left join game g on g.game_id = pm.application_id and pm.application = 'game'
                    left join desktop d on d.desktop_id = pm.application_id and pm.application = 'desktop'
                    left join website w on w.website_id = pm.application_id and pm.application = 'website'
                    where g.game_name like '%' || @search_text || '%' or d.desktop_name like '%' || @search_text || '%' or w.website_name like '%' || @search_text || '%' 
                ";
                this._state.db.cmd.Parameters.AddWithValue("@search_text", search_text);
            }
            else
            {
                this._state.db.cmd.CommandText = @"
                    select pm.*, users.master_password, g.game_name, d.desktop_name, w.website_name  from password_manager pm 
                    left join users on users.user_id = pm.user_id
                    left join game g on g.game_id = pm.application_id and pm.application = 'game'
                    left join desktop d on d.desktop_id = pm.application_id and pm.application = 'desktop'
                    left join website w on w.website_id = pm.application_id and pm.application = 'website'
                    where g.game_name like '%' || @search_text || '%' or d.desktop_name like '%' || @search_text || '%' or w.website_name like '%' || @search_text || '%' 
                    and pm.user_id=@user_id;
                ";
                this._state.db.cmd.Parameters.AddWithValue("@search_text", search_text);
                this._state.db.cmd.Parameters.AddWithValue("@user_id", this._state.user.user_id);
            }
            SQLiteDataReader reader = this._state.db.cmd.ExecuteReader();
            List<PasswordManager> list = PasswordManager.GetPasswordManagreList(ref reader);
            return list;
        }
    }
}
