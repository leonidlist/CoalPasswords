using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CoalPasswords
{
    class MainViewModel : WindowViewModel, INotifyPropertyChanged
    {
        #region Private members
        private int _selectedThemeIndex = 0;
        private int _selectedLanguageIndex = 0;
        private string _searchString = String.Empty;
        private User _currentUser;
        private ObservableCollection<IRecord> _bufferSearchRecords;
        private ObservableCollection<IRecord> _passRecords;
        private Visibility _recordPopupVisibility = Visibility.Hidden;
        private Visibility _addRecordPopupVisibility = Visibility.Hidden;
        private Visibility _settingsPopupVisibility = Visibility.Hidden;
        #endregion

        #region Properties
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

        public Visibility SettingsPopupVisibility
        {
            get => _settingsPopupVisibility;
            set
            {
                _settingsPopupVisibility = value;
                NotifyOfPropertyChange(() => SettingsPopupVisibility);
            }
        }
        public ObservableCollection<IRecord> PassRecords
        {
            get
            {
                return _passRecords;
            }
            set
            {
                _passRecords = value;
                NotifyOfPropertyChange(() => PassRecords);
            }
        }
        public ObservableCollection<IRecord> BufferSearchRecords
        {
            get => _bufferSearchRecords;
            set
            {
                _bufferSearchRecords = value;
                NotifyOfPropertyChange(() => BufferSearchRecords);
            }
        }
        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;

                NotifyOfPropertyChange(() => CurrentUser);
            }
        }
        public int SelectedThemeIndex
        {
            get => _selectedThemeIndex;
            set
            {
                _selectedThemeIndex = value;
                CurrentUser.Theme = _selectedThemeIndex.ToString();
                DatabaseConnect dbc = new DatabaseConnect("CoalPasswords.db");
                dbc.UpdateUserTheme(CurrentUser);
                ChangeTheme();
                NotifyOfPropertyChange(() => SelectedThemeIndex);
            }
        }

        public int SelectedLanguageIndex
        {
            get => _selectedLanguageIndex;
            set
            {
                _selectedLanguageIndex = value;
                CurrentUser.Language = _selectedLanguageIndex.ToString();
                DatabaseConnect dbc = new DatabaseConnect("CoalPasswords.db");
                dbc.UpdateUserLanguage(CurrentUser);
                ChangeLanguage();
                NotifyOfPropertyChange(() => SelectedLanguageIndex);
            }
        }

        public int SelectedCategoryIndex
        {
            get; set;
        }

        public string SearchString
        {
            get => _searchString;
            set
            {
                _searchString = value;
                BufferSearchRecords = new ObservableCollection<IRecord>(PassRecords.Where(x => x.Title.Contains(SearchString)));
                NotifyOfPropertyChange(() => SearchString);
            }
        }
        #endregion

        public MainViewModel(User currentUser)
        {
            CurrentUser = currentUser;
            PassRecords = new ObservableCollection<IRecord>(_currentUser.PasswordRecords.GetAll());
            BufferSearchRecords = new ObservableCollection<IRecord>(PassRecords);
            BufferRecord = new PasswordRecord();
            SelectedLanguageIndex = Convert.ToInt32(CurrentUser.Language);
            SelectedThemeIndex = Convert.ToInt32(CurrentUser.Theme);
        }

        public void AddPasswordRecord()
        {
            Random rnd = new Random();
            BufferRecord.ImageUrl = GetImageUrl(BufferRecord.Website);
            BufferRecord.CardColor = Pallete.Colors[rnd.Next(Pallete.Colors.Count)];
            BufferRecord.Category = SelectedCategoryIndex;
            BufferRecord.UniqueId = rnd.Next(100000, 1000000);
            DatabaseConnect dbc = new DatabaseConnect("CoalPasswords.db");
            while(!dbc.AddPasswordRecord(BufferRecord, CurrentUser))
            {
                BufferRecord.UniqueId = rnd.Next(100000, 1000000);
            }
            PassRecords.Add(BufferRecord);
            BufferSearchRecords = new ObservableCollection<IRecord>(PassRecords);
            ToggleRecordPopupVisibility();
        }

        public void UpdatePasswordRecord(IRecord record)
        {
            DatabaseConnect dbc = new DatabaseConnect("CoalPasswords.db");
            dbc.UpdatePasswordRecord(record);
        }

        public void AllPasswordsButton()
        {
            SearchString = String.Empty;
        }

        public void SortByCategory(int categoryId)
        {
            SearchString = String.Empty;
            BufferSearchRecords = new ObservableCollection<IRecord>(PassRecords.Where(x => x.Category == categoryId));
        }

        public void ToggleSettingsPopupVisibility()
        {
            if (SettingsPopupVisibility == Visibility.Hidden)
                SettingsPopupVisibility = Visibility.Visible;
            else
                SettingsPopupVisibility = Visibility.Hidden;
        }

        public void ToggleRecordPopupVisibility()
        {
            if (RecordPopupVisibility == Visibility.Hidden)
                RecordPopupVisibility = Visibility.Visible;
            else
            {
                RecordPopupVisibility = Visibility.Hidden;
                UpdatePasswordRecord(SelectedPassRecord);
            }
        }

        public void ToggleAddRecordVisibility()
        {
            if (AddRecordPopupVisibility == Visibility.Hidden)
                AddRecordPopupVisibility = Visibility.Visible;
            else
            {
                AddRecordPopupVisibility = Visibility.Hidden;
            }
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

        private void ChangeTheme()
        {
            Thread spectator = null;
            string themeName;
            if (_selectedThemeIndex == 2)
            {
                spectator = new Thread(new ThreadStart(SpectateForThemeChanging));
                spectator.Start();
                return;
            }

            switch (_selectedThemeIndex)
            {
                case 0:
                    themeName = "Light";
                    spectator?.Abort();
                    break;
                case 1:
                    themeName = "Dark";
                    spectator?.Abort();
                    break;
                default:
                    themeName = "Light";
                    break;
            }
            ReplaceThemeResources(themeName);
            return;
        }

        private void ChangeLanguage()
        {
            string language;
            switch (_selectedLanguageIndex)
            {
                case 0:
                    language = "English";
                    break;
                case 1:
                    language = "Russian";
                    break;
                case 2:
                    language = "Ukrainian";
                    break;
                default:
                    language = "English";
                    break;
            }
            ReplaceLanguageResource(language);
        }

        private void SpectateForThemeChanging()
        {
            while (true)
            {
                string themeName;
                if (DateTime.Now.Hour >= 20 || DateTime.Now.Hour < 6)
                    themeName = "Dark";
                else
                    themeName = "Light";
                ReplaceThemeResources(themeName);
                Thread.Sleep(60000);
            }
        }

        private void ReplaceThemeResources(string themeName)
        {
            Application.Current.Resources.MergedDictionaries.RemoveAt(Application.Current.Resources.MergedDictionaries.Count - 1);
            Application.Current.Resources.MergedDictionaries.RemoveAt(Application.Current.Resources.MergedDictionaries.Count - 1);

            ResourceDictionary color = new ResourceDictionary();
            color.Source = new Uri($"pack://application:,,,/Styles/{themeName + "Colors"}.xaml", UriKind.Absolute);
            Application.Current.Resources.MergedDictionaries.Add(color);

            ResourceDictionary material = new ResourceDictionary();
            material.Source = new Uri($"pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.{(themeName.Contains("Dark") ? "Dark" : "Light")}.xaml");
            Application.Current.Resources.MergedDictionaries.Add(material);
        }

        public void ReplaceLanguageResource(string language)
        {
            ResourceDictionary lang = new ResourceDictionary();
            lang.Source = new Uri($"pack://application:,,,/Languages/{language}.xaml", UriKind.Absolute);
            Application.Current.Resources.MergedDictionaries.Insert(Application.Current.Resources.MergedDictionaries.Count - 3, lang);
            Application.Current.Resources.MergedDictionaries.RemoveAt(Application.Current.Resources.MergedDictionaries.Count - 3);
        }

        public void DeleteRecord(object record)
        {
            ToggleRecordPopupVisibility();
            CurrentUser.PasswordRecords.Remove(record as IRecord);
            PassRecords.Remove(record as IRecord);
            BufferSearchRecords = new ObservableCollection<IRecord>(PassRecords);
            DatabaseConnect databaseConnect = new DatabaseConnect("CoalPasswords.db");
            databaseConnect.RemoveRecord(record as IRecord, CurrentUser);
        }
    }
}
