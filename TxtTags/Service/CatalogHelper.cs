using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TxtTags.Common;
using TxtTags.Entity;
using TxtTags.Service;

namespace TxtTags.Service
{
    public delegate void CatalogServiceCompleted(object sender, Result<List<CatalogInfo>> result);
    public class CatalogHelper
    {
        // 章一
        // 正文 三、魔鬼军士
        // 正文　一分卷 第一章 出山
        // 妖妻百科全书 卷一出场人物志
        //private static readonly List<Regex> REGS = new List<Regex>() {
        //    new Regex(@"^(正文)?(\s)*(([～#-=—→☆、]*?)第(.*?)[章节回集卷部篇季话]{1})"),
        //    new Regex(@"^(((CHAPTER)|(PART)|(chapter)|(part)|(Chapter)|(Part))(\s)*\d+)"),
        //    new Regex(@"^正文(\s)*(.*?)((本纪)|(世家)|(列传)|(表)){1}"),
        //    new Regex(@"^(正文[\s\S]*)?([章卷]{1})[0-9一二三四五六七八九十拾零百佰千仟]+"),
        //};
        /*
        (^(正文(\s)*)?(([～#-=—→☆、]*?)第[0-9一二三四五六七八九十拾零百佰千仟壹贰叁肆伍陆柒捌玖]+[章节節回集卷部篇季话册]{1}))
        (^(((CHAPTER)|(PART)|(chapter)|(part)|(Chapter)|(Part))(\s)*\d+))
        (^正文(\s)*(.*?)((本纪)|(世家)|(列传)|(表)){1})
        (^(正文[\s\S]*)?([章卷册]{1})[0-9一二三四五六七八九十拾零百佰千仟壹贰叁肆伍陆柒捌玖]+)

         */
        private static readonly Regex REG = new Regex(@"(^(正文(\s)*)?(([～#-=—→☆、]*?)第[0-9一二三四五六七八九十拾零百佰千仟壹贰叁肆伍陆柒捌玖]+[章节節回集卷部篇季话册]{1}))|(^(((CHAPTER)|(PART)|(chapter)|(part)|(Chapter)|(Part))(\s)*\d+))|(^正文(\s)*(.*?)((本纪)|(世家)|(列传)|(表)){1})|(^(正文[\s\S]*)?([章卷册]{1})[0-9一二三四五六七八九十拾零百佰千仟壹贰叁肆伍陆柒捌玖]+)");
        #region 单例
        public static CatalogHelper Instance { get; set; }

        static CatalogHelper()
        {
            Instance = new CatalogHelper();
        }

        #endregion
        private object Locker = new object();
        private System.ComponentModel.BackgroundWorker bg = new System.ComponentModel.BackgroundWorker();
        public event CatalogServiceCompleted OnWorkerCompleted;
        private bool _Running = false;
        public bool Running
        {
            get => _Running;
            private set => _Running = value;
        }
        private CatalogHelper()
        {
            this.bg.WorkerReportsProgress = true;
            this.bg.WorkerSupportsCancellation = true;
            this.bg.DoWork += new System.ComponentModel.DoWorkEventHandler(bg_DoWork);
            this.bg.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bg_RunWorkerCompleted);
        }

        ~CatalogHelper()
        {
            this.bg.DoWork -= new System.ComponentModel.DoWorkEventHandler(bg_DoWork);
            this.bg.RunWorkerCompleted -= new System.ComponentModel.RunWorkerCompletedEventHandler(bg_RunWorkerCompleted);
        }
        public bool Do(CatalogServiceOption option)
        {
            lock (Locker)
            {
                if (!Running)
                {
                    Running = true;
                    bg.RunWorkerAsync(option);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        private void bg_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var option = (CatalogServiceOption)e.Argument;
            List<CatalogInfo> catalogs = new List<CatalogInfo>();
            string content = ICSharpCode.AvalonEdit.Utils.FileReader.ReadFileContent(option.Full, option.Encoding??Encoding.UTF8);
            int start = 0;
            int offset;
            while ((offset = NewLineHelper.FindNextNewLine(content, start, out string newline)) > -1)
            {
                var line = content.Substring(start, offset - start);
                line = Regex.Replace(line, @"[\x01-\x1F,\x7F]", "").Trim();
                if (!string.IsNullOrWhiteSpace(line))
                {
                    if (IsCatalog(line))
                    {
                        catalogs.Add(new CatalogInfo() { Offset = start, Text = line });
                    }
                }
                start = offset + newline.Length;
            }
            e.Result = new Result<List<CatalogInfo>>(ErrorCode.SUCC, "", catalogs);
        }
        private void bg_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Running = false;
            if (e.Error != null)
            {
                OnWorkerCompleted(this, new Result<List<CatalogInfo>>(ErrorCode.UNKNOWN, e.Error.ToString()));
            }
            else if (e.Cancelled)
            {
                OnWorkerCompleted(this, new Result<List<CatalogInfo>>(ErrorCode.UNKNOWN, "操作已取消"));
            }
            else
            {
                OnWorkerCompleted(this, (Result<List<CatalogInfo>>)e.Result);
            }
        }
        public static bool IsCatalog(string input)
        {
            var r = false;
            //foreach (var reg in REGS)
            //{
            //    if (reg.IsMatch(input) && input.Length<=20)
            //    {
            //        return true;
            //    }
            //}
            if (input.Length <= 30)
            {
                return REG.IsMatch(input);
            }
            return r;
        }
    }

    public class CatalogServiceOption
    {
        public string Name { get; set; }
        public string Full { get; set; }
        public Encoding Encoding { get; set; }
    }
}
