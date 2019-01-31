using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;

namespace CoalPasswords
{
    class LoginViewModel:WindowViewModel
    {
        private IRepository<User> UsersRepository { get; set; }
        private User _signedInUser;
        /// <summary>
        /// TODO In future
        /// </summary>
        private MainView _passwordsMainWindow;
        private string _enteringStatusString;
        private string EnteredPassword { get; set; }
        public ICommand AddUserCommand { get; set; }
        public ICommand SignInCommand { get; set; }
        public ICommand OpenMainWindowCommand { get; set; }
        public ICommand SerializingCommand { get; set; }
        public ICommand DeserializingCommand { get; set; }
        public string EnteredLogin { get; set; }
        public string EnteringStatusString
        {
            get => _enteringStatusString;
            set
            {
                _enteringStatusString = value;
                Notify();
            }
        }
        public LoginViewModel(LoginView window) : base(window)
        {
            AddUserCommand = new RelayCommand(ExecuteAddingUser);
            SignInCommand = new RelayCommand(ExecuteSigningIn);
            OpenMainWindowCommand = new RelayCommand(OpenMainWindow);
            SerializingCommand = new RelayCommand(Serialize);
            DeserializingCommand = new RelayCommand(Deserialize);
        }
        private void Serialize(object param)
        {
            //BinaryFormatter bf = new BinaryFormatter();
            //using(FileStream fs = new FileStream("data.coal", FileMode.OpenOrCreate))
            //{
            //    bf.Serialize(fs, UsersRepository);
            //}
        }
        private void Deserialize(object param)
        {
            //try
            //{
            //    BinaryFormatter bf = new BinaryFormatter();
            //    using (FileStream fs = new FileStream("data.coal", FileMode.Open))
            //    {
            //        UsersRepository = (Repository<User>)bf.Deserialize(fs);
            //    }
            //} catch(Exception) { UsersRepository = new Repository<User>(); }
            DatabaseConnect dbc = new DatabaseConnect("CoalPasswords.db");
            UsersRepository = dbc.GetUsers();
        }
        private void OpenMainWindow(object param)
        {
            _passwordsMainWindow = new MainView();
            _passwordsMainWindow.DataContext = new MainViewModel(_passwordsMainWindow, _signedInUser);
            _passwordsMainWindow.Show();
        }
        private void ExecuteSigningIn(object param)
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
                        _mainWindow.Close();
                        return;
                    }
                }
                EnteringStatusString = "This username not exist.";
                return;
            }
            EnteringStatusString = "Error in Users repository.";
            throw new Exception("Users repositpry not List<User>");
        }
        private void ExecuteAddingUser(object param)
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
    }
}
