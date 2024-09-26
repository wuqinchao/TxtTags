using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TxtTags.Common;

namespace TxtTags
{
    public class QueryOption: NotifyPropertyBase
    {
        private string basePath;
        // 仅搜索文件 file:
        private bool onlyFile = false;
        // 不区分大小写 nocase:
        private bool nocase = true;
        private string keywords = "";
        // content:<text>	搜索文本内容.
        private string content = "";
        // 指定子目录
        private string subPath = "";
        // 星级
        private int star = 0;
        // 收藏
        private bool fav = false;
        private ObservableCollection<string> tags = new ObservableCollection<string>();
        public string BasePath { get => basePath; set { basePath = value; OnPropertyChanged("BasePath"); } }
        public bool OnlyFile { get => onlyFile; set { onlyFile = value; OnPropertyChanged("OnlyFile"); } }
        public bool Nocase { get => nocase; set { nocase = value; OnPropertyChanged("Nocase"); } }
        public string Content { get => content; set { content = value; OnPropertyChanged("Content"); } }
        public string Keywords { get => keywords; set { keywords = value; OnPropertyChanged("Keywords"); } }
        public string SubPath { get => subPath; set { subPath = value; OnPropertyChanged("SubPath"); } }
        public ObservableCollection<string> Tags { get => tags; set { tags = value; OnPropertyChanged("Tags"); } }
        public int Star { get => star; set { star = value; OnPropertyChanged("Star"); } }
        public bool Fav { get => fav; set { fav = value; OnPropertyChanged("Fav"); } }
    }
}
