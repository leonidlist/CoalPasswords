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
        private IRepository<User> UsersRepository { get; set; }
        public string EnteredLogin { get; set; }
        private string EnteredPassword { get; set; }
        private string _signUpStatusString;
        public string SignUpStatusString
        {
            get => _signUpStatusString;
            set
            {
                _signUpStatusString = value;
                Notify();
            }
        }
        public Data()
        {
            UsersRepository = new Repository<User>();
            CloseWindow = new RelayCommand(x => System.Windows.Application.Current.Shutdown());
            AddUser = new RelayCommand(ExecuteAddingUser);
            SignIn = new RelayCommand(ExecuteSigningIn);
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
                        SignUpStatusString = "Succesfully signed in.";
                    return;
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
                if(list.FindAll(x => x.Login == EnteredLogin).Count == 0)
                {
                    var pass = param as PasswordBox;
                    EnteredPassword = pass.Password;
                    UsersRepository.Add(new User(EnteredLogin, EnteredPassword));
                    SignUpStatusString = "Succesfully signed up.";
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
