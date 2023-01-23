using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace MySQL_Client {
    internal class ProzessManager {

        private string prozessName = null!;
        private int steps = 0;
        private int value = 0;
        private Stopwatch stopwatch = new Stopwatch();
        private ProgressBar progressBar;
        private Label label;
        private Grid grid;
        private Dispatcher uidispatcher = Application.Current.Dispatcher;

        public ProzessManager(string prozessName, int steps) {
            uidispatcher.BeginInvoke(() => {
                progressBar = new ProgressBar();
                label = new Label();
                grid = new Grid();
            });
            this.prozessName = prozessName;
            this.steps = steps;
            stopwatch.Start();
            create();
        }

        public void addProgress() {
            this.value++;
            update();
        }

        public async void done() {
            stopwatch.Stop();
            uidispatcher.BeginInvoke(() =>{
                label.Content = prozessName + ": Done after " + stopwatch.ElapsedMilliseconds + "ms"; 
                progressBar.Visibility = Visibility.Collapsed;
                label.Margin = new Thickness(0, 0, 0, 0);
            });
            await Task.Delay(2000);
            uidispatcher.BeginInvoke(() => { ((MainWindow)System.Windows.Application.Current.MainWindow).prozess_panel.Children.Remove(grid); });
        }

        public void create() {
            uidispatcher.BeginInvoke(() => {

                progressBar.Maximum = steps;
                progressBar.Value = value;
                progressBar.Background = new BrushConverter().ConvertFrom("#404040") as SolidColorBrush;
                progressBar.Foreground = Brushes.Lime;
                progressBar.Height = 15;
                progressBar.Width = 150;
                progressBar.HorizontalAlignment = HorizontalAlignment.Right;

                label.Content = prozessName + ":";
                label.Foreground = Brushes.White;
                label.FontSize = 20;
                label.Padding = new Thickness(0, 0, 10, 0);
                label.Margin = new Thickness(0, 0, 150, 0);
                label.HorizontalAlignment = HorizontalAlignment.Left;

                grid.Margin = new Thickness(0, 0, 10, 0);
                grid.Children.Add(label);
                grid.Children.Add(progressBar);
                ((MainWindow)Application.Current.MainWindow).prozess_panel.Children.Add(grid);
            });
        }

        public void update() {
            uidispatcher.BeginInvoke(() => {
                progressBar.Maximum = steps;
                progressBar.Value = value;
                progressBar.Background = new BrushConverter().ConvertFrom("#404040") as SolidColorBrush;
                progressBar.Foreground = Brushes.Lime;
                progressBar.Height = 15;
                progressBar.Width = 150;
                progressBar.HorizontalAlignment = HorizontalAlignment.Right;

                label.Content = prozessName + ":";
                label.Foreground = Brushes.White;
                label.FontSize = 20;
                label.Padding = new Thickness(0,0,10,0);
                label.Margin = new Thickness(0, 0, 150, 0);
                label.HorizontalAlignment = HorizontalAlignment.Left;
            });
        }
    }
}
