using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CoalPasswords
{
    class Data : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand CloseWindow { get; set; }
        public ICommand AddUser { get; set; }
        private IRepository<User> UsersRepository { get; set; }
        public string EnteredLogin { get; set; }
        public string EnteredPassword { get; set; }
        public Data()
        {
            UsersRepository = new Repository<User>();
            CloseWindow = new RelayCommand(x => System.Windows.Application.Current.Shutdown());
            AddUser = new RelayCommand(x => { UsersRepository.Add(new User(EnteredLogin, EnteredPassword)); });
        }
        public void ShowUsers()
        {
            foreach(var user in UsersRepository.GetAll())
            {
                Console.WriteLine(user.Login + " " + user.Password);
                Console.WriteLine("Records: " + (user.PasswordRecords.GetAll() as IList<IRecord>).Count);
            }
        }
    }
}
