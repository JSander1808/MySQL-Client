using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MySQL_Client {
    /// <summary>
    /// Interaktionslogik für Login.xaml
    /// </summary>
    public partial class Login : Window {

        private static string ip = null!;
        private static uint port = 0;
        private static string user = null!;
        private static string password = null!;
        public Login() {
            InitializeComponent();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void login_click(object sender, RoutedEventArgs e) {
            ip = tb_ip.Text;
            port = Convert.ToUInt32(int.Parse(tb_port.Text));
            user = tb_user.Text;
            password = pwb_pw.Password.ToString();
            Thread t = new Thread(() => { login(); });
            t.Start();
        }

        private void login() {
            this.Dispatcher.Invoke(() => { pb_login.Visibility = Visibility.Visible; });
            MySQLHandle.setConnectionString(ip, port, user, password, null!);
            this.Dispatcher.Invoke(() => { pb_login.Value = 1; });
            if (!MySQLHandle.isConnected()) {
                this.Dispatcher.Invoke(() => {
                    lb_error.Content = "Die Anmeldedaten sind nicht korrekt.";
                    pb_login.Value = 0;
                    pb_login.Visibility = Visibility.Collapsed;
                });
                return;
            }
            this.Dispatcher.Invoke(() => {
                pb_login.Value = 2;
                MainViewManager.updateAddressList("Server: " + tb_ip.Text);
            });
            MySqlDataReader reader = MySQLHandle.GetData("show databases;");
            this.Dispatcher.Invoke(() => { pb_login.Value = 3; });
            if (reader != null) {
                while (reader.Read()) {
                    for (int i = 0; i < reader.FieldCount; i++) {
                        TreeViewItem item = null!;
                        this.Dispatcher.Invoke(() => {
                            item = new TreeViewItem();
                            item.Header = reader.GetString(i);
                            item.FontSize = 15;
                            item.Foreground = Brushes.White;
                        });
                        MySqlDataReader reader1 = MySQLHandle.GetData("show tables from " + reader.GetString(i) + ";");
                        if (reader1 != null) {
                            while (reader1.Read()) {
                                for (int j = 0; j < reader1.FieldCount; j++) {
                                    TreeViewItem item1 = null!;
                                    this.Dispatcher.Invoke(() => {
                                        item1 = new TreeViewItem();
                                        item1.Header = reader1.GetString(j);
                                        item1.FontSize = 15;
                                        item1.Foreground = Brushes.White;
                                        string command = ip+ "!?#-2" + reader.GetString(i)+ "!?#-2" + reader1.GetString(j);
                                        item1.MouseDoubleClick += (sender,e) => { MainViewManager.loadDataTable(command); };
                                        item.Items.Add(item1);
                                    });
                                }
                            }
                        }
                        this.Dispatcher.Invoke(() => { ((MainWindow)System.Windows.Application.Current.MainWindow).treeview.Items.Add(item); });
                    }
                }
            }
            this.Dispatcher.Invoke(() => {
                pb_login.Value = 4;
                ((MainWindow)System.Windows.Application.Current.MainWindow).bt_login.IsEnabled = false;
                ((MainWindow)System.Windows.Application.Current.MainWindow).bt_logout.IsEnabled = true;
                ((MainWindow)System.Windows.Application.Current.MainWindow).bt_addDatabase.IsEnabled = true;
                ((MainWindow)System.Windows.Application.Current.MainWindow).bt_addTable.IsEnabled = true;
                ((MainWindow)System.Windows.Application.Current.MainWindow).bt_sql.IsEnabled = true;
                pb_login.Value = 5;
                Close();
                pb_login.Visibility = Visibility.Collapsed;
                pb_login.Value = 0;
            });
        }

        private void Item1_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            throw new NotImplementedException();
        }

        private void cancel_click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
