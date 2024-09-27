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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
        public static RoutedUICommand ToReposCmd = new RoutedUICommand("ToReposCmd",
                "ToReposCmd", typeof(MainWindow));
        public static RoutedUICommand ToTagsCmd = new RoutedUICommand("ToTagsCmd",
                "ToTagsCmd", typeof(MainWindow));
        public static RoutedUICommand LockCmd = new RoutedUICommand("LockCmd",
                "LockCmd", typeof(MainWindow));
        public static RoutedUICommand CreateCmd = new RoutedUICommand("CreateCmd",
                "CreateCmd", typeof(MainWindow));
        public static RoutedUICommand SelectCmd = new RoutedUICommand("SelectCmd",
                "SelectCmd", typeof(MainWindow));
        public static RoutedUICommand UpdateCmd = new RoutedUICommand("UpdateCmd",
                "UpdateCmd", typeof(MainWindow));
        public static RoutedUICommand DeleteCmd = new RoutedUICommand("DeleteCmd",
                "DeleteCmd", typeof(MainWindow));

        public static readonly DependencyProperty DataSourceProperty =
                    DependencyProperty.Register(
                        "DataSource",
                        typeof(Repos),
                        typeof(MainWindow),
                        new FrameworkPropertyMetadata(Repos.Instance));
        /// <summary>
        /// MODBUS配置
        /// </summary>
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
        public MainWindow()
        {
            this.DataContext = this;
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(ToReposCmd, ToReposHandle));
            CommandBindings.Add(new CommandBinding(ToTagsCmd, ToTagsHandle));
            CommandBindings.Add(new CommandBinding(LockCmd, LockHandle));
            CommandBindings.Add(new CommandBinding(SelectCmd, SelectHandler));
            CommandBindings.Add(new CommandBinding(CreateCmd, CreateHandle));
            CommandBindings.Add(new CommandBinding(UpdateCmd, UpdateHandler));
            CommandBindings.Add(new CommandBinding(DeleteCmd, DeleteHandler));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //WIcons w = new WIcons();
            //w.Show();
        }
        private void ToReposHandle(object sender, ExecutedRoutedEventArgs e)
        {
            PRepos w = new PRepos();
            WinManager.ShowChild(this, w, true);
        }
        private void ToTagsHandle(object sender, ExecutedRoutedEventArgs e)
        {
            WTags w = new WTags();
            WinManager.ShowChild(this, w, true);
        }
        private void LockHandle(object sender, ExecutedRoutedEventArgs e)
        {
            Configer.Locked = !Configer.Locked;
            //DataSource.OnPropertyChanged("Items");
        }
        private void DeleteHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("您确定要删除此仓库吗?", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                DataSource.Remove((int)e.Parameter);
            }
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

        private void UpdateHandler(object sender, ExecutedRoutedEventArgs e)
        {
            DRepoEdit w = new DRepoEdit();
            w.DataSource = DataSource.Fetch((int)e.Parameter).Clone();
            w.Owner = Application.Current.MainWindow;
            w.ShowDialog();
            w = null;
        }
        //public static object InvokeInternal<T>(this T caller, string method, object[] parameters)
        //{
        //    MethodInfo methodInfo = typeof(T).GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic);
        //    return methodInfo?.Invoke(caller, parameters);
        //}
    }

    //public class BookShowConvert : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        return ((Repo)value).Hide && Config.Instance.Locked ? Visibility.Collapsed : Visibility.Visible;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

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
}
