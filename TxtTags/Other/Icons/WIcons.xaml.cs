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
using System.Windows.Shapes;

namespace TxtTags
{
    /// <summary>
    /// WIcons.xaml 的交互逻辑
    /// </summary>
    public partial class WIcons : HandyControl.Controls.Window
    {
        public static RoutedUICommand CopyCmd = new RoutedUICommand("CopyCmd",
                "CopyCmd", typeof(WIcons));
        public WIcons()
        {
            this.DataContext = this;
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(CopyCmd, CopyHandle));
        }
        private void CopyHandle(object sender, ExecutedRoutedEventArgs e)
        {
            Clipboard.SetDataObject(e.Parameter.ToString());
        }
    }
}
