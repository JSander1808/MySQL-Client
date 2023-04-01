using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaktionslogik für AddTable.xaml
    /// </summary>
    public partial class AddTable : Window {

        private int rows = 3;
        public AddTable() {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void addRowOptionElement(object sender, RoutedEventArgs e) {
            Dispatcher dispatcher = Application.Current.Dispatcher;
            Grid grid;
            TextBox name;
            ComboBox typ;
            TextBox length;
            ComboBox standard;
            CheckBox nul;
            CheckBox aI;
            dispatcher.Invoke(() => { 
                grid= new Grid();
                name= new TextBox();
                typ= new ComboBox();
                length= new TextBox();
                standard= new ComboBox();
                nul= new CheckBox();
                aI= new CheckBox();
                if (rows % 2 == 0) grid.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#292929") ?? (SolidColorBrush)new BrushConverter().ConvertFrom("#333333");

                name.Height = 29;
                name.Width = 150;
                name.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#404040");
                name.Margin= new Thickness(22,15,0,15);
                name.HorizontalAlignment= HorizontalAlignment.Left;
                name.FontSize = 20;
                name.Foreground = Brushes.White;

                typ.Height = 29;
                typ.Width = 100;
                typ.Padding = new Thickness(5,0,0,0);
                typ.FontSize = 20;
                typ.Foreground = Brushes.White;
                typ.HorizontalAlignment = HorizontalAlignment.Left;
                typ.Margin = new Thickness(227,0,0,0);


                length.Height = 29;
                length.Width = 125;
                length.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#404040");
                length.Margin = new Thickness(378,15,0,15);
                length.HorizontalAlignment= HorizontalAlignment.Left;
                length.PreviewTextInput += TextBox_PreviewTextInput;
                length.Foreground = Brushes.White;
                length.FontSize = 20;

                standard.Height = 29;
                standard.Width = 210;
                standard.FontSize= 20;
                standard.Foreground = Brushes.White;
                standard.Padding = new Thickness(5,0,0,0);
                standard.HorizontalAlignment = HorizontalAlignment.Left;
                standard.Margin = new Thickness(538,0,0,0);

                nul.Margin = new Thickness(802, 20, 0, 19);
                nul.HorizontalAlignment= HorizontalAlignment.Left;
                nul.Foreground= Brushes.White;
                nul.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#404040");
                nul.LayoutTransform = new ScaleTransform(2,2);

                aI.Margin = new Thickness(958, 20, 0, 19);
                aI.HorizontalAlignment = HorizontalAlignment.Left;
                aI.Foreground= Brushes.White;
                aI.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#404040");
                aI.LayoutTransform = new ScaleTransform(2, 2);

                grid.Children.Add(name);
                grid.Children.Add(typ);
                grid.Children.Add(length);
                grid.Children.Add(standard);
                grid.Children.Add(nul);
                grid.Children.Add(aI);
                stp_rowOptionsView.Children.Add(grid);
            });
        }
    }
}
