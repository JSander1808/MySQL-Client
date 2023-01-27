using MySql.Data.MySqlClient;
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
    /// Interaktionslogik für RemoveDatabase.xaml
    /// </summary>
    public partial class RemoveDatabase : Window {
        public RemoveDatabase() {
            InitializeComponent();
            loadTreeView();
        }

        private async void loadTreeView() {
            await Task.Run(() => {
                Dispatcher dispatcher = Application.Current.Dispatcher;
                dispatcher.Invoke(() => { treeview.Items.Clear(); });
                ProzessManager pm = new ProzessManager("Load Database View", 3);
                pm.addProgress();
                MySqlDataReader reader = MySQLHandle.GetData("show databases;");
                if (reader != null) {
                    pm.addProgress();
                    while (reader.Read()) {
                        TreeViewItem item;
                        string header = reader.GetString(0);
                        dispatcher.Invoke(() => {
                            item = new TreeViewItem();
                            item.Header = header;
                            item.Foreground = Brushes.White;
                            item.FontSize = 15;
                            treeview.Items.Add(item);
                        });
                    }
                }
                pm.addProgress();
                pm.done();
            });
        }

        private void cancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void submit_Click(object sender, RoutedEventArgs e) {
            ProzessManager pm = new ProzessManager("Delete Database",0);
            pm.addProgress();
            if(treeview.SelectedItem!= null) {
                TreeViewItem item = treeview.SelectedItem as TreeViewItem;
                var result = MessageBox.Show("Sind sie sicher das sie die Datenbank " + item.Header.ToString() + " unwiederruflich löschen wollen?", "caption",MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes) {
                    if (MySQLHandle.sendDirekt("drop database `" + item.Header.ToString() + "`;")) {
                        Close();
                        MainWindow.updateTreeView();
                    }
                }
            } else {
                MainViewManager.setErrorMessage("Selected database cannot be null.");
            }
            pm.addProgress();
            pm.done();
        }
    }
}
