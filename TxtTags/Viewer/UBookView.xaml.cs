using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TxtTags.Common;
using TxtTags.Entity;
using TxtTags.Service;

namespace TxtTags
{
    /// <summary>
    /// UBookView.xaml 的交互逻辑
    /// </summary>
    public partial class UBookView : System.Windows.Controls.UserControl
    {
        public static RoutedUICommand FontSizeCmd = new RoutedUICommand("FontSizeCmd",
                "FontSizeCmd", typeof(UBookView));
        public static RoutedUICommand LineHeightCmd = new RoutedUICommand("LineHeightCmd",
                "LineHeightCmd", typeof(UBookView));
        public static RoutedUICommand SaveCmd = new RoutedUICommand("SaveCmd",
                "SaveCmd", typeof(UBookView));
        public static RoutedUICommand ToTopCmd = new RoutedUICommand("ToTopCmd",
                "ToTopCmd", typeof(UBookView));
        public static RoutedUICommand ToTailCmd = new RoutedUICommand("ToTailCmd",
                "ToTailCmd", typeof(UBookView));
        public static RoutedUICommand UndoCmd = new RoutedUICommand("UndoCmd",
                "UndoCmd", typeof(UBookView));
        public static RoutedUICommand RedoCmd = new RoutedUICommand("RedoCmd",
                "RedoCmd", typeof(UBookView));
        public static RoutedUICommand PageDownCmd = new RoutedUICommand("PageDownCmd",
                "PageDownCmd", typeof(UBookView));
        public static RoutedUICommand PageUpCmd = new RoutedUICommand("PageUpCmd",
                "PageUpCmd", typeof(UBookView));
        //public static RoutedUICommand OpenCatalogCmd = new RoutedUICommand("OpenCatalogCmd",
        //        "OpenCatalogCmd", typeof(UBookView));
        public static RoutedUICommand ToCatalogCmd = new RoutedUICommand("ToCatalogCmd",
                "ToCatalogCmd", typeof(UBookView));
        public static RoutedUICommand ToBookmarkCmd = new RoutedUICommand("ToBookmarkCmd",
                "ToBookmarkCmd", typeof(UBookView));
        public static RoutedUICommand ToSearchResultCmd = new RoutedUICommand("ToSearchResultCmd",
                "ToSearchResultCmd", typeof(UBookView));
        public static RoutedUICommand DelBookmarkCmd = new RoutedUICommand("DelBookmarkCmd",
                "DelBookmarkCmd", typeof(UBookView));
        public static RoutedUICommand MakeBookmarkCmd = new RoutedUICommand("MakeBookmarkCmd",
                "MakeBookmarkCmd", typeof(UBookView));
        public static RoutedUICommand RefreshCmd = new RoutedUICommand("RefreshCmd",
                "RefreshCmd", typeof(UBookView));
        public static RoutedUICommand NormalizeCmd = new RoutedUICommand("NormalizeCmd",
                "NormalizeCmd", typeof(UBookView));
        public static RoutedUICommand SelectForeColorCmd = new RoutedUICommand("SelectForeColorCmd",
                "SelectForeColorCmd", typeof(UBookView));
        public static RoutedUICommand SelectBackColorCmd = new RoutedUICommand("SelectBackColorCmd",
                "SelectBackColorCmd", typeof(UBookView));

