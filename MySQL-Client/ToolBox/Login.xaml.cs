using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
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
        private bool isLoged = false;
        public Login() {
            InitializeComponent();
            updateSavePanel();
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

        private async void login() {
            this.Dispatcher.Invoke(() => { 
                bt_cancel.IsEnabled = false; 
                bt_loginSubmit.IsEnabled = false;
            });
            ProzessManager pm = new ProzessManager("Login", 5);
            MySQLHandle.setConnectionString(ip, port, user, password, null!);
            pm.addProgress();
            if (!MySQLHandle.isConnected()) {
                this.Dispatcher.Invoke(() => {
                    lb_error.Content = "Die Anmeldedaten sind nicht korrekt.";
                    pm.done();
                    this.Dispatcher.Invoke(() => { 
                        bt_cancel.IsEnabled = true;
                        bt_loginSubmit.IsEnabled = true;
                    });
                });
                return;
            }
            pm.addProgress();
            await Task.Run(() => { MainWindow.updateTreeView(); });
            pm.addProgress();
            this.Dispatcher.Invoke(() => {
                ((MainWindow)System.Windows.Application.Current.MainWindow).bt_login.IsEnabled = false;
                ((MainWindow)System.Windows.Application.Current.MainWindow).bt_logout.IsEnabled = true;
                ((MainWindow)System.Windows.Application.Current.MainWindow).bt_addDatabase.IsEnabled = true;
                ((MainWindow)System.Windows.Application.Current.MainWindow).bt_addTable.IsEnabled = true;
                ((MainWindow)System.Windows.Application.Current.MainWindow).bt_sql.IsEnabled = true;
                ((MainWindow)System.Windows.Application.Current.MainWindow).bt_removeDatabase.IsEnabled = true;
                ((MainWindow)System.Windows.Application.Current.MainWindow).bt_removeTable.IsEnabled = true;
                ((MainWindow)System.Windows.Application.Current.MainWindow).bt_reloadTreeView.IsEnabled = true;
                ((MainWindow)System.Windows.Application.Current.MainWindow).bt_clearDataGrid.IsEnabled = true;
                ((MainWindow)System.Windows.Application.Current.MainWindow).addTable_image.Opacity = 1;
                ((MainWindow)System.Windows.Application.Current.MainWindow).addDatabase_image.Opacity = 1;
                ((MainWindow)System.Windows.Application.Current.MainWindow).openSql_image.Opacity = 1;
                ((MainWindow)System.Windows.Application.Current.MainWindow).login_image.Opacity = 0.2;
                ((MainWindow)System.Windows.Application.Current.MainWindow).logout_image.Opacity = 1;
                ((MainWindow)System.Windows.Application.Current.MainWindow).reloadTreeView_image.Opacity = 1;
                ((MainWindow)System.Windows.Application.Current.MainWindow).removeDatabase_image.Opacity = 1;
                ((MainWindow)System.Windows.Application.Current.MainWindow).removeTable_image.Opacity = 1;
                ((MainWindow)System.Windows.Application.Current.MainWindow).no_tableList.Visibility = Visibility.Collapsed;
                Close();
            });
            pm.addProgress();
            pm.done();
            this.Dispatcher.Invoke(() => { 
                bt_cancel.IsEnabled = true; 
                bt_loginSubmit.IsEnabled = true;
            });
        }

        private void cancel_click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void save_loginData_click(object sender, RoutedEventArgs e) {
            ProzessManager pm = new ProzessManager("Save Logindata", 4);
            pm.addProgress();
            if (tb_saveName.Text != "" && tb_saveName != null) {
                pm.addProgress();
                ConfigManager config = new ConfigManager(Directory.GetCurrentDirectory() + "\\Data\\Login\\Data.conf");
                config.init();
                pm.addProgress();
                string seperator = "2#g6b";
                string seperator2 = "82#/hr";
                string dataString = "ip" + seperator + tb_ip.Text + seperator2 + "port" + seperator + tb_port.Text + seperator2 + "user" + seperator + tb_user.Text + seperator2 + "password" + seperator + pwb_pw.Password.ToString();
                config.set(tb_saveName.Text, dataString);
                pm.addProgress();
                pm.done();
                updateSavePanel();
            } else {
                lb_error.Content = "Die Speicherung benötigt einen Namen!";
                pm.done();
                updateSavePanel();
            }
        }

        private void load_loginData_click(object sender, RoutedEventArgs e) {
            ProzessManager pm = new ProzessManager("Load Logindata", 5);
            pm.addProgress();
            if (lb_saveDataPanel.SelectedItem != null) {
                pm.addProgress();
                ConfigManager config = new ConfigManager(Directory.GetCurrentDirectory() + "\\Data\\Login\\Data.conf");
                pm.addProgress();
                string seperator = "2#g6b";
                string seperator2 = "82#/hr";
                string loginkey = config.get(lb_saveDataPanel.SelectedItem.ToString());
                pm.addProgress();
                string[] bigdata = loginkey.Split(seperator2);
                tb_ip.Text = bigdata[0].Split(seperator)[1];
                tb_port.Text = bigdata[1].Split(seperator)[1];
                tb_user.Text = bigdata[2].Split(seperator)[1];
                pwb_pw.Password = bigdata[3].Split(seperator)[1];
                ip = tb_ip.Text;
                port = Convert.ToUInt32(int.Parse(tb_port.Text));
                user = tb_user.Text;
                password = pwb_pw.Password.ToString();
                Thread thread = new Thread(() => { login(); });
                thread.Start();
                pm.addProgress();
                pm.done();
            } else {
                pm.done();
                lb_error.Content = "Kein Speicherstand zum laden ausgewählt.";
            }

        }

        private void delete_loginData_click(object sender, RoutedEventArgs e) {
            ProzessManager pm = new ProzessManager("Delete Logindata", 2);
            ConfigManager config = new ConfigManager(Directory.GetCurrentDirectory() + "\\Data\\Login\\Data.conf");
            pm.addProgress();
            if (lb_saveDataPanel.SelectedItem != null) {
                config.init();
                config.remove(lb_saveDataPanel.SelectedItem.ToString());
                updateSavePanel();
                pm.done();
            } else {
                lb_error.Content = "Kein Speicherstand zum Löschen ausgewählt.";
                pm.done();
            }

        }

        private void updateSavePanel() {
            lb_saveDataPanel.Items.Clear();
            ConfigManager config = new ConfigManager(Directory.GetCurrentDirectory() + "\\Data\\Login\\Data.conf");
            config.init();
            List<string> list = config.readLines();
            for (int i = 0; i < list.Count; i++) {
                lb_saveDataPanel.Items.Add(list[i].ToString().Split('^')[0]);
            }
        }
    }
}
