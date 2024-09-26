using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
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
using TxtTags.Entity;
using TxtTags.Service;

namespace TxtTags
{
    /// <summary>
    /// WRename.xaml 的交互逻辑
    /// </summary>
    public partial class WRename : HandyControl.Controls.Window
    {
        public static RoutedUICommand ProcessCmd = new RoutedUICommand("ProcessCmd",
                "ProcessCmd", typeof(WRename));
        public static RoutedUICommand RemoveCmd = new RoutedUICommand("RemoveCmd",
                "RemoveCmd", typeof(WRename));
        public static RoutedUICommand StartCmd = new RoutedUICommand("StartCmd",
                "StartCmd", typeof(WRename));

        public static readonly DependencyProperty DataSourceProperty =
                    DependencyProperty.Register(
                        "DataSource",
                        typeof(SortableObservableCollection<RenameInfo>),
                        typeof(WRename),
                        new FrameworkPropertyMetadata(new SortableObservableCollection<RenameInfo>()));
        public SortableObservableCollection<RenameInfo> DataSource
        {
            get { return (SortableObservableCollection<RenameInfo>)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }
        public static readonly DependencyProperty PreTxtProperty =
                    DependencyProperty.Register(
                        "PreTxt",
                        typeof(string),
                        typeof(WRename),
                        new FrameworkPropertyMetadata(""));
        public string PreTxt
        {
            get { return (string)GetValue(PreTxtProperty); }
            set { SetValue(PreTxtProperty, value); }
        }
        public static readonly DependencyProperty ReplacementProperty =
                    DependencyProperty.Register(
                        "Replacement",
                        typeof(string),
                        typeof(WRename),
                        new FrameworkPropertyMetadata(""));
        public string Replacement
        {
            get { return (string)GetValue(ReplacementProperty); }
            set { SetValue(ReplacementProperty, value); }
        }
        public static readonly DependencyProperty LastTxtProperty =
                    DependencyProperty.Register(
                        "Last",
                        typeof(string),
                        typeof(WRename),
                        new FrameworkPropertyMetadata(""));
        public string LastTxt
        {
            get { return (string)GetValue(LastTxtProperty); }
            set { SetValue(LastTxtProperty, value); }
        }
        public static readonly DependencyProperty RunningProperty =
                    DependencyProperty.Register(
                        "Running",
                        typeof(bool),
                        typeof(WRename),
                        new FrameworkPropertyMetadata(false));
        public bool Running
        {
            get { return (bool)GetValue(RunningProperty); }
            set { SetValue(RunningProperty, value); }
        }
        private System.ComponentModel.BackgroundWorker bg = new System.ComponentModel.BackgroundWorker();
        public WRename(List<TagFileInfo> list)
        {
            this.DataContext = this;
            InitializeComponent();
            DataSource.Clear();
            this.bg.DoWork += new System.ComponentModel.DoWorkEventHandler(bg_DoWork);
            this.bg.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bg_RunWorkerCompleted);
            CommandBindings.Add(new CommandBinding(ProcessCmd, ProcessHandle));
            CommandBindings.Add(new CommandBinding(RemoveCmd, RemoveHandle));
            CommandBindings.Add(new CommandBinding(StartCmd, StartHandle));
            foreach (TagFileInfo item in list)
            {
                DataSource.Add(new RenameInfo() {
                    File = item,
                    Org = item.OrgName,
                    Pattern = "",
                    Replacement = "",
                    Last = "",
                    Result = ""
                });
            }
        }
        ~WRename()
        {
            this.bg.DoWork -= new System.ComponentModel.DoWorkEventHandler(bg_DoWork);
            this.bg.RunWorkerCompleted -= new System.ComponentModel.RunWorkerCompletedEventHandler(bg_RunWorkerCompleted);
        }
        private void ProcessHandle(object sender, ExecutedRoutedEventArgs e)
        {
            foreach (var item in DataSource)
            {
                item.Apply(PreTxt, Replacement, LastTxt);
            }
        }
        private void RemoveHandle(object sender, ExecutedRoutedEventArgs e)
        {
            while (GridFiles.SelectedItems.Count > 0)
            {
                DataSource.Remove((RenameInfo)(GridFiles.SelectedItems[0]));
            }
        }
        private void StartHandle(object sender, ExecutedRoutedEventArgs e)
        {
            Running = true;
            bg.RunWorkerAsync(DataSource);
        }
        private void bg_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var list = (SortableObservableCollection<RenameInfo>)e.Argument;
            foreach(var item in list)
            {
                item.DoRename();
            }
        }
        private void bg_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Running = false;
            System.Windows.MessageBox.Show("操作完成");
        }
    }

    public class RenameInfo : NotifyPropertyBase, IComparable<RenameInfo>
    {
        private TagFileInfo _file;
        private string _org;
        private string _pattern;
        private string _replacement;
        private string _last;
        private string _result;
        public TagFileInfo File { get => _file; set { _file = value; } }
        public string Org { get => _org; set { _org = value; OnPropertyChanged("Org"); } }
        public string Pattern { get => _pattern; set { _pattern = value; OnPropertyChanged("Pattern"); } }
        public string Replacement { get => _replacement; set { _replacement = value; OnPropertyChanged("Replacement"); } }
        public string Last { get => _last; set { _last = value; OnPropertyChanged("Last"); } }
        public string Result { get => _result; set { _result = value; OnPropertyChanged("Result"); } }
        public void Apply(string pattern, string replacement, string last)
        {
            Replacement = replacement;

            try
            {
                Pattern = string.IsNullOrWhiteSpace(pattern) ? Org : Regex.Replace(Org, pattern, replacement);
                Last = string.IsNullOrWhiteSpace(last) ? Pattern : string.Format(last, Pattern);
                Result = "";
            }
            catch(Exception ex) 
            {
                Pattern = Last = "";
                Result = ex.ToString();
            }
        }
        public string DoRename()
        {
            if(string.IsNullOrWhiteSpace(Pattern)||string.IsNullOrWhiteSpace(Last))
            {
                return "";
            }
            else
            {
                var name = File.GetNewName(Last);
                if(System.IO.File.Exists(name))
                {
                    return Result = "文件名重复";
                }
                else
                {
                    File.OrgName = Last;
                    try
                    {
                        File.Rename();
                    }
                    catch(Exception ex)
                    {
                        return Result = ex.Message;
                    }
                    return Result = "成功";
                }
            }
        }
        public int CompareTo(RenameInfo other)
        {
            if(!string.IsNullOrWhiteSpace(this.Result) || !string.IsNullOrWhiteSpace(other.Result))
            {
                if(!string.IsNullOrWhiteSpace(this.Result) && !string.IsNullOrWhiteSpace(other.Result))
                {
                    if(this.Result != "成功")
                    {
                        return other.Result == "成功" ? 1 : -1;
                    }
                    else
                    {
                        return other.Result == "成功" ? -1 : 1;
                    }
                }
                else
                {
                    if(!string.IsNullOrWhiteSpace(this.Result))
                    {
                        return -1;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
            else
            {
                return this.Org.CompareTo(other.Org);
            }
        }
    }
}
