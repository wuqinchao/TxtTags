using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TxtTags
{
    public class ColumnDefinitionExtended : ColumnDefinition
    {

        static ColumnDefinitionExtended()
        {
            WidthProperty.OverrideMetadata(typeof(ColumnDefinitionExtended),
                new FrameworkPropertyMetadata(new GridLength(0), null, new CoerceValueCallback(CoerceWidth)));
            MinWidthProperty.OverrideMetadata(typeof(ColumnDefinitionExtended),
                new FrameworkPropertyMetadata((double)0, null, new CoerceValueCallback(CoerceMinWidth)));
        }

        private static object CoerceMinWidth(DependencyObject d, object baseValue)
        {
            return ((ColumnDefinitionExtended)d).Visible ? baseValue : (Double)0;
        }

        private static object CoerceWidth(DependencyObject d, object baseValue)
        {
            return ((ColumnDefinitionExtended)d).Visible ? baseValue : new GridLength(0);
        }

        public bool Visible
        {
            get { return (bool)GetValue(VisibleProperty); }
            set { SetValue(VisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Visible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VisibleProperty =
            DependencyProperty.Register("Visible", typeof(bool), typeof(ColumnDefinitionExtended),
                new PropertyMetadata(true, new PropertyChangedCallback(OnVisibleChanged)));

        private static void OnVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(WidthProperty);
            d.CoerceValue(MinWidthProperty);
        }
    }
}
