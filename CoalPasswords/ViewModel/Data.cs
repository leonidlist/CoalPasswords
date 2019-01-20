using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;

namespace CoalPasswords
{
    class Data : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand CloseWindow { get; set; }
        public ICommand AddUser { get; set; }
        public ICommand SignIn { get; set; }
        public ICommand OpenMainWindow { get; set; }
        private IRepository<User> UsersRepository { get; set; }
        public User CurrentUser { get; set; }
        public string EnteredLogin { get; set; }
        public List<IRecord> TestRecords { get; set; } = new List<IRecord>();
        private string EnteredPassword { get; set; }
        private string _signUpStatusString;
        private PasswordsMain _passwordsMain;
        private object passedParam;
        public string SignUpStatusString
        {
            get => _signUpStatusString;
            set
            {
                _signUpStatusString = value;
                Notify();
            }
        }
        public Data(object param=null)
        {
            TestRecords.Add(new PasswordRecord { Email = "Test email", Password = "123123", Title = "Password for GDrive", Username = "USer", Website = "drive.google.com" });
            TestRecords.Add(new PasswordRecord { Email = "Test email", Password = "123123", Title = "Password for GDrive", Username = "USer", Website = "drive.google.com" });
            TestRecords.Add(new PasswordRecord { Email = "Test email", Password = "123123", Title = "Password for GDrive", Username = "USer", Website = "drive.google.com" });
            TestRecords.Add(new PasswordRecord { Email = "Test email", Password = "123123", Title = "Password for GDrive", Username = "USer", Website = "drive.google.com" });
            TestRecords.Add(new PasswordRecord { Email = "Test email", Password = "123123", Title = "Password for GDrive", Username = "USer", Website = "drive.google.com" });
            TestRecords.Add(new PasswordRecord { Email = "Test email", Password = "123123", Title = "Password for GDrive", Username = "USer", Website = "drive.google.com" });
            TestRecords.Add(new PasswordRecord { Email = "Test email", Password = "123123", Title = "Password for GDrive", Username = "USer", Website = "drive.google.com" });
            TestRecords.Add(new PasswordRecord { Email = "Test email", Password = "123123", Title = "Password for GDrive", Username = "USer", Website = "drive.google.com" });
            TestRecords.Add(new PasswordRecord { Email = "Test email", Password = "123123", Title = "Password for GDrive", Username = "USer", Website = "drive.google.com" });
            TestRecords.Add(new PasswordRecord { Email = "Test email", Password = "123123", Title = "Password for GDrive", Username = "USer", Website = "drive.google.com" });
            TestRecords.Add(new PasswordRecord { Email = "Test email", Password = "123123", Title = "Password for GDrive", Username = "USer", Website = "drive.google.com" });
            TestRecords.Add(new PasswordRecord { Email = "Test email", Password = "123123", Title = "Password for GDrive", Username = "USer", Website = "drive.google.com" });
            TestRecords.Add(new PasswordRecord { Email = "Test email", Password = "123123", Title = "Password for GDrive", Username = "USer", Website = "drive.google.com" });
            TestRecords.Add(new PasswordRecord { Email = "Test email", Password = "123123", Title = "Password for GDrive", Username = "USer", Website = "drive.google.com" });
            UsersRepository = new Repository<User>();
            CloseWindow = new RelayCommand(x => System.Windows.Application.Current.Shutdown());
            AddUser = new RelayCommand(ExecuteAddingUser);
            SignIn = new RelayCommand(ExecuteSigningIn);
            OpenMainWindow = new RelayCommand(ExecuteOpenMainWindow);
            passedParam = param;
        }
        private void ExecuteOpenMainWindow(object param)
        {
            _passwordsMain = new PasswordsMain();
            _passwordsMain.DataContext = this;
            _passwordsMain.Show();
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
                    if(user.Password == EnteredPassword)
                    {
                        SignUpStatusString = "Succesfully signed in.";
                        CurrentUser = user;
                        ExecuteOpenMainWindow(null);
                        return;
                    }
                }
                SignUpStatusString = "This username not exist.";
                return;
            }
            SignUpStatusString = "Error in Users repository.";
            throw new Exception("Users repositpry not List<User>");
        }
        private void ExecuteAddingUser(object param)
        {
            if(UsersRepository.GetAll() is List<User> list)
            {
                if(list.FindAll(x => x.Login == EnteredLogin).Count == 0 && !String.IsNullOrEmpty(EnteredLogin))
                {
                    var pass = param as PasswordBox;
                    EnteredPassword = pass.Password;
                    if(!String.IsNullOrEmpty(EnteredPassword))
                    {
                        UsersRepository.Add(new User(EnteredLogin, EnteredPassword));
                        SignUpStatusString = "Succesfully signed up.";
                        return;
                    }
                    SignUpStatusString = "Fill in all fields.";
                    return;
                }
                SignUpStatusString = "This username already exist.";
                return;
            }
            SignUpStatusString = "Error in Users repository.";
            throw new Exception("Users repositpry not List<User>");
        }
        private void Notify([CallerMemberName]string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
