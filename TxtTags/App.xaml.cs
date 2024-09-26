using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using TxtTags.Common;
using TxtTags.Service;

namespace TxtTags
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            QueryService.Instance.Start();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Config.Instance.Save();
        }
    }
}
