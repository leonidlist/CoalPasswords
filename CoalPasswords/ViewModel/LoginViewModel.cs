using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using Caliburn.Micro;

namespace CoalPasswords
{
    class LoginViewModel:WindowViewModel
    {
        #region Private members
        private IWindowManager windowManager;
        private IRepository<User> UsersRepository { get; set; }
        private User _signedInUser;
        private string _enteringStatusString;
        private string EnteredPassword { get; set; }
        #endregion

        #region Properties
        public string EnteredLogin { get; set; }
        public string EnteringStatusString
        {
            get => _enteringStatusString;
            set
            {
                _enteringStatusString = value;
                NotifyOfPropertyChange(() => EnteringStatusString);
            }
        }
        #endregion

        public void SignUp(object param)
        {
            if (UsersRepository.GetAll() is List<User> list)
            {
                if (list.FindAll(x => x.Login == EnteredLogin).Count == 0 && !String.IsNullOrEmpty(EnteredLogin))
                {
                    var pass = param as PasswordBox;
                    EnteredPassword = pass.Password;
                    if (!String.IsNullOrEmpty(EnteredPassword))
                    {
                        UsersRepository.Add(new User(EnteredLogin, EnteredPassword));
                        DatabaseConnect dbc = new DatabaseConnect("CoalPasswords.db");
                        dbc.AddUser("Steve", "Jobs", EnteredLogin, EnteredPassword);
                        EnteringStatusString = "Succesfully signed up.";
                        return;
                    }
                    EnteringStatusString = "Fill in all fields.";
                    return;
                }
                EnteringStatusString = "This username already exist.";
                return;
            }
            EnteringStatusString = "Error in Users repository.";
            throw new Exception("Users repositpry not List<User>");
        }
        

        public LoginViewModel(IWindowManager windowManager) : base()
        {
            this.windowManager = windowManager;
        }

        private void SetThemeOnStartup(string themeName)
        {
            ResourceDictionary color = new ResourceDictionary();
            color.Source = new Uri($"pack://application:,,,/Styles/{themeName}.xaml", UriKind.Absolute);
            Application.Current.Resources.MergedDictionaries.Add(color);

            ResourceDictionary material = new ResourceDictionary();
            material.Source = new Uri($"pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.{(themeName.Contains("Dark")?"Dark":"Light")}.xaml");
            Application.Current.Resources.MergedDictionaries.Add(material);
        }
        private void SetLanguageOnStartup(string language)
        {
            ResourceDictionary lang = new ResourceDictionary();
            lang.Source = new Uri($"pack://application:,,,/Languages/{language}.xaml", UriKind.Absolute);
            Application.Current.Resources.MergedDictionaries.Add(lang);
        }
        public void Deserialize()
        {
            SetLanguageOnStartup("English");
            SetThemeOnStartup("LightColors");
            DatabaseConnect dbc = new DatabaseConnect("CoalPasswords.db");
            UsersRepository = dbc.GetUsers();
        }
        public void OpenMainWindow(object param)
        {
            windowManager.ShowWindow(new MainViewModel(_signedInUser));
        }
        public void SignIn(object param, object wnd)
        {
            if (UsersRepository.GetAll() is List<User> list)
            {
                var user = list.Find(x => x.Login == EnteredLogin);
                if (user != null)
                {
                    var pass = param as PasswordBox;
                    EnteredPassword = pass.Password;
                    if (user.Password == EnteredPassword)
                    {
                        EnteringStatusString = "Succesfully signed in.";
                        _signedInUser = user;
                        OpenMainWindow(null);
                        (wnd as Window).Close();
                        return;
                    }
                }
                EnteringStatusString = "This username not exist.";
                return;
            }
            EnteringStatusString = "Error in Users repository.";
            throw new Exception("Users repositpry not List<User>");
        }

        public void Close(object wnd)
        {
            (wnd as Window).Close();
        }
    }
}
