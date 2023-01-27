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
