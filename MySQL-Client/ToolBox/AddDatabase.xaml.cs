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

namespace MySQL_Client {
    /// <summary>
    /// Interaktionslogik für AddDatabase.xaml
    /// </summary>
    public partial class AddDatabase : Window {
        public AddDatabase() {
            InitializeComponent();
        }

        private void cancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private async void submit_Click(object sender, RoutedEventArgs e) {
            ProzessManager pm = new ProzessManager("Create Database",2);
            pm.addProgress();
            if(tb_databaseName.Text != null && tb_databaseName.Text != "") {
                if (MySQLHandle.sendDirekt("create database `" + tb_databaseName.Text + "`;")) {
                    Close();
                    MainWindow.updateTreeView();
                }
            } else {
                MainViewManager.setErrorMessage("Database name cannot be null.");
            }
            pm.addProgress();
            pm.done();
        }
    }
}
