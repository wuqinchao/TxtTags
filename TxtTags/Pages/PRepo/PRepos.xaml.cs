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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TxtTags.Common;
using TxtTags.Dialog;

namespace TxtTags.Pages
{
    /// <summary>
    /// PRepos.xaml 的交互逻辑
    /// </summary>
    public partial class PRepos : HandyControl.Controls.Window
    {
        public static RoutedUICommand GoBackCmd = new RoutedUICommand("GoBackCmd",
                "GoBackCmd", typeof(PRepos));
        public static RoutedUICommand CreateCmd = new RoutedUICommand("CreateCmd",
                "CreateCmd", typeof(PRepos));

        public static RoutedUICommand SelectCmd = new RoutedUICommand("SelectCmd",
                "SelectCmd", typeof(PRepos));

        public static RoutedUICommand UpateCmd = new RoutedUICommand("UpateCmd",
                "UpateCmd", typeof(PRepos));

        public static RoutedUICommand DeleteCmd = new RoutedUICommand("DeleteCmd",
                "DeleteCmd", typeof(PRepos));

        public static readonly DependencyProperty DataSourceProperty =
                    DependencyProperty.Register(
                        "DataSource",
                        typeof(Repos),
                        typeof(PRepos),
                        new FrameworkPropertyMetadata(Repos.Instance));
        /// <summary>
        /// MODBUS配置
        /// </summary>
        public Repos DataSource
        {
            get { return (Repos)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }
        public PRepos()
        {
            this.DataContext = this;
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(GoBackCmd, OnGoBackCmd));
            CommandBindings.Add(new CommandBinding(SelectCmd, SelectHandler));
            CommandBindings.Add(new CommandBinding(CreateCmd, CreateHandle));
            CommandBindings.Add(new CommandBinding(UpateCmd, UpdateHandler));
            CommandBindings.Add(new CommandBinding(DeleteCmd, DeleteHandler));
        }
        private void OnGoBackCmd(object sender, ExecutedRoutedEventArgs e)
        {
            NavigationService.GetNavigationService(this).GoBack();
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
    }
}
