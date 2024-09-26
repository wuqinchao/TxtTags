using HandyControl.Controls;
using HandyControl.Data;
using Microsoft.VisualBasic.Devices;
using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
//using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TxtTags.Common;
using TxtTags.Converter;
using TxtTags.Dialog;
using TxtTags.Service;
using static ICSharpCode.AvalonEdit.Document.TextDocumentWeakEventManager;

namespace TxtTags.Pages
{
    /// <summary>
    /// PRepo.xaml 的交互逻辑
    /// </summary>
    public partial class PRepo : HandyControl.Controls.Window
    {
        public static RoutedUICommand QueryCmd = new RoutedUICommand("QueryCmd",
                "QueryCmd", typeof(PRepo));
        public static RoutedUICommand SelectCmd = new RoutedUICommand("SelectCmd",
                "SelectCmd", typeof(PRepo));

        public static RoutedUICommand OpenFileCmd = new RoutedUICommand("OpenFileCmd",
                "OpenFileCmd", typeof(PRepo));
        public static RoutedUICommand OpenFilePathCmd = new RoutedUICommand("OpenFilePathCmd",
                "OpenFilePathCmd", typeof(PRepo));
        public static RoutedUICommand RenameCmd = new RoutedUICommand("RenameCmd",
                "RenameCmd", typeof(PRepo));
        public static RoutedUICommand CopyNameCmd = new RoutedUICommand("CopyNameCmd",
                "CopyNameCmd", typeof(PRepo));
        public static RoutedUICommand CopyPathCmd = new RoutedUICommand("CopyPathCmd",
                "CopyPathCmd", typeof(PRepo));
        public static RoutedUICommand CopyCmd = new RoutedUICommand("CopyCmd",
                "CopyCmd", typeof(PRepo));
        public static RoutedUICommand DelFileCmd = new RoutedUICommand("DelFileCmd",
                "DelFileCmd", typeof(PRepo));
        public static RoutedUICommand DelFolderCmd = new RoutedUICommand("DelFolderCmd",
                "DelFolderCmd", typeof(PRepo));
        public static RoutedUICommand ToEncodingCmd = new RoutedUICommand("ToEncodingCmd",
                "ToEncodingCmd", typeof(PRepo));
        public static RoutedUICommand ToChinaCmd = new RoutedUICommand("ToChinaCmd",
                "ToChinaCmd", typeof(PRepo));
        public static RoutedUICommand MakeTagCmd = new RoutedUICommand("MakeTagCmd",
                "MakeTagCmd", typeof(PRepo));

