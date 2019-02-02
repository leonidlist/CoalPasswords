using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CoalPasswords
{
    class MainViewModel : WindowViewModel, INotifyPropertyChanged
    {
        #region Private members
        private User _currentUser;
        private Visibility _recordPopupVisibility = Visibility.Hidden;
        private Visibility _addRecordPopupVisibility = Visibility.Hidden;
        #endregion

        public IRecord SelectedPassRecord { get; set; }
        public IRecord BufferRecord { get; set; }
        public Visibility RecordPopupVisibility
        {
            get => _recordPopupVisibility;
            set
            {
                _recordPopupVisibility = value;
                NotifyOfPropertyChange(() => RecordPopupVisibility);
            }
        }

        public Visibility AddRecordPopupVisibility
        {
            get => _addRecordPopupVisibility;
            set
            {
                _addRecordPopupVisibility = value;
                NotifyOfPropertyChange(() => AddRecordPopupVisibility);
            }
        }

        public ICommand AddTestCommand { get; set; }
        public ICommand AddPasswordRecordCommand { get; set; }
        public ICommand ClosePopupAddRecordCommand { get; set; }
        public ICommand OpenPopupShowRecordCommand { get; set; }
        public ICommand ClosePopupShowRecordCommand { get; set; }
        public ICommand OpenPopupSettingsCommand { get; set; }
        public ICommand ClosePopupSettingsCommand { get; set; }
        private ObservableCollection<IRecord> _passRecords;
        public ObservableCollection<IRecord> PassRecords { get => _passRecords; set { _passRecords = value; /*Notify();*/ } }
        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                //Notify();
            }
        }
        public MainViewModel(User currentUser)
        {
            CurrentUser = currentUser;
            PassRecords = new ObservableCollection<IRecord>(_currentUser.PasswordRecords.GetAll());
            BufferRecord = new PasswordRecord();
            //AddTestCommand = new RelayCommand(x => window.AddRecordGrid.Visibility = Visibility.Visible);
            //OpenPopupSettingsCommand = new RelayCommand(x => window.SettingsPopup.Visibility = Visibility.Visible);
            //ClosePopupSettingsCommand = new RelayCommand(x => window.SettingsPopup.Visibility = Visibility.Hidden);
        }
        public void AddPasswordRecord()
        {
            Random rnd = new Random();
            BufferRecord.ImageUrl = GetImageUrl(BufferRecord.Website);
            BufferRecord.CardColor = Pallete.Colors[rnd.Next(Pallete.Colors.Count)];
            PassRecords.Add(BufferRecord);
            
            DatabaseConnect dbc = new DatabaseConnect("CoalPasswords.db");
            dbc.AddPasswordRecord(BufferRecord, CurrentUser);
        }

        public void ToggleRecordPopupVisibility()
        {
            if (RecordPopupVisibility == Visibility.Hidden)
                RecordPopupVisibility = Visibility.Visible;
            else
                RecordPopupVisibility = Visibility.Hidden;
        }

        public void ToggleAddRecordVisibility()
        {
            if (AddRecordPopupVisibility == Visibility.Hidden)
                AddRecordPopupVisibility = Visibility.Visible;
            else
                AddRecordPopupVisibility = Visibility.Hidden;
        }

        private string GetImageUrl(string url)
        {
            return $"http://{url}/favicon.ico";
        }

        public void Close(object param)
        {
            if (param is Window wnd)
                wnd.Close();
        }
    }
}
