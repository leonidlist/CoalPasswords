using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;

namespace CoalPasswords
{
    class WindowViewModel:PropertyChangedBase
    {
        #region Protected Members

        protected int _cornerRadius = 5;
        protected string _windowTitle;

        #endregion

        #region Public Members

        public int ResizeBorder { get; set; } = 3;
        public int TitleHeight { get; set; } = 40;
        public ResizeMode ResizeMode { get; set; } = ResizeMode.NoResize;
        public Thickness ResizeBorderThickness
        {
            get => new Thickness(ResizeBorder);
        }
        public CornerRadius WindowCornerRadius
        {
            get => new CornerRadius(_cornerRadius);
        }
        public int CornerRadius
        {
            get => _cornerRadius;
            set
            {
                _cornerRadius = value;
                NotifyOfPropertyChange(() => CornerRadius);
            }
        }
        public string WindowTitle
        {
            get => _windowTitle;
            set
            {
                _windowTitle = value;
                NotifyOfPropertyChange(() => CornerRadius);
            }
        }

        #endregion

        public WindowViewModel()
        {

        }
    }
}
