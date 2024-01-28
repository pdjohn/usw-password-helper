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
    /// Interaction logic for Passwords.xaml
    /// </summary>
    public partial class Passwords : Page
    {

        private string _type = "";
        public Passwords(string type)
        {
            this._type = type;
            InitializeComponent();
            label.Content = this._type;
        }
    }
}
