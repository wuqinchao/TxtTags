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
    public partial class DTagSelection : HandyControl.Controls.Window
    {
        public static RoutedUICommand OkCmd = new RoutedUICommand("OkCmd",
                "OkCmd", typeof(DTagSelection));
        public static RoutedUICommand CancelCmd = new RoutedUICommand("CancelCmd",
                "CancelCmd", typeof(DTagSelection));
        public static readonly DependencyProperty SelectionsProperty =
                    DependencyProperty.Register(
                        "Selections",
                        typeof(ObservableCollection<EditableTag>),
                        typeof(DTagSelection),
                        new FrameworkPropertyMetadata(new ObservableCollection<EditableTag>()));
        public ObservableCollection<EditableTag> Selections
        {
            get { return (ObservableCollection<EditableTag>)GetValue(SelectionsProperty); }
            set { SetValue(SelectionsProperty, value); }
        }
        public int TagId { get; set; }
        public TagFileInfo TagFile { get; set; }
        public DTagSelection()
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
                var exist = TagFile.Tags.Contains(item);
                Selections.Add(new EditableTag() { Text = item, Older = exist, Checked = exist });
            }
        }

        private void CancelHandle(object sender, ExecutedRoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void OkHandle(object sender, ExecutedRoutedEventArgs e)
        {
            bool changed = false;
            foreach(var t in Selections)
            {
                if(t.Checked && !t.Older)
                {
                    changed = true;
                    TagFile.Tags.Add(t.Text);
                    TagFile.CategoryTags.First(x=>x.Id==TagId).Tags.Add(t.Text);
                }
                else if(!t.Checked && t.Older)
                {
                    changed = true;
                    TagFile.Tags.Remove(t.Text);
                    TagFile.CategoryTags.First(x => x.Id == TagId).Tags.Remove(t.Text);
                }
            }
            if (changed)
            {
                TagFile.Rename();
            }
            this.DialogResult = true;
            this.Close();
        }
    }

    public class EditableTag : NotifyPropertyBase
    {
        private bool _Older = false;
        /// <summary>
        /// 评级(0-5)
        /// </summary>
        public bool Older { get => _Older; set { _Older = value; OnPropertyChanged("Older"); } }
        private bool _Checked = false;
        public bool Checked { get => _Checked; set { _Checked = value; OnPropertyChanged("Checked"); } }
        private string _tag;
        public string Text { get => _tag; set { _tag = value; OnPropertyChanged("Text"); } }
    }
}