        public static readonly DependencyProperty UrlProperty =
            DependencyProperty.Register("Url", typeof(string), typeof(UBookView), new FrameworkPropertyMetadata(null, OnUrlChanged));
        public string Url
        {
            get { return (string)GetValue(UrlProperty); }
            set { SetValue(UrlProperty, value); }
        }
        public static readonly DependencyProperty IsReadonlyProperty =
            DependencyProperty.Register("IsReadonly", typeof(bool), typeof(UBookView), new FrameworkPropertyMetadata(true));
        public bool IsReadonly
        {
            get { return (bool)GetValue(IsReadonlyProperty); }
            set { SetValue(IsReadonlyProperty, value); }
        }
        public static readonly DependencyProperty RunningProperty =
            DependencyProperty.Register("Running", typeof(bool), typeof(UBookView), new FrameworkPropertyMetadata(false));
        public bool Running
        {
            get { return (bool)GetValue(RunningProperty); }
            set { SetValue(RunningProperty, value); }
        }
        public static readonly DependencyProperty CatalogOpenedProperty =
            DependencyProperty.Register("CatalogOpened", typeof(bool), typeof(UBookView), new FrameworkPropertyMetadata(true));
        public bool CatalogOpened
        {
            get { return (bool)GetValue(CatalogOpenedProperty); }
            set { SetValue(CatalogOpenedProperty, value); }
        }
        public static readonly DependencyProperty CatalogReadyProperty =
            DependencyProperty.Register("CatalogReady", typeof(bool), typeof(UBookView), new FrameworkPropertyMetadata(true));
        public bool CatalogReady
        {
            get { return (bool)GetValue(CatalogReadyProperty); }
            set { SetValue(CatalogReadyProperty, value); }
        }
        public static readonly DependencyProperty BookmarkOpenedProperty =
            DependencyProperty.Register("BookmarkOpened", typeof(bool), typeof(UBookView), new FrameworkPropertyMetadata(true));
        public bool BookmarkOpened
        {
            get { return (bool)GetValue(BookmarkOpenedProperty); }
            set { SetValue(BookmarkOpenedProperty, value); }
        }
        //public static readonly DependencyProperty LineHeightProperty =
        //    DependencyProperty.Register("LineHeight", typeof(double), typeof(UBookView), new FrameworkPropertyMetadata(1.5d, OnLineHeightChanged));
        //public double LineHeight
        //{
        //    get { return (double)GetValue(LineHeightProperty); }
        //    set { SetValue(LineHeightProperty, value); }
        //}
        //public static readonly DependencyProperty DocFontSizeProperty =
        //    DependencyProperty.Register("DocFontSize", typeof(double), typeof(UBookView), new FrameworkPropertyMetadata(16d));
        //public double DocFontSize
        //{
        //    get { return (double)GetValue(DocFontSizeProperty); }
        //    set { SetValue(DocFontSizeProperty, value); }
        //}
        //public static readonly DependencyProperty FontColorProperty = 
        //    DependencyProperty.Register("FontColor", typeof(SolidColorBrush), typeof(UBookView), new UIPropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333333"))));
        //public SolidColorBrush FontColor
        //{
        //    get { return (SolidColorBrush)GetValue(FontColorProperty); }
        //    set { SetValue(FontColorProperty, value); }
        //}
        //public static readonly DependencyProperty FontBackgroundProperty =
        //    DependencyProperty.Register("FontBackground", typeof(SolidColorBrush), typeof(UBookView), new UIPropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CDFAE7"))));
        //public SolidColorBrush FontBackground
        //{
        //    get { return (SolidColorBrush)GetValue(FontBackgroundProperty); }
        //    set { SetValue(FontBackgroundProperty, value); }
        //}
        public static readonly DependencyProperty EncodingProperty =
            DependencyProperty.Register("Encoding", typeof(System.Text.Encoding), typeof(UBookView), new UIPropertyMetadata(null));
        public System.Text.Encoding Encoding
        {
            get { return (System.Text.Encoding)GetValue(EncodingProperty); }
            set { SetValue(EncodingProperty, value); }
        }
        public static readonly DependencyProperty CatalogProperty =
            DependencyProperty.Register("Catalog", typeof(ObservableCollection<CatalogInfo>), typeof(UBookView), new UIPropertyMetadata(new ObservableCollection<CatalogInfo>()));
        public ObservableCollection<CatalogInfo> Catalog
        {
            get { return (ObservableCollection<CatalogInfo>)GetValue(CatalogProperty); }
            set { SetValue(CatalogProperty, value); }
        }
        public static readonly DependencyProperty BookmarkProperty =
            DependencyProperty.Register("Bookmark", typeof(BookmarkService), typeof(UBookView), new UIPropertyMetadata(null));
        public BookmarkService Bookmark
        {
            get { return (BookmarkService)GetValue(BookmarkProperty); }
            set { SetValue(BookmarkProperty, value); }
        }
        public static readonly DependencyProperty NormalizeOptionProperty =
            DependencyProperty.Register("NormalizeOption", typeof(NormalizeOption), typeof(UBookView), new UIPropertyMetadata(new NormalizeOption()));
        public NormalizeOption NormalizeOption
        {
            get { return (NormalizeOption)GetValue(NormalizeOptionProperty); }
            set { SetValue(NormalizeOptionProperty, value); }
        }
        public static readonly DependencyProperty ConfigerProperty =
                    DependencyProperty.Register("Configer", typeof(Config), typeof(UBookView), new FrameworkPropertyMetadata(Config.Instance));
        public Config Configer
        {
            get { return (Config)GetValue(ConfigerProperty); }
            set { SetValue(ConfigerProperty, value); }
        }
        public UBookView()
        {
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(FontSizeCmd, FontSizeHandle));
            CommandBindings.Add(new CommandBinding(LineHeightCmd, LineHeightHandle));
            CommandBindings.Add(new CommandBinding(SaveCmd, SaveHandle));
            CommandBindings.Add(new CommandBinding(ToTopCmd, ToTopHandle));
            CommandBindings.Add(new CommandBinding(ToTailCmd, ToTailHandle));
            CommandBindings.Add(new CommandBinding(UndoCmd, UndoHandle));
            CommandBindings.Add(new CommandBinding(RedoCmd, RedoHandle));
            CommandBindings.Add(new CommandBinding(PageUpCmd, PageUpHandle));
            CommandBindings.Add(new CommandBinding(PageDownCmd, PageDownHandle));
            CommandBindings.Add(new CommandBinding(ToCatalogCmd, ToCatalogHandle));
            //CommandBindings.Add(new CommandBinding(OpenCatalogCmd, OpenCatalogHandle));
            CommandBindings.Add(new CommandBinding(MakeBookmarkCmd, MakeBookmarkHandle));
            CommandBindings.Add(new CommandBinding(ToBookmarkCmd, ToBookmarkHandle));
            CommandBindings.Add(new CommandBinding(ToSearchResultCmd, ToSearchResultHandle));
            CommandBindings.Add(new CommandBinding(DelBookmarkCmd, DelBookmarkHandle));
            CommandBindings.Add(new CommandBinding(RefreshCmd, RefreshHandle));
            CommandBindings.Add(new CommandBinding(NormalizeCmd, NormalizeHandle));
            CommandBindings.Add(new CommandBinding(SelectForeColorCmd, SelectForeColorHandle));
            CommandBindings.Add(new CommandBinding(SelectBackColorCmd, SelectBackColorHandle));
            ICSharpCode.AvalonEdit.Search.SearchPanel.Install(TxtPreview);
            TxtPreview.TextArea.TextView.AfterEnsureVisualLines += TextView_LayoutUpdated;  
            //TxtPreview.TextArea.TextView.LayoutUpdated += TextView_LayoutUpdated;
            //TxtPreview.LayoutUpdated+= TextView_LayoutUpdated;
        }
        private void TextView_LayoutUpdated(object sender, EventArgs e)
        {
            //Console.WriteLine("AfterEnsureVisualLines");
            ResetCatalog();
        }
        public static System.Windows.Media.Color GetMediaColorFromDrawingColor(System.Drawing.Color color)
        {
            return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
        public static System.Drawing.Color GetDrawingColorFromMediaColor(System.Windows.Media.Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
        private void SelectForeColorHandle(object sender, ExecutedRoutedEventArgs e)
        {
            //创建对象
            ColorDialog colorDialog = new ColorDialog();
            //允许使用该对话框的自定义颜色  
            colorDialog.AllowFullOpen = true;
            colorDialog.FullOpen = true;
            colorDialog.ShowHelp = true;
            //初始化当前文本框中的字体颜色，  
            colorDialog.Color = GetDrawingColorFromMediaColor(Configer.ViewerForceColor.Color);

            //当用户在ColorDialog对话框中点击"确定"按钮  
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Configer.ViewerForceColor = new SolidColorBrush(GetMediaColorFromDrawingColor(colorDialog.Color));
            }
        }
        private void SelectBackColorHandle(object sender, ExecutedRoutedEventArgs e)
        {
            //创建对象
            ColorDialog colorDialog = new ColorDialog();
            //允许使用该对话框的自定义颜色  
            colorDialog.AllowFullOpen = true;
            colorDialog.FullOpen = true;
            colorDialog.ShowHelp = true;
            //初始化当前文本框中的字体颜色，  
            colorDialog.Color = GetDrawingColorFromMediaColor(Configer.ViewerBackground.Color);

            //当用户在ColorDialog对话框中点击"确定"按钮  
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Configer.ViewerBackground = new SolidColorBrush(GetMediaColorFromDrawingColor(colorDialog.Color));
            }
        }
        private void ToTopHandle(object sender, ExecutedRoutedEventArgs e)
        {
            TxtPreview.ScrollToHome();
        }
        private void ToTailHandle(object sender, ExecutedRoutedEventArgs e)
        {
            TxtPreview.ScrollToEnd();
        }
        private void UndoHandle(object sender, ExecutedRoutedEventArgs e)
        {
            if(TxtPreview.CanUndo)
            {
                TxtPreview.Undo();
            }
        }
        private void RedoHandle(object sender, ExecutedRoutedEventArgs e)
        {
            if (TxtPreview.CanRedo)
            {
                TxtPreview.Redo();
            }
        }
        private void RefreshHandle(object sender, ExecutedRoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Url) && System.IO.File.Exists(Url))
                LoadBook();
        }
        private void MakeBookmarkHandle(object sender, ExecutedRoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TxtPreview.SelectedText))
            {
                Bookmark.Add(new BookmarkInfo() { Offset = TxtPreview.SelectionStart, Text = TxtPreview.SelectedText });
            }
            else
            {
                HandyControl.Controls.Growl.Info("请选择书签文本");
            }
        }
        private void DelBookmarkHandle(object sender, ExecutedRoutedEventArgs e)
        {
            if (View_Bookmark.SelectedItem != null)
            {
                var mark = (BookmarkInfo)(View_Bookmark.SelectedItem);
                Bookmark.Remove(mark.Offset);
            }
        }
        private void ToCatalogHandle(object sender, ExecutedRoutedEventArgs e)
        {
            var catalog = (CatalogInfo)e.Parameter;
            var r = TxtPreview.Document.GetLocation(catalog.Offset);
            TxtPreview.CaretOffset = Convert.ToInt32(catalog.Offset);
            TxtPreview.Select(catalog.Offset, catalog.Text.Length);
            TxtPreview.ScrollTo(r.Line, r.Column);
            TxtPreview.Focus();
            View_Catalog.SelectedItem = catalog;
        }
        private void ToBookmarkHandle(object sender, ExecutedRoutedEventArgs e)
        {
            var mark = (BookmarkInfo)e.Parameter;
            var r = TxtPreview.Document.GetLocation(mark.Offset);
            TxtPreview.CaretOffset = Convert.ToInt32(mark.Offset);
            TxtPreview.Select(mark.Offset, mark.Text.Length);
            TxtPreview.ScrollTo(r.Line, r.Column);
            TxtPreview.Focus();
            View_Bookmark.SelectedItem = mark;
        }
        private void ToSearchResultHandle(object sender, ExecutedRoutedEventArgs e)
        {
            var mark = (BookmarkInfo)e.Parameter;
            var r = TxtPreview.Document.GetLocation(mark.Offset);
            TxtPreview.CaretOffset = Convert.ToInt32(mark.Offset);
            TxtPreview.Select(mark.Offset, mark.Text.Length);
            TxtPreview.ScrollTo(r.Line, r.Column);
            TxtPreview.Focus();
        }
        private void PageUpHandle(object sender, ExecutedRoutedEventArgs e)
        {
            TxtPreview.PageUp();
            ResetCatalog();
        }
        private void PageDownHandle(object sender, ExecutedRoutedEventArgs e)
        {
            TxtPreview.PageDown();
            ResetCatalog();
        }
        private void SaveHandle(object sender, ExecutedRoutedEventArgs e)
        {
            TxtPreview.Save(Url);
        }
        private void FontSizeHandle(object sender, ExecutedRoutedEventArgs e)
        {
            Configer.ViewerFontSize = (double)e.Parameter;
        }
        private void LineHeightHandle(object sender, ExecutedRoutedEventArgs e)
        {
            Configer.ViewerLineHeight = (double)e.Parameter;
        }
        //private static void OnLineHeightChanged(DependencyObject obj, DependencyPropertyChangedEventArgs r)
        //{
        //    var c = (UBookView)obj;
        //    c.SetLineHeight();
        //}
        private static void OnUrlChanged(DependencyObject obj, DependencyPropertyChangedEventArgs r)
        {
            var c = (UBookView)obj;
            c.LoadBook();
        }
        public void LoadBook()
        {
            if (!string.IsNullOrWhiteSpace(Url) && System.IO.File.Exists(Url))
            {
                //Catalog.Clear();
                TxtPreview.Load(Url);
                TxtPreview.ScrollToHome();
                Bookmark = new BookmarkService(Url);
                NormalizeOption.Encoding = this.Encoding;
                NormalizeOption.Full = this.Url;
                NormalizeOption.Name = TagFileInfo.GetOrgName(System.IO.Path.GetFileName(this.Url));
                LoadCatalog();
            }
        }
        public void SetLineHeight()
        {
            TxtPreview.TextArea.TextView.LineSpacing = Configer.ViewerLineHeight;
            TxtPreview.TextArea.TextView.Redraw();
        }
        private void ResetCatalog()
        {
            var line = TxtPreview.TextArea.TextView.GetVisualLineFromVisualTop(TxtPreview.TextArea.TextView.ScrollOffset.Y);
            if(line != null)
            {
                var offset = line.StartOffset;
                if (Catalog.Count > 0)
                {
                    CatalogInfo pre;
                    for (int i = 1; i < Catalog.Count; i++)
                    {
                        pre = Catalog[i - 1];
                        if (offset >= pre.Offset && offset < Catalog[i].Offset)
                        {
                            View_Catalog.SelectedItem = pre;
                            View_Catalog.ScrollIntoView(pre);
                        }
                    }
                }
                if (Bookmark != null && Bookmark.Marks != null && Bookmark.Marks.Count > 0)
                {
                    BookmarkInfo pre;
                    for (int i = 1; i < Bookmark.Marks.Count; i++)
                    {
                        pre = Bookmark.Marks[i - 1];
                        if (offset >= pre.Offset && offset < Bookmark.Marks[i].Offset)
                        {
                            View_Bookmark.SelectedItem = pre;
                            View_Bookmark.ScrollIntoView(pre);
                        }
                    }
                }
            }
            //var p = TxtPreview.TextArea.TextView.GetPosition(new Point(0, TxtPreview.TextArea.TextView.ScrollOffset.Y));
            //if (p.HasValue)
            //{
            //    var offset = TxtPreview.Document.GetOffset(p.Value.Line, 0);
            //    if (Catalog.Count > 0)
            //    {
            //        CatalogInfo pre;
            //        for (int i = 1; i < Catalog.Count; i++)
            //        {
            //            pre = Catalog[i - 1];
            //            if (offset >= pre.Offset && offset < Catalog[i].Offset)
            //            {
            //                View_Catalog.SelectedItem = pre;
            //                View_Catalog.ScrollIntoView(pre);
            //            }
            //        }
            //    }
            //    if (Bookmark != null && Bookmark.Marks != null && Bookmark.Marks.Count > 0)
            //    {
            //        BookmarkInfo pre;
            //        for (int i = 1; i < Bookmark.Marks.Count; i++)
            //        {
            //            pre = Bookmark.Marks[i - 1];
            //            if (offset >= pre.Offset && offset < Bookmark.Marks[i].Offset)
            //            {
            //                View_Bookmark.SelectedItem = pre;
            //                View_Bookmark.ScrollIntoView(pre);
            //            }
            //        }
            //    }
            //}
        }
        private void FontSize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //if (DocFontSize != e.NewValue)
            //    DocFontSize = (double)e.NewValue;
        }
        private void LineHeight_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (TxtPreview == null) return;
            SetLineHeight();
        }
        private void NormalizeHandle(object sender, ExecutedRoutedEventArgs e)
        {
            if(!System.IO.File.Exists(this.Url))
            {
                HandyControl.Controls.Growl.Info("文件不存在");
                return;
            }
            if(!NormalizeOption.IsValid())
            {
                HandyControl.Controls.Growl.Info("请至少选择一个整理项");
                return;
            }
            NormalizeBtn.IsChecked = false;
            Running = true;
            //var fo = new NormalizeOption()
            //{
            //    Encoding = this.Encoding,
            //    Full = this.Url,
            //    Name = TagFileInfo.GetOrgName(System.IO.Path.GetFileName(this.Url))
            //};
            NormalizeServiceCompleted handle = null;
            NormalizeHelper.Instance.OnWorkerCompleted += handle = (service, result) =>
            {
                NormalizeHelper.Instance.OnWorkerCompleted -= handle;
                Running = false;
                if (result.OK)
                {
                    LoadBook();
                }
                else
                {
                    HandyControl.Controls.Growl.Info(new HandyControl.Data.GrowlInfo() { Message = result.Message, StaysOpen = false });
                }
            };
            if(!NormalizeHelper.Instance.Do(NormalizeOption))
            {
                Running = false;
                HandyControl.Controls.Growl.Info("线程忙,请稍后再试");
            }
        }
        private void LoadCatalog()
        {
            if (!System.IO.File.Exists(this.Url))
            {
                HandyControl.Controls.Growl.Info("文件不存在");
                return;
            }
            //TxtPreview.CaretOffset = 100;
            Catalog.Clear();
            var fo = new CatalogServiceOption()
            {
                Encoding = this.Encoding,
                Full = this.Url,
                Name = TagFileInfo.GetOrgName(this.Url)
            };
            CatalogServiceCompleted handle = null;
            CatalogHelper.Instance.OnWorkerCompleted += handle = (service, result) =>
            {
                CatalogHelper.Instance.OnWorkerCompleted -= handle;
                if (result.OK)
                {
                    if(result.Data.Count>0)
                    {
                        foreach(var cl in result.Data)
                        {
                            Catalog.Add(cl);
                        }
                    }
                    if (View_Catalog.Items != null && View_Catalog.Items.Count > 0)
                        View_Catalog.ScrollIntoView(View_Catalog.Items[0]);
                }
                else
                {
                    HandyControl.Controls.Growl.Info(new HandyControl.Data.GrowlInfo() { Message = result.Message, StaysOpen = false });
                }
            };
            CatalogHelper.Instance.Do(fo);
        }
        private void UBookView_Loaded(object sender, RoutedEventArgs e)
        {
            CatalogOpened = false;
            //OpenCatalogHandle(null, null);
            BookmarkOpened = false;
            //OpenBookmarkHandle(null, null);
            SetLineHeight();
        }
    }
}
