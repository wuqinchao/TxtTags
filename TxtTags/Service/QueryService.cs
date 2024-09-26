using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TxtTags.Common;

namespace TxtTags.Service
{
    public delegate void QueryServiceCompleted(object sender, Result<List<TagFileInfo>> result);
    public class QueryService
    {
        #region 单例
        public static QueryService Instance { get; set; }

        static QueryService()
        {
            Instance = new QueryService();
        }

        #endregion

        #region everything
        public static readonly string EverythingBase = System.Environment.CurrentDirectory + @"\Everything";
        public static readonly string ExeName = "Everything";
        public string PathExe { get { return $"{EverythingBase}\\{ExeName}.exe"; } }
        #endregion
        private System.ComponentModel.BackgroundWorker bg = new System.ComponentModel.BackgroundWorker(); 
        public event QueryServiceCompleted OnWorkerCompleted;

        private QueryService() 
        {
            this.bg.WorkerReportsProgress = true;
            this.bg.WorkerSupportsCancellation = true;
            this.bg.DoWork += new System.ComponentModel.DoWorkEventHandler(bg_DoWork);
            this.bg.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bg_RunWorkerCompleted);
        }

        ~QueryService()
        {
            this.bg.DoWork -= new System.ComponentModel.DoWorkEventHandler(bg_DoWork);
            this.bg.RunWorkerCompleted -= new System.ComponentModel.RunWorkerCompletedEventHandler(bg_RunWorkerCompleted);
        }


        public void Start(bool admin = true)
        {
            Task.Run(() => {
                Process[] ps = Process.GetProcessesByName(ExeName);
                if (ps.Count() == 0)
                {
                    Process process = new Process();
                    process.StartInfo.FileName = PathExe;
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;//关键代码
                    process.StartInfo.WorkingDirectory = EverythingBase;
                    if (admin)
                    {
                        process.StartInfo.Verb = "runas";
                    }
                    process.StartInfo.UseShellExecute = true;
                    process.Start();
                    process.WaitForInputIdle(100); //关键代码
                }
            });
        }
        private void bg_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                OnWorkerCompleted(this, new Result<List<TagFileInfo>>(ErrorCode.UNKNOWN, e.Error.ToString()));
            }
            else if (e.Cancelled)
            {
                OnWorkerCompleted(this, new Result<List<TagFileInfo>>(ErrorCode.UNKNOWN, "操作已取消"));
            }
            else
            {
                OnWorkerCompleted(this, (Result<List<TagFileInfo>>)e.Result);
            }
        }

        public void Do(QueryOption option)
        {
            bg.RunWorkerAsync(option);
        }

        private void bg_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var option = (QueryOption)e.Argument;
            string Text = option.BasePath;
            if (!string.IsNullOrWhiteSpace(option.SubPath))
            {
                Text = Path.Combine(Text, option.SubPath);
            }
            Text += $" ext:txt";
            if (!string.IsNullOrWhiteSpace(option.Keywords))
            {
                Text += $" {option.Keywords}";
            }
            if (option.Tags.Count > 0)
            {
                foreach (var t in option.Tags)
                {
                    Text += $" #{t}";
                }
            }
            if (option.Star > 0)
            {
                Text += $" &{option.Star}";
            }
            if (option.Fav)
            {
                Text += $" ^^";
            }
            if (!string.IsNullOrWhiteSpace(option.Content))
            {
                Text += $" content:{option.Content}";
            }
            if (option.OnlyFile)
            {
                Text += $" file:";
            }
            if (option.Nocase)
            {
                Text += $" nocase:";
            }
            List<TagFileInfo> result = new List<TagFileInfo>();
            uint i;
            // 设置搜索内容
            EverythingWrapper.Everything_SetSearchW(Text);
            EverythingWrapper.Everything_SetRequestFlags(
                EverythingWrapper.EVERYTHING_REQUEST_FILE_NAME |
                EverythingWrapper.EVERYTHING_REQUEST_PATH |
                EverythingWrapper.EVERYTHING_REQUEST_DATE_CREATED |
                EverythingWrapper.EVERYTHING_REQUEST_DATE_MODIFIED |
                EverythingWrapper.EVERYTHING_REQUEST_DATE_ACCESSED |
                EverythingWrapper.EVERYTHING_REQUEST_SIZE);

            EverythingWrapper.Everything_SetSort(EverythingWrapper.EVERYTHING_SORT_DATE_MODIFIED_DESCENDING);
            EverythingWrapper.Everything_SetMatchPath(false);
            // 您是不是要找：
            EverythingWrapper.Everything_QueryW(true);
            // //按路径排序
            EverythingWrapper.Everything_SortResultsByPath();

            const int bufferSize = 256;
            long modified, created, accessed, size;
            StringBuilder buffer = new StringBuilder(bufferSize);
            // 循环遍历结果，将每个结果添加到列表框中。
            for (i = 0; i < EverythingWrapper.Everything_GetNumResults(); i++)
            {
                if (!EverythingWrapper.Everything_IsFileResult(i))
                {
                    continue;
                }
                EverythingWrapper.Everything_GetResultFullPathName(i, buffer, bufferSize);
                var fullPath = buffer.ToString();
                EverythingWrapper.Everything_GetResultSize(i, out size);
                EverythingWrapper.Everything_GetResultDateCreated(i, out created);
                EverythingWrapper.Everything_GetResultDateModified(i, out modified);
                EverythingWrapper.Everything_GetResultDateAccessed(i, out accessed);
                //Console.WriteLine($"{fullPath} {DateTime.FromFileTime(created).ToString("yyyy/MM/dd HH:mm:ss")} {Path.GetFileName(fullPath)} {Path.GetDirectoryName(fullPath)} {size}");

                result.Add(new TagFileInfo(new EverythingFile()
                {
                    FullPath = fullPath,
                    Accessed = accessed >= 0 ? DateTime.FromFileTime(accessed) : DateTime.MinValue,
                    Created = created >= 0 ? DateTime.FromFileTime(created) : DateTime.MinValue,
                    Modified = modified >= 0 ? DateTime.FromFileTime(modified) : DateTime.MinValue,
                    Size = size
                }));

            }
            e.Result = new Result<List<TagFileInfo>>(ErrorCode.SUCC, "", result);
        }
    }
}
