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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TxtTags.Common;

namespace TxtTags
{
    /// <summary>
    /// UIcons.xaml 的交互逻辑
    /// </summary>
    public partial class UIcons : UserControl
    {
        public static RoutedUICommand CopyCmd = new RoutedUICommand("CopyCmd",
                "CopyCmd", typeof(UIcons));

        public static readonly DependencyProperty GeometryItemsProperty =
            DependencyProperty.Register("GeometryItems", typeof(ObservableCollection<GeometryItemModel>), typeof(UBookView), new FrameworkPropertyMetadata(new ObservableCollection<GeometryItemModel>()));
        public ObservableCollection<GeometryItemModel> GeometryItems
        {
            get { return (ObservableCollection<GeometryItemModel>)GetValue(GeometryItemsProperty); }
            set { SetValue(GeometryItemsProperty, value); }
        }
        public UIcons()
        {
            this.DataContext = this;
            GenerateGeometries();
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(CopyCmd, CopyHandle));
        }
        private void CopyHandle(object sender, ExecutedRoutedEventArgs e)
        {
            Clipboard.SetDataObject(e.Parameter.ToString());
        }
        public void GenerateGeometries()
        {
            foreach (var key in Application.Current.Resources.MergedDictionaries[3].Keys.OfType<string>().OrderBy(item => item))
            {
                if (!key.EndsWith("Geometry")) continue;
                GeometryItems.Add(new GeometryItemModel
                {
                    Key = key,
                    Data = ResourceHelper.GetResource<Geometry>(key)
                });
            }
        }
    }
}
