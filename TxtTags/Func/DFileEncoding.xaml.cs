using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
using TxtTags.Dialog;
using TxtTags.Pages;
using TxtTags.Service;

namespace TxtTags
{
    /// <summary>
    /// DFileEncoding.xaml 的交互逻辑
    /// </summary>
    public partial class DFileEncoding : HandyControl.Controls.Window
    {
        public static RoutedUICommand OkCmd = new RoutedUICommand("OkCmd",
                "OkCmd", typeof(DFileEncoding));
        public static readonly DependencyProperty DataSourceProperty =
                    DependencyProperty.Register(
                        "DataSource",
                        typeof(FileEncodingService),
                        typeof(DFileEncoding),
                        new FrameworkPropertyMetadata(new FileEncodingService()));
        public FileEncodingService DataSource
        {
            get { return (FileEncodingService)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }
        public DFileEncoding(List<TagFileInfo> fs, Encoding to)
        {
            this.DataContext = this;
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(OkCmd, OkHandle));
            DataSource.Init(fs, to);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataSource.Start();
        }
        private void OkHandle(object sender, ExecutedRoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
