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
using System.Windows.Threading;

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
            bt_reloadTreeView.IsEnabled = false;
            bt_clearDataGrid.IsEnabled = false;
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
            reloadTreeView_image.Opacity = 0.2;
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

        private async void sqlSubmit(object sender, RoutedEventArgs e) {
            string database = MySQLHandle.database;
            ProzessManager pm = new ProzessManager("Sql Request",3);
            pm.addProgress();
            if(tb_sqlCommand.Text != "" && tb_sqlCommand.Text != null) {
                pm.addProgress();
                DataTable dt = MySQLHandle.SelectData(tb_sqlCommand.Text);
                if (dt != null) {
                    table_view_datagrid.ItemsSource = dt.DefaultView;
                    no_table.Visibility = Visibility.Collapsed;
                }
            }
            await Task.Run(() => { updateTreeView(); });
            pm.addProgress();
            pm.done();
        }

        private async void reloadTreeView(object sender, RoutedEventArgs e) {
            await Task.Run(() => { updateTreeView(); });
        }

        private void tb_sqlCommand_KeyDown(object sender, KeyEventArgs e) {
            if(e.Key== Key.Enter) {
                sqlSubmit(sender, e);
            }
        }

        private void bt_help_Click(object sender, RoutedEventArgs e) {
            System.Diagnostics.Process.Start("cmd", "/c " + "start https://www.google.com");
        }

        public static void updateTreeView() {
            Dispatcher dispatcher = Application.Current.Dispatcher;
            if (MySQLHandle.isConnected()) {
                TreeViewItem newDatabase;
                dispatcher.Invoke(() => {
                    ((MainWindow)System.Windows.Application.Current.MainWindow).bt_removeTable.IsEnabled = false;
                    ((MainWindow)System.Windows.Application.Current.MainWindow).bt_addTable.IsEnabled = false;
                    ((MainWindow)System.Windows.Application.Current.MainWindow).addTable_image.Opacity = 0.2;
                    ((MainWindow)System.Windows.Application.Current.MainWindow).removeTable_image.Opacity = 0.2;
                    ((MainWindow)Application.Current.MainWindow).no_tableList.Visibility = Visibility.Collapsed;
                    ((MainWindow)Application.Current.MainWindow).treeview.Items.Clear();
                    newDatabase = new TreeViewItem();
                    newDatabase.Header = "Neue Datenbank";
                    newDatabase.FontSize = 15;
                    newDatabase.Foreground = Brushes.White;
                    newDatabase.Selected += (sender, e) => { 
                        AddDatabase addDatabase = new AddDatabase();
                        addDatabase.ShowDialog();
                    };

                    ((MainWindow)Application.Current.MainWindow).treeview.Items.Add(newDatabase);
                });
                MySqlDataReader reader = MySQLHandle.GetData("show databases;");
                if (reader != null) {
                    while (reader.Read()) {
                        TreeViewItem item = null!;
                        dispatcher.Invoke(() => {
                            item = new TreeViewItem();
                            string header = reader.GetString(0);
                            item.Header = header;
                            item.FontSize = 15;
                            item.Foreground = Brushes.White;
                            item.Selected += (sender, e) => { 
                                MySQLHandle.setDatabase(header);
                                ((MainWindow)System.Windows.Application.Current.MainWindow).bt_removeTable.IsEnabled = true;
                                ((MainWindow)System.Windows.Application.Current.MainWindow).removeTable_image.Opacity = 1;
                            };
                        });
                        MySQLHandle.setDatabase(reader.GetString(0));
                        MySqlDataReader readerTable = MySQLHandle.GetData("show tables;");
                        if (readerTable != null) {
                            while (readerTable.Read()) {
                                TreeViewItem itemTable;
                                dispatcher.Invoke(() => {
                                    itemTable = new TreeViewItem();
                                    itemTable.Header = readerTable.GetString(0);
                                    itemTable.FontSize = 15;
                                    itemTable.Foreground = Brushes.White;
                                    string command = MySQLHandle.ip + "!?#-2" + reader.GetString(0) + "!?#-2" + readerTable.GetString(0);
                                    itemTable.Selected += (sender, e) => {
                                        Thread thread = new Thread(() => { MainViewManager.loadDataTable(command); });
                                        thread.Start();
                                    };
                                    item.Items.Add(itemTable);
                                });
                            }
                        }
                        dispatcher.Invoke(() => { ((MainWindow)Application.Current.MainWindow).treeview.Items.Add(item); });
                    }
                }
            }
            MySQLHandle.setDatabase(null);
        }

        private async void reloadTreeView_Click(object sender, RoutedEventArgs e) {
            ProzessManager pm = new ProzessManager("Load Databases",2);
            pm.addProgress();
            await Task.Run(() => { updateTreeView(); });
            pm.addProgress();
            pm.done();
        }

        private void addDatabase_Click(object sender, RoutedEventArgs e) {
            AddDatabase addDatabase = new AddDatabase();
            addDatabase.ShowDialog();
        }

        private void clearDataGrid_click(object sender, RoutedEventArgs e) {
            table_view_datagrid.ItemsSource = null;
            no_table.Visibility = Visibility.Visible;
        }

        private void removeDatabase_Click(object sender, RoutedEventArgs e) {
            RemoveDatabase removeDatabase = new RemoveDatabase();
            removeDatabase.ShowDialog();
        }

        private void addTable_Click(object sender, RoutedEventArgs e) {
            AddTable addTable = new AddTable();
            addTable.ShowDialog();
        }
    }
}
