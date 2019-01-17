using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CoalPasswords
{
    class Data : INotifyPropertyChanged
    {
        public ICommand CloseWindow { get; set; }
        private string login;
        private string password;
        public string Login
        {
            get => login;
            set
            {
                login = value;
                Notify();
            }
        }
        public string Password
        {
            get => password;
            set
            {
                password = value;
                Notify();
            }
        }

        public int CallerMemberName { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public Data()
        {
            CloseWindow = new RelayCommand(x => System.Windows.Application.Current.Shutdown());
        }
        private void Notify([CallerMemberName]string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
