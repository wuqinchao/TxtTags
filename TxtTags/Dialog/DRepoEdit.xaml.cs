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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TxtTags.Common;

namespace TxtTags.Dialog
{
    /// <summary>
    /// DRepoEdit.xaml 的交互逻辑
    /// </summary>
    public partial class DRepoEdit : HandyControl.Controls.Window
    {
        public static RoutedUICommand FolderCmd = new RoutedUICommand("FolderCmd",
                "FolderCmd", typeof(DRepoEdit));
        public static RoutedUICommand OkCmd = new RoutedUICommand("OkCmd",
                "OkCmd", typeof(DRepoEdit));
        public static RoutedUICommand CancelCmd = new RoutedUICommand("CancelCmd",
                "CancelCmd", typeof(DRepoEdit));

        public static readonly DependencyProperty DataSourceProperty =
                    DependencyProperty.Register(
                        "DataSource",
                        typeof(Repo),
                        typeof(DRepoEdit),
                        new FrameworkPropertyMetadata(null));
        /// <summary>
        /// MODBUS配置
        /// </summary>
        public Repo DataSource
        {
            get { return (Repo)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }
        public DRepoEdit()
        {
            this.DataContext = this;
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(FolderCmd, FolderHandle));
            CommandBindings.Add(new CommandBinding(OkCmd, OkHandle));
            CommandBindings.Add(new CommandBinding(CancelCmd, CancelHandle));
        }
        public static readonly DependencyProperty ConfigerProperty =
                    DependencyProperty.Register(
                        "Configer",
                        typeof(Config),
                        typeof(DRepoEdit),
                        new FrameworkPropertyMetadata(Config.Instance));
        public Config Configer
        {
            get { return (Config)GetValue(ConfigerProperty); }
            set { SetValue(ConfigerProperty, value); }
        }

        private void FolderHandle(object sender, ExecutedRoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = true;
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataSource.Path = folderBrowserDialog.SelectedPath;
            }
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
                Growl.Info("请提供仓库名称");
                return;
            }
            if (String.IsNullOrWhiteSpace(DataSource.Path))
            {
                Growl.Info("请提供管理目录");
                return;
            }
            Repos.Instance.Add(DataSource);
            this.DialogResult = true;
            this.Close();
        }
    }
    /*
    var dialog = new CommonOpenFileDialog();
    dialog.IsFolderPicker = true;
    CommonFileDialogResult result = dialog.ShowDialog();
    if (result == CommonFileDialogResult.Ok)
    {
        var fileName = dialog.FileName;
        Console.WriteLine("文件夹名字：" + fileName);              
    }
    */
}
