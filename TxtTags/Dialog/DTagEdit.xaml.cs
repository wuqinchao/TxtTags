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
using TxtTags.Common;
using TxtTags.Dialog;

namespace TxtTags
{
    /// <summary>
    /// DTagEdit.xaml 的交互逻辑
    /// </summary>
    public partial class DTagEdit : HandyControl.Controls.Window
    {
        public static RoutedUICommand OkCmd = new RoutedUICommand("OkCmd",
                "OkCmd", typeof(DTagEdit));
        public static RoutedUICommand CancelCmd = new RoutedUICommand("CancelCmd",
                "CancelCmd", typeof(DTagEdit));

        public static readonly DependencyProperty DataSourceProperty =
                    DependencyProperty.Register(
                        "DataSource",
                        typeof(BookTag),
                        typeof(DTagEdit),
                        new FrameworkPropertyMetadata(null));
        /// <summary>
        /// MODBUS配置
        /// </summary>
        public BookTag DataSource
        {
            get { return (BookTag)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }
        public static readonly DependencyProperty ConfigerProperty =
                    DependencyProperty.Register(
                        "Configer",
                        typeof(Config),
                        typeof(DTagEdit),
                        new FrameworkPropertyMetadata(Config.Instance));
        public Config Configer
        {
            get { return (Config)GetValue(ConfigerProperty); }
            set { SetValue(ConfigerProperty, value); }
        }
        public DTagEdit()
        {
            this.DataContext = this;
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(OkCmd, OkHandle));
            CommandBindings.Add(new CommandBinding(CancelCmd, CancelHandle));
        }

        private void CancelHandle(object sender, ExecutedRoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void OkHandle(object sender, ExecutedRoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(DataSource.Name))
            {
                Growl.Info("请提供标签类别名称");
                return;
            }
            if (String.IsNullOrWhiteSpace(DataSource.Tag))
            {
                Growl.Info("请提供标签");
                return;
            }
            BookTags.Instance.Add(DataSource);
            this.DialogResult = true;
            this.Close();
        }
    }
}
