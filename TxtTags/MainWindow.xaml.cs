using HandyControl.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TxtTags.Common;
using TxtTags.Converter;
using TxtTags.Dialog;
using TxtTags.Pages;
using TxtTags.Service;

namespace TxtTags
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : HandyControl.Controls.Window
    {
        public static RoutedUICommand ToTagsCmd = new RoutedUICommand("ToTagsCmd",
                "ToTagsCmd", typeof(MainWindow));
        public static RoutedUICommand LockCmd = new RoutedUICommand("LockCmd",
                "LockCmd", typeof(MainWindow));
        public static RoutedUICommand CreateCmd = new RoutedUICommand("CreateCmd",
                "CreateCmd", typeof(MainWindow));
        public static RoutedUICommand SelectCmd = new RoutedUICommand("SelectCmd",
                "SelectCmd", typeof(MainWindow));
        public static RoutedUICommand LinktoCmd = new RoutedUICommand("LinktoCmd",
                "LinktoCmd", typeof(MainWindow));

        public static readonly DependencyProperty DataSourceProperty =
                    DependencyProperty.Register(
                        "DataSource",
                        typeof(Repos),
                        typeof(MainWindow),
                        new FrameworkPropertyMetadata(Repos.Instance));
        public Repos DataSource
        {
            get { return (Repos)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }

        public static readonly DependencyProperty ConfigerProperty =
                    DependencyProperty.Register(
                        "Configer",
                        typeof(Config),
                        typeof(MainWindow),
                        new FrameworkPropertyMetadata(Config.Instance));
        public Config Configer
        {
            get { return (Config)GetValue(ConfigerProperty); }
            set { SetValue(ConfigerProperty, value); }
        }
        public ICommand BookUpdateCommand { get; set; }
        public ICommand BookDeleteCommand { get; set; }

        public MainWindow()
        {
            this.DataContext = this;
            this.BookUpdateCommand = new BookUpdateCommand(this);
            this.BookDeleteCommand = new BookDeleteCommand(this);
            CommandBindings.Add(new CommandBinding(ToTagsCmd, ToTagsHandle));
            CommandBindings.Add(new CommandBinding(LockCmd, LockHandle));
            CommandBindings.Add(new CommandBinding(SelectCmd, SelectHandler));
            CommandBindings.Add(new CommandBinding(CreateCmd, CreateHandle));
            CommandBindings.Add(new CommandBinding(LinktoCmd, LinktoHandler));
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //WIcons w = new WIcons();
            //w.Show();
        }
        private void LinktoHandler(object sender, ExecutedRoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(e.Parameter.ToString()));
            e.Handled = true;
        }
        private void ToTagsHandle(object sender, ExecutedRoutedEventArgs e)
        {
            WTags w = new WTags();
            WinManager.ShowChild(this, w, true);
        }
        private void LockHandle(object sender, ExecutedRoutedEventArgs e)
        {
            Configer.Locked = !Configer.Locked;
        }
        private void SelectHandler(object sender, ExecutedRoutedEventArgs e)
        {
            var w = new PRepo();
            w.DataSource = (Repo)e.Parameter;
            w.Owner = this;
            WinManager.ShowChild(this, w, true);
        }
        private void CreateHandle(object sender, ExecutedRoutedEventArgs e)
        {
            var r = DataSource.GetNewId();
            if (!r.OK)
            {
                Growl.Info(r.Message);
                return;
            }
            Repo item = new Repo() { Id = r.Data };
            DRepoEdit w = new DRepoEdit();
            w.DataSource = item;
            w.Owner = Application.Current.MainWindow;
            w.ShowDialog();
            w = null;
        }
    }

    public class BookShowConvert : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (values == null || values.Length < 2)
                {
                    return Visibility.Collapsed;
                }

                return ((Repo)values[0]).Hide && (bool)values[1] ? Visibility.Collapsed : Visibility.Visible;
            }
            catch
            {
            }
            return Binding.DoNothing;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    class BookUpdateCommand : ICommand
    {

        public BookUpdateCommand(MainWindow viemo)
        {
            win = viemo;
        }

        MainWindow win { get; set; }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        int i = 0;

        /// <summary>
        /// 命令是否可用
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return parameter != null;
        }

        /// <summary>
        /// 命令执行的操作
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            var repo = Repos.Instance.Fetch((int)parameter);
            DRepoEdit w = new DRepoEdit();
            w.DataSource = repo.Clone();
            w.Owner = Application.Current.MainWindow;
            w.ShowDialog();
            w = null;
        }

    }
    class BookDeleteCommand : ICommand
    {

        public BookDeleteCommand(MainWindow viemo)
        {
            win = viemo;
        }

        MainWindow win { get; set; }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        int i = 0;

        /// <summary>
        /// 命令是否可用
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return parameter != null;
        }

        /// <summary>
        /// 命令执行的操作
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            var repo = Repos.Instance.Fetch((int)parameter);
            if (System.Windows.MessageBox.Show($"您确定要删除【{repo.Name}】吗?", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                win.DataSource.Remove(repo.Id);
            }
        }

    }
}
