using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TxtTags.Service;

namespace TxtTags.Func
{
    /// <summary>
    /// UNormalizeOption.xaml 的交互逻辑
    /// </summary>
    public partial class UNormalizeOption : UserControl
    {
        public static readonly DependencyProperty OptionProperty =
            DependencyProperty.Register("Option", typeof(NormalizeOption), typeof(UNormalizeOption), new UIPropertyMetadata(new NormalizeOption()));
        public NormalizeOption Option
        {
            get { return (NormalizeOption)GetValue(OptionProperty); }
            set { SetValue(OptionProperty, value); }
        }
        public UNormalizeOption()
        {
            InitializeComponent();
        }
    }
}
