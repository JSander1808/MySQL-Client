using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySQL_Client {
    internal class MainViewManager {

        public MainViewManager() { 
        
        }

        public static void loadDataTable(string command) {
            string[] tagArgs = command.Split("!?#-2");
            MySQLHandle.setDatabase(tagArgs[1]);
            DataTable dt = MySQLHandle.SelectData("select * from `" + tagArgs[2] + "`;");
            ((MainWindow)System.Windows.Application.Current.MainWindow).table_view_datagrid.ItemsSource = dt.DefaultView;
            updateAddressList("Server: " + tagArgs[0]+" >> Datenbank: " + tagArgs[1]+" >> Tabelle: " + tagArgs[2]);
        }

        public static void updateAddressList(string command) {
            ((MainWindow)System.Windows.Application.Current.MainWindow).addresslist.Content = command;
        }
    }
}
