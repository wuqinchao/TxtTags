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
using System.Windows.Threading;
using TxtTags.Common;
using TxtTags.Dialog;
using TxtTags.Entity;
using TxtTags.Service;
using static System.Collections.Specialized.BitVector32;

namespace TxtTags
{
    /// <summary>
    /// USearcher.xaml 的交互逻辑
    /// </summary>
    public partial class USearcher : UserControl
    {
        public USearcher()
        {
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(ToSearchReultCmd, ToSearchReultHandle));
        }
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(USearcher), new PropertyMetadata(null));
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(USearcher), new PropertyMetadata(null));
        /// <summary>
        /// 获取或设置Command的值
        /// </summary>  
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        /// <summary>
        /// 获取或设置CommandParameter的值
        /// </summary>  
        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }
        public static RoutedUICommand ToSearchReultCmd = new RoutedUICommand("ToSearchReultCmd",
                "ToSearchReultCmd", typeof(USearcher));
        public static readonly DependencyProperty UrlProperty =
                    DependencyProperty.Register("Url", typeof(string), typeof(USearcher), new FrameworkPropertyMetadata("", OnUrlChanged));
        public string Url
        {
            get { return (string)GetValue(UrlProperty); }
            set { SetValue(UrlProperty, value); }
        }
        public static readonly DependencyProperty KeywordProperty =
                    DependencyProperty.Register("Keyword", typeof(string), typeof(USearcher), new FrameworkPropertyMetadata(""));
        public string Keyword
        {
            get { return (string)GetValue(KeywordProperty); }
            set { SetValue(KeywordProperty, value); }
        }
        public static readonly DependencyProperty ResultsProperty =
            DependencyProperty.Register("Results", typeof(SortableObservableCollection<BookmarkInfo>), typeof(USearcher), new UIPropertyMetadata(new SortableObservableCollection<BookmarkInfo>()));
        public SortableObservableCollection<BookmarkInfo> Results
        {
            get { return (SortableObservableCollection<BookmarkInfo>)GetValue(ResultsProperty); }
            set { SetValue(ResultsProperty, value); }
        }
        public static readonly DependencyProperty RunningProperty =
            DependencyProperty.Register("Running", typeof(bool), typeof(USearcher), new FrameworkPropertyMetadata(false));
        public bool Running
        {
            get { return (bool)GetValue(RunningProperty); }
            set { SetValue(RunningProperty, value); }
        }
        private void Keywords_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Results.Clear();
                if (!string.IsNullOrWhiteSpace(Url) && !string.IsNullOrWhiteSpace(Keyword))
                {
                    Search(Url, Keyword);
                }
            }
        }
        private void Search(string url, string keyword)
        {
            Running = true;
            Task.Run(() =>
            {
                string input = FileReader.ReadFileContent(url, Encoding.UTF8);
                int start = 0, offset, find;
                string line;
                while ((offset = NewLineHelper.FindNextNewLine(input, start, out string newline)) > -1)
                {
                    line = input.Substring(start, offset + newline.Length - start);
                    if ((find = IsMatch(line, keyword)) > -1)
                    {
                        LoadResult(start + find, line.Trim());
                    }
                    start = offset + newline.Length;
                }
                line = input.Substring(start, input.Length - start);
                if ((find = IsMatch(line, keyword)) > -1)
                {
                    LoadResult(start + find, line.Trim());
                }

                this.Dispatcher.BeginInvoke(
                    DispatcherPriority.Normal,
                    new Action(() => {
                        Running = false;
                    })
                );                
            });
        }
        private int IsMatch(string line, string key)
        {
            return line.IndexOf(key);
        }
        private void LoadResult(int offset, string line)
        {
            this.Dispatcher.BeginInvoke(
                DispatcherPriority.Normal,
                new Action(() => {
                    Results.Add(new BookmarkInfo() { Offset = offset, Text = line });
                })
            );
        }
        private void ToSearchReultHandle(object sender, ExecutedRoutedEventArgs e)
        {
            View_Bookmark.SelectedItem = e.Parameter;
            CommandParameter = e.Parameter;
            Command.Execute(CommandParameter);
        }
        private static void OnUrlChanged(DependencyObject obj, DependencyPropertyChangedEventArgs r)
        {
            var c = (USearcher)obj;
            c.Clear();
        }
        public void Clear()
        {
            Results.Clear();
        }
    }
}
