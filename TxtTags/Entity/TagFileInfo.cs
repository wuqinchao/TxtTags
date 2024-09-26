using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using TxtTags.Common;
using TxtTags.Service;
using static System.Net.Mime.MediaTypeNames;

namespace TxtTags
{
    public class TagFileInfo: NotifyPropertyBase
    {
        private Encoding _encoding;
        public Encoding Encoding { get => _encoding; set { _encoding = value; OnPropertyChanged("Encoding"); } }
        private int _star = 0;
        /// <summary>
        /// 评级(0-5)
        /// </summary>
        public int Star { get => _star; set { _star = value; OnPropertyChanged("Star"); } }
        private bool _fav = false;
        /// <summary>
        /// 收藏
        /// </summary>
        public bool Fav { get => _fav; set { _fav = value; OnPropertyChanged("Fav"); } }
        private string _orgName;
        /// <summary>
        /// 原始文件名(不包括扩展名、标签)
        /// </summary>
        public string OrgName { get => _orgName; set { _orgName = value; OnPropertyChanged("OrgName"); } }
        private EverythingFile _file;
        public EverythingFile File { get => _file; set { _file = value; OnPropertyChanged("File"); } }

        private List<string> _tags = new List<string>();
        public List<string> Tags { get => _tags; set { _tags = value; OnPropertyChanged("Tags"); } }

        private ObservableCollection<BookTag> _CategoryTags = new ObservableCollection<BookTag>();
        public ObservableCollection<BookTag> CategoryTags
        {
            get => _CategoryTags;
            set
            {
                if (value == null)
                {
                    _CategoryTags = new ObservableCollection<BookTag>();
                }
                else
                {
                    _CategoryTags = value;
                }
                OnPropertyChanged("CategoryTags");
            }
        }

        public TagFileInfo(EverythingFile file) 
        {
            Star = file.Star;
            Fav = file.Fav;
            File = file;
            var temp = File.Name;
            // 扩展名
            temp = temp.Replace(File.Ext, "");
            // 评级
            if(Regex.IsMatch(temp, @"&(\d{1})"))
            {
                temp = Regex.Replace(temp, @"&(\d{1})", "", RegexOptions.Compiled);
            }
            // 收藏
            if(temp.Contains(EverythingFile.PRE_FAV))
            {
                temp = Regex.Replace(temp, @"(\^\^)", "", RegexOptions.Compiled);
            }
            // 标签
            if (temp.Contains("#"))
            {
                var ts = temp.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 1; i < ts.Length; i++)
                {
                    Tags.Add($"{ts[i]}");
                }
                OrgName = ts[0];
            }
            else
            {
                OrgName = temp;
            }

            foreach(BookTag category in BookTags.Instance.Items)
            {
                var targetCategory = new BookTag() { Id = category.Id, Name = category.Name, Hide = category.Hide };
                foreach (var tag in category.Tags)
                {
                    if(Tags.Contains(tag))
                    {
                        targetCategory.Tags.Add(tag);
                    }
                }
                CategoryTags.Add(targetCategory);                    
            }
        }
        /// <summary>
        /// 获取原始文件名
        /// </summary>
        /// <param name="path">文件全路径</param>
        /// <param name="ext">返回文件名是否需要包含后缀名, 默认false</param>
        /// <returns>原始文件名</returns>
        public static string GetOrgName(string path, bool includeExt = false)
        {
            var ext = Path.GetExtension(path);
            var temp = path;
            // 扩展名
            temp = temp.Replace(ext, "");
            // 评级
            if (Regex.IsMatch(temp, @"&(\d{1})"))
            {
                temp = Regex.Replace(temp, @"&(\d{1})", "", RegexOptions.Compiled);
            }
            // 收藏
            if (temp.Contains(EverythingFile.PRE_FAV))
            {
                temp = Regex.Replace(temp, @"(\^\^)", "", RegexOptions.Compiled);
            }
            // 标签
            if (temp.Contains("#"))
            {
                var ts = temp.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                temp = ts[0];
            }
            if (includeExt)
                return temp + ext;
            else
                return temp;
        }
        public void CopyName()
        {
            Clipboard.SetDataObject(TagFileInfo.GetOrgName(File.Name));
        }
        public void CopyPath()
        {
            Clipboard.SetDataObject(File.FullPath);
        }
        public void Open()
        {
            System.Diagnostics.Process.Start(Config.Instance.ViewExe, $"\"{File.FullPath}\"");
        }
        public string GetNewName(string org, bool ext=false)
        {
            var temp = org;
            if (Fav)
            {
                temp += EverythingFile.PRE_FAV;
            }
            if (Star > 0)
            {
                temp += $"{EverythingFile.PRE_START}{Star}";
            }
            foreach (var ts in Tags)
            {
                temp += $"{EverythingFile.PRE_TAG}{ts}";
            }
            if (ext)
                temp += File.Ext;
            return temp;
        }
        public void Rename()
        {
            var temp = OrgName;
            if(Fav)
            {
                temp += EverythingFile.PRE_FAV;
            }
            if(Star > 0)
            {
                temp += $"{EverythingFile.PRE_START}{Star}";
            }
            foreach (var ts in Tags)
            {
                temp += $"{EverythingFile.PRE_TAG}{ts}";
            }
            temp += File.Ext;
            var tempFullPath = Path.Combine(File.Path, temp);
            if (tempFullPath != File.FullPath)
            {
                if (System.IO.File.Exists(tempFullPath))
                {
                    System.IO.File.Delete(tempFullPath);
                }
                System.IO.File.Move(File.FullPath, tempFullPath);
                BookmarkService.Rename(File.FullPath, tempFullPath);
                File.FullPath = tempFullPath;
            }
        }
    }
}
