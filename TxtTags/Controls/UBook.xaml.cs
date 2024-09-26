using ICSharpCode.AvalonEdit;
using System;
using System.Collections.Generic;
using System.IO;
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
using TxtTags.Pages;
using UtfUnknown;

namespace TxtTags
{
    /// <summary>
    /// UBook.xaml 的交互逻辑
    /// </summary>
    public partial class UBook : UserControl
    {
        public static RoutedUICommand EditTagsCmd = new RoutedUICommand("EditTagsCmd",
                "EditTagsCmd", typeof(UBook));
        public static RoutedUICommand ToEncodingCmd = new RoutedUICommand("ToEncodingCmd",
                "ToEncodingCmd", typeof(UBook));
        public static RoutedUICommand ToChinaCmd = new RoutedUICommand("ToChinaCmd",
                "ToChinaCmd", typeof(UBook));
        public static RoutedUICommand MakeBookmarkCmd = new RoutedUICommand("MakeBookmarkCmd",
                "MakeBookmarkCmd", typeof(UBook));

        public static readonly DependencyProperty DataSourceProperty =
                    DependencyProperty.Register(
                        "DataSource",
                        typeof(TagFileInfo),
                        typeof(UBook),
                        new FrameworkPropertyMetadata(null, OnDataSourceChanged));
        /// <summary>
        /// MODBUS配置
        /// </summary>
        public TagFileInfo DataSource
        {
            get { return (TagFileInfo)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }
        public static readonly DependencyProperty TagsProperty =
                    DependencyProperty.Register(
                        "Tags",
                        typeof(BookTags),
                        typeof(UBook),
                        new FrameworkPropertyMetadata(BookTags.Instance));
        public BookTags Tags
        {
            get { return (BookTags)GetValue(TagsProperty); }
            set { SetValue(TagsProperty, value); }
        }
        public static readonly DependencyProperty PreviewProperty =
                    DependencyProperty.Register(
                        "Preview",
                        typeof(string),
                        typeof(UBook),
                        new FrameworkPropertyMetadata(""));
        public string Preview
        {
            get { return (string)GetValue(PreviewProperty); }
            set { SetValue(PreviewProperty, value); }
        }
        public static readonly DependencyProperty ConfigerProperty =
                    DependencyProperty.Register(
                        "Configer",
                        typeof(Config),
                        typeof(UBook),
                        new FrameworkPropertyMetadata(Config.Instance));
        public Config Configer
        {
            get { return (Config)GetValue(ConfigerProperty); }
            set { SetValue(ConfigerProperty, value); }
        }
        public UBook()
        {
            InitializeComponent();
            //ICSharpCode.AvalonEdit.Search.SearchPanel.Install(TxtPreview);
            //TxtPreview.TextArea.TextView.LineSpacing = 1.5d;
            CommandBindings.Add(new CommandBinding(EditTagsCmd, EditTagsHandle));
            CommandBindings.Add(new CommandBinding(ToEncodingCmd, ToEncodingHandle));
            CommandBindings.Add(new CommandBinding(ToChinaCmd, ToChinaHandle));
            CommandBindings.Add(new CommandBinding(MakeBookmarkCmd, MakeBookmarkHandle));
        }
        private void MakeBookmarkHandle(object sender, ExecutedRoutedEventArgs e)
        {
            //HandyControl.Controls.Growl.Info($"CaretOffset: {TxtPreview.CaretOffset}");
        }
        private void ToChinaHandle(object sender, ExecutedRoutedEventArgs e)
        {
            var fs = new List<TagFileInfo>() { DataSource };
            DFileEncoding w = new DFileEncoding(fs, null);
            w.ShowDialog();
        }
        private void ToEncodingHandle(object sender, ExecutedRoutedEventArgs e)
        {
            var encoding = e.Parameter.ToString() == "UTF8" ? Encoding.UTF8:Encoding.GetEncoding(54936);
            var fs = new List<TagFileInfo>() { DataSource };
            DFileEncoding w = new DFileEncoding(fs, encoding);
            w.ShowDialog();
        }
        private void EditTagsHandle(object sender, ExecutedRoutedEventArgs e)
        {
            //category id, e.Parameter
            var id = (int)e.Parameter;
            var t = DataSource.CategoryTags.First(x => x.Id == id);
            DTagSelection w = new DTagSelection();
            w.TagId = id;
            w.TagFile = DataSource;
            w.ShowDialog();
        }
        private void OnFavChanged(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded) return;
            if (DataSource == null) return;
            if(DataSource.Fav != DataSource.File.Fav)
            {
                DataSource.Rename();
            }
        }
        private void OnStarChanged(object sender, HandyControl.Data.FunctionEventArgs<double> e)
        {
            if (!IsLoaded) return;
            if (DataSource == null) return;
            if (DataSource.Star != e.Info)
            {
                DataSource.Star = (int)e.Info;
                DataSource.Rename();
            }
        }

        private void UStar_ValueChanged(object sender, StarChangedEventArgs e)
        {
            if (!IsLoaded) return;
            if (DataSource == null) return;
            if (DataSource.Star != e.Star)
            {
                DataSource.Star = (int)e.Star;
                DataSource.Rename();
            }
        }
        public void DetectEncoding()
        {
            if(DataSource != null)
            {
                try
                {
                    DataSource.Encoding = CharsetDetector.DetectFromFile(DataSource.File.FullPath).Detected.Encoding;
                }
                catch
                {
                    DataSource.Encoding = null;
                }
                //LoadPreview(DataSource.File.FullPath, DataSource.Encoding);
            }
        }
        private static void OnDataSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs r)
        {
            var c = (UBook)obj;
            c.DetectEncoding();
        }
        public void Reload()
        {
            if (DataSource != null)
            {
                BookView.LoadBook();
            }
        }
    }
    public class TagCategoryShowConvert : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (values == null || values.Length < 2)
                {
                    return Visibility.Collapsed;
                }

                return ((BookTag)values[0]).Hide && Config.Instance.Locked ? Visibility.Collapsed : Visibility.Visible;
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
