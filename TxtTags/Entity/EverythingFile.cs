using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TxtTags.Common;
using System.IO;

namespace TxtTags
{
    public class EverythingFile : NotifyPropertyBase
    {
        public const string PRE_START = "&";
        public const string PRE_FAV = "^^";
        public const string PRE_TAG = "#";

        private string   _name;
        private string   _path;
        private string   _fullPath;
        private string   _ext;
        private long     _size;
        private int      _star = 0;
        private bool     _fav = false;
        private DateTime _modified;
        private DateTime _created;
        private DateTime _accessed;
        /// <summary>
        /// 文件名，包含标签及后缀
        /// </summary>
        public string Name { get => _name; set { _name = value; OnPropertyChanged("Name"); } }
        /// <summary>
        /// 文件目录
        /// </summary>
        public string Path { get => _path; set { _path = value; OnPropertyChanged("Path"); } }
        /// <summary>
        /// 文件全路径
        /// </summary>
        public string FullPath 
        { 
            get => _fullPath; 
            set 
            { 
                _fullPath = value;
                OnPropertyChanged("FullPath");
                Name = System.IO.Path.GetFileName(value);
                Path = System.IO.Path.GetDirectoryName(value);
                Ext  = System.IO.Path.GetExtension(value);
                Fav = Name.Contains(PRE_FAV);
                if(Name.Contains(PRE_START))
                {
                    var s = RegexHelper.Search(Name, @"&(\d{1})");
                    if(!string.IsNullOrWhiteSpace(s))
                    {
                        Star = int.Parse(s);
                    }
                }
            } 
        }
        /// <summary>
        /// 文件后缀
        /// </summary>
        public string Ext { get => _ext; set { _ext = value; OnPropertyChanged("Ext"); } }
        /// <summary>
        /// 文件大小(byte)
        /// </summary>
        public long Size { get => _size; set { _size = value; OnPropertyChanged("Size"); } }
        public DateTime Modified { get => _modified; set { _modified = value; OnPropertyChanged("Modified"); } }
        public DateTime Created { get => _created; set { _created = value; OnPropertyChanged("Created"); } }
        public DateTime Accessed { get => _accessed; set { _accessed = value; OnPropertyChanged("Accessed"); } }
        /// <summary>
        /// 评级(0-5)
        /// </summary>
        public int Star { get => _star; set { _star = value; OnPropertyChanged("Star"); } }
        /// <summary>
        /// 收藏
        /// </summary>
        public bool Fav { get => _fav; set { _fav = value; OnPropertyChanged("Fav"); } }
    }
}
