 using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace CoalPasswords
{
    /// <summary>
    /// View model for custom window with rounded corners.
    /// </summary>
    class WindowViewModel:INotifyPropertyChanged
    {
        #region Protected Members
        /// <summary>
        /// The window this ViewModel controls
        /// </summary>
        protected Window _mainWindow;
        /// <summary>
        /// Margin around the window to allow drop shadows
        /// </summary>
        protected int _outerMarginSize = 5;
        /// <summary>
        /// Window corner radius
        /// </summary>
        protected int _cornerRadius = 5;
        protected string _windowTitle;
        #endregion

        #region Public Members
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand CloseWindowCommand { get; set; }
        public int ResizeBorder { get; set; } = 3;
        public int TitleHeight { get; set; } = 40;
        public Thickness ResizeBorderThickness
        {
            get => new Thickness(ResizeBorder);
        }
        public CornerRadius WindowCornerRadius
        {
            get => new CornerRadius(_cornerRadius);
        }
        public int OuterMarginSize
        {
            get => _outerMarginSize;
            set
            {
                _outerMarginSize = value;
                Notify();
            }
        }
        public int CornerRadius
        {
            get => _cornerRadius;
            set
            {
                _cornerRadius = value;
                Notify();
            }
        }
        public string WindowTitle
        {
            get => _windowTitle;
            set
            {
                _windowTitle = value;
                Notify();
            }
        }
        public ResizeMode ResizeMode { get; set; }
        #endregion

        public WindowViewModel(Window window)
        {
            _mainWindow = window;
            _windowTitle = window.Title;
            ResizeMode = window.ResizeMode;
            CloseWindowCommand = new RelayCommand(x => _mainWindow.Close());
        }
        protected void Notify([CallerMemberName]string callerName="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName));
        }
    }
}
