using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TxtTags.Common;

namespace TxtTags
{
    public class BookTags : NotifyPropertyBase
    {
        private ObservableCollection<BookTag> _items = new ObservableCollection<BookTag>();
        public ObservableCollection<BookTag> Items 
        { 
            get => _items; 
            set {
                if (value == null)
                {
                    _items = new ObservableCollection<BookTag>();
                }
                else
                {
                    _items = value;
                }
                OnPropertyChanged("Items"); 
            } 
        }

        private static object Locker = new object();
        public static BookTags Instance = null;
        public static string ConfigPath
        {
            get { return $"{FilePath.BaseFolder}\\{FilePath.AppName}\\tags.json"; }
        }
        private BookTags() { }
        static BookTags()
        {
            if (File.Exists(ConfigPath))
            {
                Instance = Load();
            }
            else
            {
                Instance = new BookTags();
                Instance.Save();
            }
        }
        public static BookTags Load()
        {
            BookTags handle = null;
            if (File.Exists(ConfigPath))
            {
                try
                {
                    lock (Locker)
                    {
                        handle = JsonConvert.DeserializeObject<BookTags>(File.ReadAllText(ConfigPath));
                    }
                }
                catch (Exception exp)
                {
                    Console.WriteLine(exp.Message);
                }
            }
            if (handle == null)
            {
                if (File.Exists($"{ConfigPath}.bak"))
                {
                    try
                    {
                        lock (Locker)
                        {
                            handle = JsonConvert.DeserializeObject<BookTags>(File.ReadAllText($"{ConfigPath}.bak"));
                        }
                    }
                    catch (Exception exp)
                    {
                        Console.WriteLine(exp);
                    }
                }
                else
                {
                    handle = new BookTags();
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
            return Items.Any(s => s.Id == id);
        }
        public BookTag Fetch(int id)
        {
            return Items.FirstOrDefault(s => s.Id == id);
        }
        public void Add(BookTag info)
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
        public void Update(BookTag info)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items.ElementAt(i).Id.Equals(info.Id))
                {
                    BookTag item = Items.ElementAt(i);
                    BookTag.Copy(ref item, info);
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
        
        public int FindCategoryId(string key)
        {
            var t = Items.FirstOrDefault(x => x.Tags.Contains(key));
            if (t != null)
                return t.Id;
            else
                return 0;
        }
    }
    public class BookTag : NotifyPropertyBase
    {
        private int _id;
        private string _name;
        private bool _hide = false;
        private string _tag;
        private SortableObservableCollection<string> _tags = new SortableObservableCollection<string>();
        /// <summary>
        /// 标签类别ID
        /// </summary>
        public int Id { get => _id; set { _id = value; OnPropertyChanged("Id"); } }
        /// <summary>
        /// 标签类别名称
        /// </summary>
        public string Name { get => _name; set { _name = value; OnPropertyChanged("Name"); } }
        public bool Hide { get => _hide; set { _hide = value; OnPropertyChanged("Hide"); } }
        /// <summary>
        /// 标签列表,包含逗号
        /// </summary>
        public string Tag
        {
            get => _tag;
            set
            {
                _tag = value;
                OnPropertyChanged("Tag");
                if (string.IsNullOrWhiteSpace(value))
                {
                    Tags = new SortableObservableCollection<string>();
                }
                else
                {
                    Tags = new SortableObservableCollection<string>(Tag.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                }
            }
        }
        [JsonIgnore]
        public SortableObservableCollection<string> Tags
        {
            get => _tags;
            set { _tags = value; OnPropertyChanged("Tags"); }
        }
        public static void Copy(ref BookTag to, BookTag from)
        {
            to.Id = from.Id;
            to.Name = from.Name;
            to.Tag = from.Tag;
            to.Hide = from.Hide;
        }
        public BookTag Clone()
        {
            var to = new BookTag()
            {
                Id = Id,
                Name = Name,
                Tag = Tag,
                Hide = Hide
            };

            return to;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
