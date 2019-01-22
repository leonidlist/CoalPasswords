using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CoalPasswords
{
    class MainWindowViewModel:WindowViewModel,INotifyPropertyChanged
    {
        private User _currentUser;
        public ICommand AddTestCommand { get; set; }
        private ObservableCollection<IRecord> _passRecords;
        public ObservableCollection<IRecord> PassRecords { get => _passRecords; set { _passRecords = value; Notify(); } }
        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                Notify();
            }
        }

        public MainWindowViewModel(PasswordsMain window, User currentUser) : base(window)
        {
            CurrentUser = currentUser;
            PassRecords = new ObservableCollection<IRecord>(_currentUser.PasswordRecords.GetAll());
            AddTestCommand = new RelayCommand(x => PassRecords.Add(new PasswordRecord { Title = "GBoard" }));
        }
    }
}
