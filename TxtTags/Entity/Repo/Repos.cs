using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TxtTags.Common;

namespace TxtTags
{
    public class Repos: NotifyPropertyBase
    {
        private static object Locker = new object();
        public static Repos Instance = null;
        public static string ConfigPath
        {
            get { return $"{FilePath.BaseFolder}\\{FilePath.AppName}\\repos.json"; }
        }
        private Repos() { }
        static Repos()
        {
            if (File.Exists(ConfigPath))
            {
                Instance = Load();
            }
            else
            {
                Instance = new Repos();
                Instance.Save();
            }
        }
        public static Repos Load()
        {
            Repos handle = null;
            if (File.Exists(ConfigPath))
            {
                try
                {
                    lock (Locker)
                    {
                        handle = JsonConvert.DeserializeObject<Repos>(File.ReadAllText(ConfigPath));
                    }
                }
                catch (Exception exp)
                {
                    Console.WriteLine(exp.Message);
                }
            }
            if (handle == null && File.Exists($"{ConfigPath}.bak"))
            {
                try
                {
                    lock (Locker)
                    {
                        handle = JsonConvert.DeserializeObject<Repos>(File.ReadAllText($"{ConfigPath}.bak"));
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
        private SortableObservableCollection<Repo> _items = new SortableObservableCollection<Repo>();
        /// <summary>
        /// 相机触发源
        /// </summary>
        public SortableObservableCollection<Repo> Items
        {
            get => _items;
            set { _items = value; OnPropertyChanged("Items"); }
        }
        public Result<int> GetNewId()
        {
            if (Items.Count == 0)
            {
                return new Result<int>(ErrorCode.SUCC, "", 1);
            }
            else
            {
                var i = Items.Max(s => s.Id);
                return new Result<int>(ErrorCode.SUCC, "", i + 1);
            }
        }
        public bool Exists(int id)
        {
            return Items.Any(x => x.Id == id);
        }
        public Repo Fetch(int id)
        {
            return Items.FirstOrDefault(x => x.Id == id);
        }
        public void Add(Repo info)
        {
            if (Exists(info.Id))
            {
                Update(info);
            }
            else
            {
                var item = info.Clone();
                Items.Add(item);
                Save();
            }
        }
        public void Update(Repo info)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items.ElementAt(i).Id.Equals(info.Id))
                {
                    Repo item = Items.ElementAt(i);
                    Repo.Copy(ref item, info);
                    Save();
                    return;
                }
            }
        }
        public void Remove(int id)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items.ElementAt(i).Id.Equals(id))
                {
                    Items.Remove(Items.ElementAt(i));
                    Save();
                    return;
                }
            }
        }
    }
}
