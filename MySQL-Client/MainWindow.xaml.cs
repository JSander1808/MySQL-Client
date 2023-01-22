using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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

namespace MySQL_Client {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void table_view_datagrid_Loaded(object sender, RoutedEventArgs e) {
        }

        private void LogInButton_Click(object sender, RoutedEventArgs e) {
            Login loginFrame = new Login();
            loginFrame.ShowDialog();
        }

        private void LogOutButton_click(object sender, RoutedEventArgs e) {
            MySQLHandle.setConnectionString(null!, 0, null!, null!, null!);
            bt_login.IsEnabled = true;
            bt_logout.IsEnabled = false;
            bt_addDatabase.IsEnabled = false;
            bt_addTable.IsEnabled = false;
            bt_sql.IsEnabled = false;
            treeview.Items.Clear();
            TreeViewItem item = new TreeViewItem();
            item.Header = "Neue Datenbank";
            item.Foreground = Brushes.White;
            item.FontSize = 15;
            treeview.Items.Add(item);
            MainViewManager.updateAddressList("Server:      -");
            DataGridTextColumn column = new DataGridTextColumn();
            column.Header = "Debug";
            table_view_datagrid.ItemsSource = null;
        }

        private void newDatabase_click(object sender, MouseButtonEventArgs e) {

        }

        private static void loadDataTable(object sender, MouseButtonEventArgs e) {

        }
    }
}
