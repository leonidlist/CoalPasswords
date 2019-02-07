using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CoalPasswords
{
    public class Color
    {
        public string BackgroundColor { get; set; }
        public string ForegroundColor { get; set; }
        public Brush BackgroundBrush
        {
            get => new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString(BackgroundColor));
        }
    }
}
