using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TxtTags.Common;

namespace TxtTags.Dialog
{
    /// <summary>
    /// DInput.xaml 的交互逻辑
    /// </summary>
    public partial class DInput : HandyControl.Controls.Window
    {
        public static RoutedUICommand OkCmd = new RoutedUICommand("OkCmd",
                "OkCmd", typeof(DInput));
        public static RoutedUICommand CancelCmd = new RoutedUICommand("CancelCmd",
                "CancelCmd", typeof(DInput));

        public static readonly DependencyProperty NotesProperty =
                    DependencyProperty.Register(
                        "Notes",
                        typeof(string),
                        typeof(DInput),
                        new FrameworkPropertyMetadata(null));
        public string Notes
        {
            get { return (string)GetValue(NotesProperty); }
            set { SetValue(NotesProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
                    DependencyProperty.Register(
                        "Value",
                        typeof(string),
                        typeof(DInput),
                        new FrameworkPropertyMetadata(null));
        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public DInput()
        {
            this.DataContext = this;
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(OkCmd, OkHandle));
            CommandBindings.Add(new CommandBinding(CancelCmd, CancelHandle));
        }
        private void CancelHandle(object sender, ExecutedRoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void OkHandle(object sender, ExecutedRoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