        public static readonly DependencyProperty DataSourceProperty =
                    DependencyProperty.Register(
                        "DataSource",
                        typeof(Repo),
                        typeof(PRepo),
                        new FrameworkPropertyMetadata(null));
        public Repo DataSource
        {
            get { return (Repo)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }
        public static readonly DependencyProperty FilesProperty =
                    DependencyProperty.Register(
                        "Files",
                        typeof(ObservableCollection<TagFileInfo>),
                        typeof(PRepo),
                        new FrameworkPropertyMetadata(null));
        /// <summary>
        /// MODBUS配置
        /// </summary>
        public ObservableCollection<TagFileInfo> Files
        {
            get { return (ObservableCollection<TagFileInfo>)GetValue(FilesProperty); }
            set { SetValue(FilesProperty, value); }
        }
        public static readonly DependencyProperty OptionProperty =
                    DependencyProperty.Register(
                        "Option",
                        typeof(QueryOption),
                        typeof(PRepo),
                        new FrameworkPropertyMetadata(new QueryOption()));
        /// <summary>
        /// MODBUS配置
        /// </summary>
        public QueryOption Option
        {
            get { return (QueryOption)GetValue(OptionProperty); }
            set { SetValue(OptionProperty, value); }
        }
        public static readonly DependencyProperty BookProperty =
                    DependencyProperty.Register(
                        "Book",
                        typeof(TagFileInfo),
                        typeof(PRepo),
                        new FrameworkPropertyMetadata(null));
        public TagFileInfo Book
        {
            get { return (TagFileInfo)GetValue(BookProperty); }
            set { SetValue(BookProperty, value); }
        }
        public static readonly DependencyProperty TagsProperty =
                    DependencyProperty.Register(
                        "Tags",
                        typeof(ObservableCollection<TagItem>),
                        typeof(PRepo),
                        new FrameworkPropertyMetadata(new ObservableCollection<TagItem>()));
        public ObservableCollection<TagItem> Tags
        {
            get { return (ObservableCollection<TagItem>)GetValue(TagsProperty); }
            set { SetValue(TagsProperty, value); }
        }
        public static readonly DependencyProperty RunningProperty =
                    DependencyProperty.Register(
                        "Running",
                        typeof(bool),
                        typeof(PRepo),
                        new FrameworkPropertyMetadata(false));
        public bool Running
        {
            get { return (bool)GetValue(RunningProperty); }
            set { SetValue(RunningProperty, value); }
        }
        public static readonly DependencyProperty TagRepoProperty =
                    DependencyProperty.Register(
                        "TagRepo",
                        typeof(BookTags),
                        typeof(PRepo),
                        new FrameworkPropertyMetadata(BookTags.Instance));
        public BookTags TagRepo
        {
            get { return (BookTags)GetValue(TagRepoProperty); }
            set { SetValue(TagRepoProperty, value); }
        }
        public static readonly DependencyProperty ConfigerProperty =
                    DependencyProperty.Register(
                        "Configer",
                        typeof(Config),
                        typeof(PRepo),
                        new FrameworkPropertyMetadata(Config.Instance));
        public Config Configer
        {
            get { return (Config)GetValue(ConfigerProperty); }
            set { SetValue(ConfigerProperty, value); }
        }
        private string keyword = "";
        public PRepo()
        {
            this.DataContext = this;
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(QueryCmd, QueryHandle));
            CommandBindings.Add(new CommandBinding(SelectCmd, SelectHandle));

            CommandBindings.Add(new CommandBinding(OpenFileCmd, OpenFileHandle));
            CommandBindings.Add(new CommandBinding(OpenFilePathCmd, OpenFilePathHandle));
            CommandBindings.Add(new CommandBinding(RenameCmd, RenameHandle));
            CommandBindings.Add(new CommandBinding(CopyNameCmd, CopyNameHandle));
            CommandBindings.Add(new CommandBinding(CopyPathCmd, CopyPathHandle));
            CommandBindings.Add(new CommandBinding(CopyCmd, CopyHandle));
            CommandBindings.Add(new CommandBinding(DelFileCmd, DelFileHandle));
            CommandBindings.Add(new CommandBinding(DelFolderCmd, DelFolderHandle));
            CommandBindings.Add(new CommandBinding(ToEncodingCmd, ToEncodingHandle));
            CommandBindings.Add(new CommandBinding(ToChinaCmd, ToChinaHandle));
            CommandBindings.Add(new CommandBinding(MakeTagCmd, MakeTagHandle));
        }
        private void MakeTagHandle(object sender, ExecutedRoutedEventArgs e)
        {
            if (GridFiles.SelectedItems.Count < 1) return;
            var lst = GridFiles.SelectedItems;
            var fs = new List<TagFileInfo>();
            var cid = (int)e.Parameter;
            if(cid==0)
            {
                Growl.Info($"无法找到【{e.Parameter.ToString()}】的类别");
                return;
            }
            foreach (var item in lst)
            {
                var f = (TagFileInfo)item;
                if(!fs.Contains(f))
                {
                    fs.Add(f);
                }
            }
            DBatchTagSelection w = new DBatchTagSelection();
            w.TagId = cid;
            w.TagFile = fs;
            w.ShowDialog();
        }
        private void ToChinaHandle(object sender, ExecutedRoutedEventArgs e)
        {
            if (GridFiles.SelectedItems.Count < 1) return;
            var lst = GridFiles.SelectedItems;
            var fs = new List<TagFileInfo>();
            foreach (var item in lst)
            {
                fs.Add(((TagFileInfo)item));
            }
            DFileEncoding w = new DFileEncoding(fs, null);
            w.ShowDialog();
        }
        private void ToEncodingHandle(object sender, ExecutedRoutedEventArgs e)
        {
            if (GridFiles.SelectedItems.Count < 1) return;
            var lst = GridFiles.SelectedItems;
            var encoding = e.Parameter.ToString() == "UTF8" ? Encoding.UTF8 : Encoding.GetEncoding(54936);
            var fs = new List<TagFileInfo>();
            foreach (var item in lst)
            {
                fs.Add(((TagFileInfo)item));
            }
            DFileEncoding w = new DFileEncoding(fs, encoding);
            w.ShowDialog();
        }
        private void OpenFileHandle(object sender, ExecutedRoutedEventArgs e)
        {
            if (GridFiles.SelectedItems.Count > 0)
            {
                var file = (TagFileInfo)GridFiles.SelectedItems[0];
                file.Open();
            }
        }
        private void OpenFilePathHandle(object sender, ExecutedRoutedEventArgs e)
        {
            if (GridFiles.SelectedItems.Count > 0)
            {
                var file = (TagFileInfo)GridFiles.SelectedItems[0];
                FileOperation.ExplorerFile(file.File.FullPath);
            }
        }
        private void RenameHandle(object sender, ExecutedRoutedEventArgs e)
        {
            if (GridFiles.SelectedItems.Count > 0)
            {
                if (GridFiles.SelectedItems.Count == 1)
                {
                    var file = (TagFileInfo)GridFiles.SelectedItems[0];
                    DInput w = new DInput();
                    w.Value = file.OrgName;
                    w.Title = "请输入新文件名";
                    var b = w.ShowDialog();
                    if (b.HasValue && b.Value)
                    {
                        if (!string.IsNullOrWhiteSpace(w.Value))
                        {
                            file.OrgName = w.Value;
                            file.Rename();
                        }
                        else
                        {
                            Growl.Info($"文件名不能为空");
                        }
                    }
                    w = null;
                }
                else
                {
                    List<TagFileInfo> fs = new List<TagFileInfo>();
                    foreach(var f in GridFiles.SelectedItems)
                    {
                        fs.Add((TagFileInfo)f);
                    }
                    WRename w = new WRename(fs);
                    w.ShowDialog();
                }
                ResetSelectBook();
            }
        }
        private void CopyNameHandle(object sender, ExecutedRoutedEventArgs e)
        {
            if (GridFiles.SelectedItems.Count > 0)
            {
                var file = (TagFileInfo)GridFiles.SelectedItems[0];
                file.CopyName();
            }
        }
        private void CopyPathHandle(object sender, ExecutedRoutedEventArgs e)
        {

            if (GridFiles.SelectedItems.Count > 0)
            {
                var file = (TagFileInfo)GridFiles.SelectedItems[0];
                file.CopyPath();
            }
        }
        private void CopyHandle(object sender, ExecutedRoutedEventArgs e)
        {
            if (GridFiles.SelectedItems.Count < 1) return;
            var lst = GridFiles.SelectedItems;
            System.Collections.Specialized.StringCollection fs = new System.Collections.Specialized.StringCollection();
            foreach (var item in lst)
            {
                var path = ((TagFileInfo)item).File.FullPath;
                if (!fs.Contains(path))
                {
                    fs.Add(path);
                }
            }
            if (fs.Count < 1)
            {
                Growl.Info("无可处理内容");
                return;
            }
            Clipboard.SetFileDropList(fs);
        }
        private void DelFileHandle(object sender, ExecutedRoutedEventArgs e)
        {
            if (GridFiles.SelectedItems.Count < 1) return;
            var fs = GetSelectFiles();
            if (fs.Count < 1)
            {
                Growl.Info("无可处理内容");
                return;
            }
            var bs = SelectToList();
            if (System.Windows.MessageBox.Show($"确定删除{fs.Count}项内容吗?", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                FileOperation.ApiDelete(fs, true);
                RemoveFiles(bs, true);
            }
        }
        private void DelFolderHandle(object sender, ExecutedRoutedEventArgs e)
        {
            if (GridFiles.SelectedItems.Count < 1) return;
            var fs = GetSelectDirs();
            if (fs.Count < 1)
            {
                Growl.Info("无可处理内容");
                return;
            }
            List<TagFileInfo> bs = new List<TagFileInfo>();
            foreach(var f in Files)
            {
                if(fs.Contains(System.IO.Path.GetDirectoryName(f.File.FullPath)))
                {
                    bs.Add(f);
                }
            }
            if (System.Windows.MessageBox.Show($"确定删除{fs.Count}项内容的目录吗?", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                FileOperation.ApiDelete(fs, true);
                RemoveFiles(bs, true);
            }
        }
        private void QueryHandle(object sender, ExecutedRoutedEventArgs e)
        {
            //Growl.Error(new GrowlInfo() { Message = "StaysOpen false", StaysOpen = false });
            if (Running) return;
            Option.Tags.Clear();
            foreach(var item in Tags)
            {
                if (item.Checked)
                    Option.Tags.Add(item.Name);
            }
            Running = true;
            QueryServiceCompleted handle = null;
            QueryService.Instance.OnWorkerCompleted += handle = (service, result) =>
            {
                QueryService.Instance.OnWorkerCompleted -= handle;
                Running = false;
                if (result.OK)
                {
                    Files = new ObservableCollection<TagFileInfo>(result.Data);
                    if (Book != null)
                    {
                        if (Files.Any(x => x.File.FullPath == Book.File.FullPath))
                        {
                            Book = Files.First(x => x.File.FullPath == Book.File.FullPath);
                        }
                        else
                        {
                            Book = null;
                        }
                    }
                }
                else
                {
                    Growl.Error(new GrowlInfo() { Message = result.Message, StaysOpen=false});
                }
            };
            keyword = Option.Keywords;
            QueryService.Instance.Do(Option);
        }
        private void SelectHandle(object sender, ExecutedRoutedEventArgs e)
        {
            //Book = null;
            Book = (TagFileInfo)e.Parameter;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Option.BasePath = DataSource.Path;
            this.Title = DataSource.Name;
            foreach(var c in BookTags.Instance.Items)
            {
                if (c.Hide && Config.Instance.Locked) continue;
                foreach(var s in c.Tags)
                {
                    if(!Tags.Any(x=>x.Name.Equals(s)))
                    {
                        Tags.Add(new TagItem(s));
                    }                    
                }
            }
            QueryHandle(null, null);
        }
        private void Keywords_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                QueryHandle(null, null);
            }
        }

