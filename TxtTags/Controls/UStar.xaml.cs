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

namespace TxtTags
{
    /// <summary>
    /// UStar.xaml 的交互逻辑
    /// </summary>
    public partial class UStar : UserControl
    {
        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            nameof(ValueChanged), RoutingStrategy.Bubble, typeof(EventHandler<StarChangedEventArgs>), typeof(UStar)
        );
        public event EventHandler<StarChangedEventArgs> ValueChanged
        {
            add => AddHandler(ValueChangedEvent, value);
            remove => RemoveHandler(ValueChangedEvent, value);
        }

        public static RoutedUICommand ValueChangedCmd = new RoutedUICommand("ValueChangedCmd",
                "ValueChangedCmd", typeof(UStar));

        public static readonly DependencyProperty FalseColorProperty = 
            DependencyProperty.Register("FalseColor", typeof(Brush), typeof(UStar), new UIPropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#757575"))));
        public Brush FalseColor
        {
            get { return (Brush)GetValue(FalseColorProperty); }
            set { SetValue(FalseColorProperty, value); }
        }
        public static readonly DependencyProperty TrueColorProperty =
            DependencyProperty.Register("TrueColor", typeof(Brush), typeof(UStar), new UIPropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2db84d"))));
        public Brush TrueColor
        {
            get { return (Brush)GetValue(TrueColorProperty); }
            set { SetValue(TrueColorProperty, value); }
        }
        public static readonly DependencyProperty CountProperty =
            DependencyProperty.Register("Count", typeof(int), typeof(UStar), new UIPropertyMetadata(5, CountChanged));
        public int Count
        {
            get { return (int)GetValue(CountProperty); }
            set { SetValue(CountProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(UStar), new UIPropertyMetadata(0));
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValuesProperty =
            DependencyProperty.Register("Values", typeof(List<int>), typeof(UStar), new UIPropertyMetadata(new List<int>() { 1,2,3,4,5 }));
        public List<int> Values
        {
            get { return (List<int>)GetValue(ValuesProperty); }
            set { SetValue(ValuesProperty, value); }
        }
        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register("IconSize", typeof(double), typeof(UStar), new UIPropertyMetadata(16d));
        public double IconSize
        {
            get { return (double)GetValue(IconSizeProperty); }
            set { SetValue(IconSizeProperty, value); }
        }
        public UStar()
        {
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(ValueChangedCmd, ValueChangedHandle));
        }
        private void ValueChangedHandle(object sender, ExecutedRoutedEventArgs e)
        {
            int input = (int)e.Parameter;
            var temp = Value;
            if (temp != input)
            {
                temp = input;
            }
            else
            {
                temp = 0;
            }
            FireStarChangedEvent(temp);
        }
        public void ResetCount()
        {
            Values.Clear();
            for(int i = 1; i <= Count; i++)
            {
                Values.Add(i);
            }
        }
        private static void CountChanged(DependencyObject obj, DependencyPropertyChangedEventArgs r)
        {
            var c = (UStar)obj;
            c.ResetCount();
        }
        public void FireStarChangedEvent(int star)
        {
            this.RaiseEvent(new StarChangedEventArgs(ValueChangedEvent, this)
            {
                Star = star
            });
        }
    }
    public class StarChangedEventArgs : RoutedEventArgs
    {
        public StarChangedEventArgs(RoutedEvent routedEvent, object source)
            : base(routedEvent, source)
        {
        }
        public int Star { get; set; }
    }
}
