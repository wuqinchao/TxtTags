using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TxtTags.Common
{
    public class NotifyPropertyBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                if (handler.Target is System.Windows.Controls.Control)
                {
                    System.Windows.Controls.Control target = handler.Target as System.Windows.Controls.Control;
                    if (System.Threading.Thread.CurrentThread != target.Dispatcher.Thread)
                    {
                        target.Dispatcher.Invoke(handler, new object[] { propertyName });
                    }
                    else
                    {
                        handler(this, new PropertyChangedEventArgs(propertyName));
                    }
                }
                else
                {
                    handler(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }
    }
}
