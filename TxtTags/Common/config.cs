using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TxtTags.Service;

namespace TxtTags.Common
{
    public class Config: NotifyPropertyBase
    {
        private static object Locker = new object();
        public const string DefaultPassword = "123456";

        #region 单例
        public static Config Instance { get; set; }

        static Config()
        {
            if (File.Exists(ConfigPath))
            {
                Instance = Load();
                if (string.IsNullOrWhiteSpace(Instance.Password))
                {
                    Instance.Password = Copyto(DefaultPassword);
                }
            }
            else
            {
                Instance = new Config();
                Instance.Password = Copyto(DefaultPassword);
                Instance.Save();
            }
        }
        #endregion

        #region 文件操作
        public static string ConfigPath
        {
            get { return $"{FilePath.BaseFolder}\\{FilePath.AppName}\\app.json"; }
        }
        public static Config Load()
        {
            Config handle = null;
            if (File.Exists(ConfigPath))
            {
                try
                {
                    lock (Locker)
                    {
                        handle = JsonConvert.DeserializeObject<Config>(File.ReadAllText(ConfigPath));
                    }
                }
                catch (Exception exp)
                {
                    Console.WriteLine(exp);
                }
            }
            if (handle == null && File.Exists($"{ConfigPath}.bak"))
            {
                try
                {
                    lock (Locker)
                    {
                        handle = JsonConvert.DeserializeObject<Config>(File.ReadAllText($"{ConfigPath}.bak"));
                    }
                }
                catch (Exception exp)
                {
                    Console.WriteLine(exp);
                }

                if (handle != null)
                {
                    handle.Save();
                }
            }
            return handle;
        }
        public void Save()
        {
            lock (Locker)
            {
                var json = JsonConvert.SerializeObject(this);
                File.WriteAllText(ConfigPath, json);
                File.WriteAllText($"{ConfigPath}.bak", json);
            }
        }
        #endregion

        private readonly FileVersionInfo VersionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
        private string _ViewExe = "\"G:\\GreenTools\\EditPlus 3\\EditPlus.exe\"";
        private LogLevel logLevel = LogLevel.Error;
        private bool _locked = true;
        private string password;
        /// <summary>
        /// 外部编辑器
        /// </summary>
        public string ViewExe { get => _ViewExe; set { _ViewExe = value; OnPropertyChanged("ViewExe"); } }
        /// <summary>
        /// 用户MD5后的密码
        /// </summary>
        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }
        /// <summary>
        /// 日志等级, 默认Error
        /// </summary>
        public LogLevel LogLevel
        {
            get => logLevel;
            set
            {
                logLevel = value;
                OnPropertyChanged("LogLevel");
            }
        }

        #region 运行时动态属性
        /// <summary>
        /// 当前锁定状态
        /// </summary>
        [JsonIgnore]
        public bool Locked 
        { 
            get => _locked; 
            set { _locked = value; OnPropertyChanged("Locked"); } 
        }
        /// <summary>
        /// 获取版本
        /// </summary>
        [JsonIgnore]
        public FileVersionInfo Version
        {
            get => VersionInfo;
        }

        #endregion

        #region 阅读器配置属性
        private double _ViewerFontSize = 16d;
        /// <summary>
        /// 字体大小
        /// </summary>
        public double ViewerFontSize { get => _ViewerFontSize; set { _ViewerFontSize = value; OnPropertyChanged("ViewerFontSize"); } }
        private double _ViewerLineHeight = 1.5d;
        /// <summary>
        /// 行高
        /// </summary>
        public double ViewerLineHeight { get => _ViewerLineHeight; set { _ViewerLineHeight = value; OnPropertyChanged("ViewerLineHeight"); } }
        private SolidColorBrush _ViewerForceColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333333"));
        /// <summary>
        /// 前景色
        /// </summary>
        public SolidColorBrush ViewerForceColor { get => _ViewerForceColor; set { _ViewerForceColor = value; OnPropertyChanged("ViewerForceColor"); } }
        private SolidColorBrush _ViewerBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CDFAE7"));
        /// <summary>
        /// 背景色
        /// </summary>
        public SolidColorBrush ViewerBackground { get => _ViewerBackground; set { _ViewerBackground = value; OnPropertyChanged("ViewerBackground"); } }
        #endregion

        public static string Copyto(string input)
        {
            return Crypto.GetMd5Str("TEXTVIEWER-" + Crypto.GetMd5Str(input.Trim())).ToLower();
        }
    }
}