        private bool LeftDown = false;
        private List<string> DragFiles = new List<string>();
        private List<TagFileInfo> backups = new List<TagFileInfo>();
        private void GridFiles_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var ele = GridFiles.InputHitTest(e.GetPosition(GridFiles));
            if(ele is FrameworkElement cell)
            {
                if(cell.DataContext is TagFileInfo file)
                {
                    DragFiles.Clear();
                    backups.Clear();
                    LeftDown = true;
                    Book = file;
                    var lst = GridFiles.SelectedItems;
                    foreach (var item in lst)
                    {
                        if (!DragFiles.Contains(((TagFileInfo)item).File.FullPath))
                        {
                            backups.Add((TagFileInfo)item);
                            DragFiles.Add(((TagFileInfo)item).File.FullPath);
                        }
                    }
                    if (!DragFiles.Contains(file.File.FullPath))
                    {
                        backups.Add(file);
                        DragFiles.Add(file.File.FullPath);
                    }                    
                    return;
                }
            }
            LeftDown = false;
        }
        private void GridFiles_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            LeftDown = false;
            DragFiles.Clear();
        }
        private void GridFiles_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && LeftDown && DragFiles.Count > 0)
            {
                //Console.WriteLine($"GridFiles_MouseMove DragFiles:{DragFiles.Count}");
                LeftDown = false;
                System.Windows.DataObject mv = new System.Windows.DataObject(System.Windows.DataFormats.FileDrop, DragFiles.ToArray());
                DragFiles.Clear();
                //Console.WriteLine($"GridFiles_MouseMove clear");
                var dropEffect = DragDrop.DoDragDrop(GridFiles, mv, System.Windows.DragDropEffects.All);
                if (dropEffect == DragDropEffects.Move)
                {
                    RemoveFiles(backups);
                    backups.Clear();
                }
            }
            else
            {
                LeftDown = false;
            }
        }
        private void UStar_ValueChanged(object sender, StarChangedEventArgs e)
        {
            //if (Option.Star != e.Star)
                Option.Star = e.Star;
            //else
            //    Option.Star = 0;
            QueryHandle(null, null);
        }
        private void GridFiles_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var ele = GridFiles.InputHitTest(e.GetPosition(GridFiles));
            if (ele is TextBlock cell)
            {
                if (cell.DataContext is TagFileInfo file)
                {
                    WBook w = new WBook();
                    w.DataSource = file;
                    w.Show();
                }
            }
        }
    
        private List<string> GetSelectFiles()
        {
            List<string> fs = new List<string>();
            var lst = GridFiles.SelectedItems;
            foreach (var item in lst)
            {
                var path = ((TagFileInfo)item).File.FullPath;
                if (!fs.Contains(path))
                {
                    fs.Add(path);
                }
            }
            return fs;
        }
        public List<string> GetSelectDirs()
        {
            List<string> fs = new List<string>();
            var lst = GridFiles.SelectedItems;
            foreach (var item in lst)
            {
                var path = System.IO.Path.GetDirectoryName(((TagFileInfo)item).File.FullPath);
                if (!fs.Contains(path))
                {
                    fs.Add(path);
                }
            }
            return fs;
        }

        public List<TagFileInfo> SelectToList()
        {
            List<TagFileInfo> fs = new List<TagFileInfo>();
            var lst = GridFiles.SelectedItems;
            foreach (var item in lst)
            {
                fs.Add((TagFileInfo)item);
            }
            return fs;
        }
    
        public void RemoveFiles(List<TagFileInfo> ls, bool delete=false)
        {
            foreach (var f in ls)
            {
                Files.Remove(f);
                if(Book == f)
                {
                    Book = null;
                }
                if(delete)
                {
                    BookmarkService.Delete(f.File.FullPath);
                }
            }
        }
        public void ResetSelectBook()
        {
            if (Book != null)
            {
                if (Files.Any(x => x.File.FullPath == Book.File.FullPath))
                {
                    Book = Files.First(x => x.File.FullPath == Book.File.FullPath);
                    BookView.Reload();
                }
                else
                {
                    Book = null;
                }
            }
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            var control = sender as HandyControl.Controls.TextBox;
            var now = control.Text.Trim();
            var lastChar = now.Length > 0 ? control.Text.Trim().Last() : char.MaxValue;
            if (keyword.Trim() != now && (now.Length == 0 || (lastChar != '|' && lastChar != '!')))
                QueryHandle(null, null);
        }
    }
    public class TagSelectedConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var list = (ObservableCollection<string>)parameter;
            return list.Contains(value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TagItem: NotifyPropertyBase
    {
        public TagItem(string name) 
        {
            Name = name;
            Checked = false;
        }
        private string _name;
        private bool _checked = false;
        public string Name { get => _name; set { _name = value; OnPropertyChanged("Name"); } }
        public bool Checked { get => _checked; set { _checked = value; OnPropertyChanged("Checked"); } }
    }
}
