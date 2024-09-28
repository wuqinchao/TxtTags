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
using TxtTags.Common;
using TxtTags.Service;

namespace TxtTags.Service
{
    public class HistoryService : NotifyPropertyBase
    {
        private static object Locker = new object();
        private static object UpdateLocker = new object();

        #region 单例
        public static HistoryService Instance { get; set; }

        static HistoryService()
        {
            if (File.Exists(ConfigPath))
            {
                Instance = Load();
            }
            else
            {
                Instance = new HistoryService();
                Instance.Save();
            }
        }
        #endregion

        #region 文件操作
        public static string ConfigPath
        {
            get { return $"{FilePath.BaseFolder}\\{FilePath.AppName}\\history.json"; }
        }
        public static HistoryService Load()
        {
            HistoryService handle = null;
            if (File.Exists(ConfigPath))
            {
                try
                {
                    lock (Locker)
                    {
                        handle = JsonConvert.DeserializeObject<HistoryService>(File.ReadAllText(ConfigPath));
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
                        handle = JsonConvert.DeserializeObject<HistoryService>(File.ReadAllText($"{ConfigPath}.bak"));
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

        private SortableObservableCollection<HistoryItem> _Items = new SortableObservableCollection<HistoryItem>();
        /// <summary>
        /// 字体大小
        /// </summary>
        public SortableObservableCollection<HistoryItem> Items { get => _Items; set { _Items = value; OnPropertyChanged("Items"); } }

        public void Update(string key, int offset)
        {
            lock (UpdateLocker)
            {
                var h = Items.FirstOrDefault(x => x.Key == key);
                if (offset > 0)
                {
                    if (h != null)
                    {
                        h.Offset = offset;
                        h.Time = DateTime.Now;
                    }
                    else
                    {
                        Items.Add(new HistoryItem() { Key = key, Offset = offset, Time = DateTime.Now });
                    }
                    Save();
                }
                else
                {
                    if (h != null)
                    {
                        Items.Remove(h);
                        Save();
                    }
                }
            }
        }

        public int GetOffset(string key)
        {
            lock (UpdateLocker)
            {
                var h = Items.FirstOrDefault(x => x.Key == key);
                if (h != null)
                {
                    return h.Offset;
                }
                return 0;
            }
        }
    }

    public class HistoryItem : NotifyPropertyBase, IComparable<HistoryItem>
    {
        private string _key;
        public string Key { get => _key; set { _key = value; OnPropertyChanged("Key"); } }
        private int _offset;
        public int Offset { get => _offset; set { _offset = value; OnPropertyChanged("Offset"); } }
        private DateTime _time;
        public DateTime Time { get => _time; set { _time = value; OnPropertyChanged("Time"); } }

        public int CompareTo(HistoryItem other)
        {
            return Time.CompareTo(other.Time);
        }
    }
}
