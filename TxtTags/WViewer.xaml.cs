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
using UtfUnknown;

namespace TxtTags
{
    /// <summary>
    /// WViewer.xaml 的交互逻辑
    /// </summary>
    public partial class WViewer : HandyControl.Controls.Window
    {
        public static readonly DependencyProperty DataSourceProperty =
                    DependencyProperty.Register(
                        "DataSource",
                        typeof(TagFileInfo),
                        typeof(WViewer),
                        new FrameworkPropertyMetadata(null, OnDataSourceChanged));
        public TagFileInfo DataSource
        {
            get { return (TagFileInfo)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }
        public static readonly DependencyProperty ColumnGapProperty =
                    DependencyProperty.Register(
                        "ColumnGap",
                        typeof(double),
                        typeof(WViewer),
                        new FrameworkPropertyMetadata(20d));
        public double ColumnGap
        {
            get { return (double)GetValue(ColumnGapProperty); }
            set { SetValue(ColumnGapProperty, value); }
        }
        public static readonly DependencyProperty ColumnRuleWidthProperty =
                    DependencyProperty.Register(
                        "ColumnRuleWidth",
                        typeof(double),
                        typeof(WViewer),
                        new FrameworkPropertyMetadata(5d));
        public double ColumnRuleWidth
        {
            get { return (double)GetValue(ColumnRuleWidthProperty); }
            set { SetValue(ColumnRuleWidthProperty, value); }
        }
        public static readonly DependencyProperty ColumnWidthProperty =
                    DependencyProperty.Register(
                        "ColumnWidth",
                        typeof(double),
                        typeof(WViewer),
                        new FrameworkPropertyMetadata(800d));
        public double ColumnWidth
        {
            get { return (double)GetValue(ColumnWidthProperty); }
            set { SetValue(ColumnWidthProperty, value); }
        }
        public static readonly DependencyProperty LineHeightValueProperty =
                    DependencyProperty.Register(
                        "LineHeightValue",
                        typeof(double),
                        typeof(WViewer),
                        new FrameworkPropertyMetadata(30d));
        public double LineHeightValue
        {
            get { return (double)GetValue(LineHeightValueProperty); }
            set { SetValue(LineHeightValueProperty, value); }
        }
        public static readonly DependencyProperty LineHeightProperty =
                    DependencyProperty.Register(
                        "LineHeight",
                        typeof(double),
                        typeof(WViewer),
                        new FrameworkPropertyMetadata(1.5d, OnLineHeightChanged));
        public double LineHeight
        {
            get { return (double)GetValue(LineHeightProperty); }
            set { SetValue(LineHeightProperty, value); }
        }
        public static readonly DependencyProperty DocFontSizeProperty =
                    DependencyProperty.Register(
                        "DocFontSize",
                        typeof(double),
                        typeof(WViewer),
                        new FrameworkPropertyMetadata(20d, OnDocFontSizeChanged));
        public double DocFontSize
        {
            get { return (double)GetValue(DocFontSizeProperty); }
            set { SetValue(DocFontSizeProperty, value); }
        }
        public WViewer()
        {
            this.DataContext = this;
            InitializeComponent();
        }
        private static void OnDataSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs r)
        {
            var c = (WViewer)obj;
            c.DetectEncoding();
        }
        private static void OnLineHeightChanged(DependencyObject obj, DependencyPropertyChangedEventArgs r)
        {
            var c = (WViewer)obj;
            c.LineHeightValue = c.DocFontSize * c.LineHeight;
        }
        private static void OnDocFontSizeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs r)
        {
            var c = (WViewer)obj;
            c.LineHeightValue = c.DocFontSize * c.LineHeight;
        }
        public void DetectEncoding()
        {
            if (DataSource != null)
            {
                this.Title = DataSource.OrgName;
                try
                {
                    DataSource.Encoding = CharsetDetector.DetectFromFile(DataSource.File.FullPath).Detected.Encoding;
                }
                catch
                {
                    DataSource.Encoding = null;
                }
                
                LoadPreview(DataSource.File.FullPath, DataSource.Encoding);
            }
        }
        public void LoadPreview(string filepath, Encoding encoding)
        {
            //Paragraph paragraph = new Paragraph();
            //if (encoding != null)
            //{
            //    paragraph.Inlines.Add(System.IO.File.ReadAllText(filepath, encoding));
            //}
            //else
            //{
            //    paragraph.Inlines.Add(System.IO.File.ReadAllText(filepath));
            //}
            //FlowDocument document = new FlowDocument(paragraph);
            //FlowDocReader.Document = document;
            PreParagraph.Inlines.Clear();
            if (encoding != null)
            {
                PreParagraph.Inlines.Add(System.IO.File.ReadAllText(filepath, encoding));
            }
            else
            {
                PreParagraph.Inlines.Add(System.IO.File.ReadAllText(filepath));
            }
        }
    }
}
