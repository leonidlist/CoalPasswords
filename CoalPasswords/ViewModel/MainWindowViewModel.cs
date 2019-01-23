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
        /// <summary>
        /// Текущий пользователь
        /// </summary>
        private User _currentUser;
        /// <summary>
        /// Команда открытия всплывающего окна о добавлении записи
        /// </summary>
        public ICommand AddTestCommand { get; set; }
        /// <summary>
        /// Команда добавления новой записи
        /// </summary>
        public ICommand AddPasswordRecordCommand { get; set; }
        /// <summary>
        /// Команда закрытия всплывающего окна о добавлении записи
        /// </summary>
        public ICommand ClosePopupAddRecordCommand { get; set; }
        /// <summary>
        /// Коллекция сохраненных паролей текущего пользователя
        /// </summary>
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
            AddTestCommand = new RelayCommand(x => window.AddRecordGrid.Visibility = Visibility.Visible);
            ClosePopupAddRecordCommand = new RelayCommand(x => window.AddRecordGrid.Visibility = Visibility.Hidden);
            AddPasswordRecordCommand = new RelayCommand(AddPasswordRecord);
        }

        private void AddPasswordRecord(object param = null)
        {
            PasswordsMain curr = _mainWindow as PasswordsMain;
            PassRecords.Add(new PasswordRecord{ Title = curr.titleAdd.Text, Username = curr.usernameAdd.Text, Email = curr.emailAdd.Text, Password = curr.passwordAdd.Text, Website = curr.websiteAdd.Text});
        }
    }
}
