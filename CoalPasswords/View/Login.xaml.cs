using System;
using System.Windows;
using System.Windows.Input;

namespace CoalPasswords
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class LoginPage : Window
    {
        Data data = new Data();
        public LoginPage()
        {
            InitializeComponent();
            LoginMainBorder.DataContext = data;
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
