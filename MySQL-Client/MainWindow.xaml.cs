using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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
            initialize();
        }

        public async void initialize() {
            await Task.Run(() => { Thread.Sleep(1000); });
            Login loginFrame = new Login();
            loginFrame.ShowDialog();
        }

        private void table_view_datagrid_Loaded(object sender, RoutedEventArgs e) {
        }

        private void LogInButton_Click(object sender, RoutedEventArgs e) {
            Login loginFrame = new Login();
            loginFrame.ShowDialog();
        }

        private void LogOutButton_click(object sender, RoutedEventArgs e) {
            ProzessManager pm = new ProzessManager("Logout",4);
            pm.addProgress();
            MySQLHandle.setConnectionString(null!, 0, null!, null!, null!);
            bt_login.IsEnabled = true;
            bt_logout.IsEnabled = false;
            bt_addDatabase.IsEnabled = false;
            bt_addTable.IsEnabled = false;
            bt_sql.IsEnabled = false;
            bt_removeDatabase.IsEnabled = false;
            bt_removeTable.IsEnabled = false;
            treeview.Items.Clear();
            pm.addProgress();
            MainViewManager.updateAddressList("Server:      -");
            DataGridTextColumn column = new DataGridTextColumn();
            column.Header = "Debug";
            pm.addProgress();
            table_view_datagrid.ItemsSource = null;
            no_table.Visibility = Visibility.Visible;
            no_status.Visibility = Visibility.Visible;
            no_propeties.Visibility = Visibility.Visible;
            no_tableList.Visibility = Visibility.Visible;
            addDatabase_image.Opacity = 0.2;
            addTable_image.Opacity = 0.2;
            openSql_image.Opacity = 0.2;
            logout_image.Opacity = 0.2;
            login_image.Opacity = 1;
            removeDatabase_image.Opacity = 0.2;
            removeTable_image.Opacity = 0.2;
            table_view.Margin = new Thickness(310, 114, 300, 33);
            sql_commander.Visibility = Visibility.Collapsed;
            pm.addProgress();
            pm.done();
        }

        private void newDatabase_click(object sender, MouseButtonEventArgs e) {

        }

        private static void loadDataTable(object sender, MouseButtonEventArgs e) {

        }

        private void sql_click(object sender, RoutedEventArgs e) {
            if(sql_commander.Visibility == Visibility.Collapsed) {
                table_view.Margin = new Thickness(310, 340, 300, 33);
                sql_commander.Visibility = Visibility.Visible;
                tb_sqlCommand.Clear();
            } else {
                table_view.Margin = new Thickness(310, 114, 300, 33);
                sql_commander.Visibility = Visibility.Collapsed;
            }
        }

        private void sqlSubmit(object sender, RoutedEventArgs e) {
            ProzessManager pm = new ProzessManager("Sql Request",0);
            pm.addProgress();
            if(tb_sqlCommand.Text != "" && tb_sqlCommand.Text != null) {
                pm.addProgress();
                DataTable dt = MySQLHandle.SelectData(tb_sqlCommand.Text);
                if (dt != null) {
                    table_view_datagrid.ItemsSource = dt.DefaultView;
                    no_table.Visibility = Visibility.Collapsed;
                }
            }
            MainViewManager.updateTreeView();
            pm.addProgress();
            pm.done();
        }

        private void reloadTreeView(object sender, RoutedEventArgs e) {
            Thread thread = new Thread(() => { MainViewManager.updateTreeView(); });
            thread.Start();
        }
    }
}
