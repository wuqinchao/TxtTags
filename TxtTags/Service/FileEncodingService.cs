using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Shapes;
using TxtTags.Common;
using UtfUnknown;
using static System.Net.Mime.MediaTypeNames;

namespace TxtTags.Service
{
    public class FileEncodingService: NotifyPropertyBase
    {
        #region 单例
        //public static FileEncodingService Instance { get; set; }

        //static FileEncodingService()
        //{
        //    Instance = new FileEncodingService();
        //}

        #endregion

        private ObservableCollection<EncodingItem> _files = new ObservableCollection<EncodingItem>();
        public ObservableCollection<EncodingItem> Files { get => _files; set { _files = value; OnPropertyChanged("Files"); } }

        private bool _running = false;
        public bool Running { get => _running; set { _running = value; OnPropertyChanged("Running"); } }
        public void Init(List<TagFileInfo> fs, Encoding to)
        {
            Files.Clear();
            MakeList(fs, to);
        }
        public void Start()
        {
            Running = true;
            Task.Run(() =>
            {
                foreach(var item in Files)
                {
                    Trans(item);
                }
                Running = false;
            });
        }
        private void MakeList(List<TagFileInfo> fs, Encoding to)
        {
            foreach (TagFileInfo file in fs)
            {
                var item = new EncodingItem();
                item.File = file;
                item.Path = file.File.FullPath;
                item.TempPath = item.Path + "_____";
                item.Name = file.OrgName;
                item.Status = 0;
                if (file.Encoding != null)
                {
                    item.From = file.Encoding;
                }
                else
                {
                    try
                    {
                        item.From = file.Encoding = CharsetDetector.DetectFromFile(file.File.FullPath).Detected.Encoding;
                    }
                    catch
                    {
                        item.From = null;
                    }
                }
                if (to != null)
                {
                    item.To = to;
                }
                else
                {
                    item.To = item.From;
                }
                Files.Add(item);
            }
        }
        private void Trans(EncodingItem file)
        {
            try
            {
                file.Status = 1;
                var content = FileReader.ReadFileContent(file.Path, file.From ?? Encoding.UTF8);
                if(string.IsNullOrWhiteSpace(content))
                {
                    return;
                }
                content = content.ToSimplified();
                File.WriteAllText(file.TempPath, content, file.To ?? file.From ?? Encoding.UTF8);

                if (File.Exists(file.Path))
                    File.Delete(file.Path);
                System.IO.File.Move(file.TempPath, file.Path);
                file.File.Encoding = file.To;
                file.Status = 2;
            }
            catch(Exception exp)
            {
                file.Error = exp.Message;
                file.Status = 3;
            }
        }
    }

    public class EncodingItem: NotifyPropertyBase
    {
        private TagFileInfo _file;
        private string _path;
        private string _error;
        private string _temppath;
        private string _name;
        private int _status = 0;
        private bool _simplified = true;
        private Encoding _from;
        private Encoding _to;
        public TagFileInfo File { get => _file; set { _file = value; OnPropertyChanged("File"); } }
        public string TempPath { get => _temppath; set { _temppath = value; OnPropertyChanged("TempPath"); } }
        /// <summary>
        /// 原文件全路径
        /// </summary>
        public string Path { get => _path; set { _path = value; OnPropertyChanged("Path"); } }
        /// <summary>
        /// 文件名
        /// </summary>
        public string Name { get => _name; set { _name = value; OnPropertyChanged("Name"); } }
        public string Error { get => _error; set { _error = value; OnPropertyChanged("Error"); } }
        /// <summary>
        /// 0.未处理, 1.正在处理, 2.完成, 3.失败
        /// </summary>
        public int Status { get => _status; set { _status = value; OnPropertyChanged("Status"); } }
        public bool Simplified { get => _simplified; set { _simplified = value; OnPropertyChanged("Simplified"); } }
        public Encoding From { get => _from; set { _from = value; OnPropertyChanged("From"); } }
        public Encoding To { get => _to; set { _to = value; OnPropertyChanged("To"); } }
    }
}
