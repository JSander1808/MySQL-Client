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
using System.Windows.Threading;

namespace MySQL_Client {
    /// <summary>
    /// Interaktionslogik für Error.xaml
    /// </summary>
    public partial class Error : Window {

        private Dispatcher uidispatcher = Application.Current.Dispatcher;
        public Error(string message) {
            InitializeComponent();
            tb_errorContent.Text = message;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
