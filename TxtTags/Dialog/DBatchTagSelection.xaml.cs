using HandyControl.Controls;
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
using System.Windows.Shapes;
using TxtTags.Common;

namespace TxtTags.Dialog
{
    /// <summary>
    /// DTagSelection.xaml 的交互逻辑
    /// </summary>
    public partial class DBatchTagSelection : HandyControl.Controls.Window
    {
        public static RoutedUICommand OkCmd = new RoutedUICommand("OkCmd",
                "OkCmd", typeof(DBatchTagSelection));
        public static RoutedUICommand CancelCmd = new RoutedUICommand("CancelCmd",
                "CancelCmd", typeof(DBatchTagSelection));
        public static readonly DependencyProperty SelectionsProperty =
                    DependencyProperty.Register(
                        "Selections",
                        typeof(ObservableCollection<EditableTag>),
                        typeof(DBatchTagSelection),
                        new FrameworkPropertyMetadata(new ObservableCollection<EditableTag>()));
        public ObservableCollection<EditableTag> Selections
        {
            get { return (ObservableCollection<EditableTag>)GetValue(SelectionsProperty); }
            set { SetValue(SelectionsProperty, value); }
        }
        public int TagId { get; set; }
        public List<TagFileInfo> TagFile { get; set; }
        public DBatchTagSelection()
        {
            this.DataContext = this;
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(OkCmd, OkHandle));
            CommandBindings.Add(new CommandBinding(CancelCmd, CancelHandle));
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var all = BookTags.Instance.Items.First(x=>x.Id == TagId);
            Selections.Clear();
            foreach (var item in all.Tags)
            {
                Selections.Add(new EditableTag() { Text = item, Older = false, Checked = false });
            }
        }

        private void CancelHandle(object sender, ExecutedRoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void OkHandle(object sender, ExecutedRoutedEventArgs e)
        {
            foreach(var f in TagFile)
            {
                bool changed = false;
                foreach (var t in Selections)
                {
                    if (t.Checked && !t.Older)
                    {
                        if (!f.Tags.Contains(t.Text))
                        {
                            changed = true;
                            f.Tags.Add(t.Text);
                            f.CategoryTags.First(x => x.Id == TagId).Tags.Add(t.Text);
                        }
                    }
                }
                if (changed)
                {
                    f.Rename();
                }
            }
            
            this.DialogResult = true;
            this.Close();
        }
    }
}
