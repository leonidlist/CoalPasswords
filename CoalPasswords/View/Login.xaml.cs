﻿using System;
using System.Windows;
using System.Windows.Input;

namespace CoalPasswords
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class LoginPage : Window
    {
        public LoginPage()
        {
            InitializeComponent();
            DataContext = new WindowViewModel(this);
        }

        //private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    DragMove();
        //}
    }
}
