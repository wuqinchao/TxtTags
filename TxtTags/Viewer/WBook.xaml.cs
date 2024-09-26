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
using UtfUnknown;

namespace TxtTags
{
    /// <summary>
    /// WBook.xaml 的交互逻辑
    /// </summary>
    public partial class WBook : HandyControl.Controls.Window
    {
        public static readonly DependencyProperty DataSourceProperty =
                    DependencyProperty.Register(
                        "DataSource",
                        typeof(TagFileInfo),
                        typeof(WBook),
                        new FrameworkPropertyMetadata(null, OnDataSourceChanged));
        /// <summary>
        /// MODBUS配置
        /// </summary>
        public TagFileInfo DataSource
        {
            get { return (TagFileInfo)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }
        public WBook()
        {
            InitializeComponent();
        }
        private static void OnDataSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs r)
        {
            var c = (WBook)obj;
            c.DetectEncoding();
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
            }
        }
    }
}
