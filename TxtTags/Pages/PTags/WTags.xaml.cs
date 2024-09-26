using HandyControl.Controls;
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
using TxtTags.Common;
using TxtTags.Dialog;

namespace TxtTags.Pages
{
    /// <summary>
    /// WTags.xaml 的交互逻辑
    /// </summary>
    public partial class WTags : HandyControl.Controls.Window
    {
        public static RoutedUICommand CreateCmd = new RoutedUICommand("CreateCmd",
                "CreateCmd", typeof(WTags));

        public static RoutedUICommand UpateCmd = new RoutedUICommand("UpateCmd",
                "UpateCmd", typeof(WTags));

        public static RoutedUICommand DeleteCmd = new RoutedUICommand("DeleteCmd",
                "DeleteCmd", typeof(WTags));

        public static readonly DependencyProperty DataSourceProperty =
                    DependencyProperty.Register(
                        "DataSource",
                        typeof(BookTags),
                        typeof(WTags),
                        new FrameworkPropertyMetadata(BookTags.Instance));
        /// <summary>
        /// MODBUS配置
        /// </summary>
        public BookTags DataSource
        {
            get { return (BookTags)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }
        public static readonly DependencyProperty ConfigerProperty =
                    DependencyProperty.Register(
                        "Configer",
                        typeof(Config),
                        typeof(WTags),
                        new FrameworkPropertyMetadata(Config.Instance));
        public Config Configer
        {
            get { return (Config)GetValue(ConfigerProperty); }
            set { SetValue(ConfigerProperty, value); }
        }
        public WTags()
        {
            this.DataContext = this;
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(CreateCmd, CreateHandle));
            CommandBindings.Add(new CommandBinding(UpateCmd, UpdateHandler));
            CommandBindings.Add(new CommandBinding(DeleteCmd, DeleteHandler));
        }

        private void DeleteHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("您确定要删除此标签类别吗?", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                DataSource.Remove((int)e.Parameter);
            }
        }
        private void CreateHandle(object sender, ExecutedRoutedEventArgs e)
        {
            var r = DataSource.GetNewId();
            if (!r.OK)
            {
                Growl.Info(r.Message);
                return;
            }
            BookTag item = new BookTag() { Id = r.Data };
            DTagEdit w = new DTagEdit();
            w.DataSource = item;
            w.Owner = Application.Current.MainWindow;
            w.ShowDialog();
            w = null;
        }

        private void UpdateHandler(object sender, ExecutedRoutedEventArgs e)
        {
            DTagEdit w = new DTagEdit();
            w.DataSource = DataSource.Fetch((int)e.Parameter).Clone();
            w.Owner = Application.Current.MainWindow;
            w.ShowDialog();
            w = null;
        }
    }
}
