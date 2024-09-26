using HandyControl.Expression.Shapes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TxtTags.Common;
using TxtTags.Entity;

namespace TxtTags.Service
{
    public class BookmarkService : NotifyPropertyBase
    {
        public static string BaseFolder
        {
            get { return System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData); }
        }
        public static string AppName
        {
            get { return "TxtTags"; }
        }
        public static string AppDataPath
        {
            get { return $"{BaseFolder}\\{AppName}\\bookmarks"; }
        }
        static BookmarkService()
        {
            if (!Directory.Exists(AppDataPath))
            {
                Directory.CreateDirectory(AppDataPath);
            }
        }
        private static object Locker = new object();

        private SortableObservableCollection<BookmarkInfo> _marks = new SortableObservableCollection<BookmarkInfo>();
        private string _url;
        public string Url
        {
            get => _url;
            set => _url = value;
        }
        private string _key;
        public string Key
        {
            get => _key;
            set => _key = value;
        }
        public SortableObservableCollection<BookmarkInfo> Marks
        {
            get => _marks;
            set { _marks = value; OnPropertyChanged("Marks"); }
        }
        public string Path
        {
            get { return $"{AppDataPath}\\{Key}.json"; }
        }
        public void Add(BookmarkInfo bookmarkInfo)
        {
            if(!Marks.Any(x=>x.Offset == bookmarkInfo.Offset))
            {
                Marks.Add(bookmarkInfo);
                Save();
            }
        }
        public void Remove(int offset)
        {
            if (Marks.Any(x => x.Offset == offset))
            {
                Marks.Remove(Marks.First(x=>x.Offset==offset));
                Save();
            }
        }
        public BookmarkService(string url) 
        {
            Url = url;
            Key = TagFileInfo.GetOrgName(System.IO.Path.GetFileName(url));
            Load();
        }
        public void Load()
        {
            if (File.Exists(Path))
            {
                try
                {
                    lock (Locker)
                    {
                        Marks = JsonConvert.DeserializeObject<SortableObservableCollection<BookmarkInfo>>(File.ReadAllText(Path));
                    }
                }
                catch (Exception exp)
                {
                    Console.WriteLine(exp.Message);
                }
            }
        }
        public void Save()
        {
            lock (Locker)
            {
                var json = JsonConvert.SerializeObject(Marks);
                File.WriteAllText(Path, json);
            }
        }
        public static bool Exist(string path)
        {
            return System.IO.File.Exists(GetBookmarkPathFormFullPath(path));
        }
        public static void Rename(string src, string dst)
        {
            var scr_path = GetBookmarkPathFormFullPath(src);
            if (!System.IO.File.Exists(scr_path)) return;
            var dst_path = GetBookmarkPathFormFullPath(dst);
            if (System.IO.File.Exists(dst_path))
            {
                System.IO.File.Delete(dst_path);
            }
            System.IO.File.Move(scr_path, dst_path);
        }
        public static void Delete(string path)
        {
            if(Exist(path))
            {
                System.IO.File.Delete(GetBookmarkPathFormFullPath(path));
            }
        }
        public static string GetBookmarkPathFormFullPath(string path)
        {
            return $"{AppDataPath}\\{TagFileInfo.GetOrgName(System.IO.Path.GetFileName(path))}.json";
        }
    }
}
