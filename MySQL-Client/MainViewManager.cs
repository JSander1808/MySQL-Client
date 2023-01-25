using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace MySQL_Client {
    internal class MainViewManager{

        private static  Dispatcher uidispatcher = Application.Current.Dispatcher;

        public MainViewManager() {

        }

        public static void updateTreeView() {
            uidispatcher.BeginInvoke(() => {
                if (MySQLHandle.isConnected()) {
                    ((MainWindow)Application.Current.MainWindow).treeview.Items.Clear();
                    TreeViewItem newDatabase = new TreeViewItem();
                    newDatabase.Header = "Neue Datenbank";
                    newDatabase.FontSize = 15;
                    newDatabase.Foreground = Brushes.White;
                    ((MainWindow)Application.Current.MainWindow).treeview.Items.Add(newDatabase);
                    MySqlDataReader reader = MySQLHandle.GetData("show databases;");
                    if (reader != null) {
                        while (reader.Read()) {
                            TreeViewItem item = null!;
                            item = new TreeViewItem();
                            string header = reader.GetString(0);
                            item.Header = header;
                            item.FontSize = 15;
                            item.Foreground = Brushes.White;
                            item.Selected += (sender, e) => { MySQLHandle.setDatabase(header); };
                            MySQLHandle.setDatabase(reader.GetString(0));
                            MySqlDataReader readerTable = MySQLHandle.GetData("show tables;");
                            if (readerTable != null) {
                                while (readerTable.Read()) {
                                    TreeViewItem itemTable;
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
                                }
                            }
                            ((MainWindow)Application.Current.MainWindow).treeview.Items.Add(item);
                        }
                    }
                }
                MySQLHandle.setDatabase(null);
            });
        }

        public static void loadDataTable(string command) {
            ProzessManager pm = new ProzessManager("Load Table", 4);
            pm.addProgress();
            string[] tagArgs = command.Split("!?#-2");
            MySQLHandle.setDatabase(tagArgs[1]);
            pm.addProgress();
            DataTable dt = MySQLHandle.SelectData("select * from `" + tagArgs[2] + "`;");
            pm.addProgress();
            if (dt != null) uidispatcher.BeginInvoke(() => { ((MainWindow)System.Windows.Application.Current.MainWindow).table_view_datagrid.ItemsSource = dt.DefaultView; });
            pm.addProgress();
            updateAddressList("Server: " + tagArgs[0] + " >> Datenbank: " + tagArgs[1] + " >> Tabelle: " + tagArgs[2]);
            uidispatcher.BeginInvoke(() => { ((MainWindow)System.Windows.Application.Current.MainWindow).no_table.Visibility = Visibility.Collapsed; });
            pm.addProgress();
            pm.done();
        }

        public static void updateAddressList(string command) {
            uidispatcher.BeginInvoke(() => { ((MainWindow)System.Windows.Application.Current.MainWindow).addresslist.Content = command; });
        }

        public static void setErrorMessage(string message) {
            Error error = new Error(message);
            error.ShowDialog();
        }
    }
}
