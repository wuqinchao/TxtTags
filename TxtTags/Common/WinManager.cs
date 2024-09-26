using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TxtTags.Common
{
    public class WinManager
    {
        public static void WindowHide(Window w)
        {
            w.Visibility = Visibility.Hidden;
            w.ShowInTaskbar = false;
        }

        public static void WindowShow(Window w, bool showInTaskbar = true)
        {
            w.Visibility = Visibility.Visible;
            w.ShowInTaskbar = showInTaskbar;
        }

        public static void ShowChild(Window p, Window w, bool IshowInTaskbar = true)
        {
            EventHandler handle = null;
            w.Closed += handle = (s, ev) => {
                w.Closed -= handle;
                WindowShow(p, IshowInTaskbar);
            };
            WindowHide(p);
            w.Show();
        }
    }
}
