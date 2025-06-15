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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MasterP
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Page1 page1 = new Page1();

            NavigationWindow navWindow = new NavigationWindow();
            navWindow.Content = page1;
            navWindow.Show();
            this.Close();
        }

        private void Button_Click_Two(object sender, RoutedEventArgs e)
        {
            Page2 page2 = new Page2();

            NavigationWindow navWindow = new NavigationWindow();
            navWindow.Content = page2;
            navWindow.Show();
            this.Close();
        }
        private void Button_Click_Tree(object sender, RoutedEventArgs e)
        {
            Page3 page3 = new Page3();

            NavigationWindow navWindow = new NavigationWindow();
            navWindow.Content = page3;
            navWindow.Show();
            this.Close();
        }
    }
}
