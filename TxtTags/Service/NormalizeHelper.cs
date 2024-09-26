using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using TxtTags.Common;
using TxtTags.Dialog;
using TxtTags.Entity;

namespace TxtTags.Service
{
    public delegate void NormalizeServiceCompleted(object sender, Result<string> result);
    public class NormalizeHelper
    {
        /// <summary>
        /// 强制分段标志字符，除非此符号未在成对符号内，否则强制分段
        /// </summary>
        static readonly char[] SECTION = { '。', '”' };
        /// <summary>
        /// 成对符号中间不分段
        /// </summary>
        static readonly char[] EXSECTION = new char[] { '”', '“', '(', ')', '（', '）', '[', ']', '【', '】' };
        /// <summary>
        /// 前面发现的是成对符号中的闭合符号时可以分段
        /// </summary>
        static readonly char[] CanSection = new char[] { '”', ')', '）', ']', '】' };
        private const string REGSTR_HEAD_SPACE = @"^(\s*)";
        private static readonly Regex REG_HEAD_SPACE = new Regex(REGSTR_HEAD_SPACE);
        /// <summary>
        /// 无意义空行字符, 如果一行全是这些字符和空格，将被去除
        /// </summary>
        private const string REGSTR_NOMEANING = @"^([-=*&%#☆★○●◎◇◆□℃‰€■△▲※→←↑↓〓¤＃＆＠＼︿＿￣―♂♀°。，\s]*)$";
        private static readonly Regex REG_NOMEANING = new Regex(REGSTR_NOMEANING);
        /// <summary>
        /// 可以作为行尾的字符，否则会去掉去掉行尾的换行符（标题除外）
        /// </summary>
        public readonly Regex REG_ALLOW_NEWLINE_PUNCTUATION = new Regex(@"([。”！!》】？）\?\)]{1,})$");
        /// <summary>
        /// 不能作为行首，发现后会去掉上一行的的换行符
        /// </summary>
        public readonly char[] REG_NOTALLOW_NEWLINE_PUNCTUATION = { '.', '。', ',', '，', '”', '；', '！', '?', '？' };
        #region 单例
        public static NormalizeHelper Instance { get; set; }

        static NormalizeHelper()
        {
            Instance = new NormalizeHelper();
        }

        #endregion
        private object Locker = new object();
        private System.ComponentModel.BackgroundWorker bg = new System.ComponentModel.BackgroundWorker();
        public event NormalizeServiceCompleted OnWorkerCompleted;
        private bool _Running = false;
        public bool Running
        {
            get => _Running;
            private set => _Running = value;
        }
        private NormalizeHelper()
        {
            this.bg.WorkerReportsProgress = true;
            this.bg.WorkerSupportsCancellation = true;
            this.bg.DoWork += new System.ComponentModel.DoWorkEventHandler(bg_DoWork);
            this.bg.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bg_RunWorkerCompleted);
        }

        ~NormalizeHelper()
        {
            this.bg.DoWork -= new System.ComponentModel.DoWorkEventHandler(bg_DoWork);
            this.bg.RunWorkerCompleted -= new System.ComponentModel.RunWorkerCompletedEventHandler(bg_RunWorkerCompleted);
        }
        public bool Do(NormalizeOption option)
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
            var option = (NormalizeOption)e.Argument;
            string content = FileReader.ReadFileContent(option.Full, option.Encoding??Encoding.UTF8);
            // 规范换行
            if (option.Newline)
            {
                content = NewLineHelper.NormalizeNewLines(content, NewLineHelper.NewLine);
            }
            // 分段
            if (option.Section)
            {
                content = content.Replace('「', '“').Replace('」', '”');
                content = MarkSection(content);
            }
            // 保留一个空行
            if (option.OneNewline)
            {
                content = Regex.Replace(content, @"(\r\n\s*){1,}", $"{NewLineHelper.NewLine}{NewLineHelper.NewLine}");
            }
            // 段头统一用\t
            if (option.Linehead)
            {
                content = NormalizeLineHead(content, "\t");
            }
            else
            {
                content = NormalizeLineHead(content, string.Empty);
            }
            // 删除无意义的空行
            if(option.MeaninglessLine)
            {
                content = MeaninglessLine(content);
            }
            // 保留一个空行
            if (option.OneNewline)
            {
                content = Regex.Replace(content, @"(\r\n){1,}", $"{NewLineHelper.NewLine}{NewLineHelper.NewLine}");
            }
            if (option.Newline)
            {
                content = MistakeNewline(content);
            }
            if (option.Simplified)
            {
                content = content.ToSimplified();
            }
            if (option.SbctoDbc)
            {
                content = content.ConvertSBCToDBC();
            }
            File.WriteAllText(option.Full, content, Encoding.UTF8);
            e.Result = new Result<string>(ErrorCode.SUCC, "", content);
        }
        private void bg_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Running = false;
            if (e.Error != null)
            {
                OnWorkerCompleted(this, new Result<string>(ErrorCode.UNKNOWN, e.Error.ToString()));
            }
            else if (e.Cancelled)
            {
                OnWorkerCompleted(this, new Result<string>(ErrorCode.UNKNOWN, "操作已取消"));
            }
            else
            {
                OnWorkerCompleted(this, (Result<string>)e.Result);
            }
        }
        /// <summary>
        /// 无意义行检查
        /// </summary>
        public string MeaninglessLine(string input)
        {
            if (input == null) return null;

            StringBuilder b = new StringBuilder(input.Length);
            int start = 0, offset;
            string line;
            while ((offset = NewLineHelper.FindNextNewLine(input, start, out string newline)) > -1)
            {
                line = input.Substring(start, offset + newline.Length - start);
                if(!IsMeaninglessLine(line))
                {
                    b.Append(line);
                }
                start = offset + newline.Length;
            }
            line = input.Substring(start, input.Length - start);
            if (!IsMeaninglessLine(line))
            {
                b.Append(line);
            }
            return b.ToString();
        }
        /// <summary>
        /// 是否是无意义的行
        /// </summary>
        private bool IsMeaninglessLine(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return true;
            var line = input.Trim();
            if (string.IsNullOrWhiteSpace(line)) return true;
            if (REG_HEAD_SPACE.IsMatch(line))
            {
                line = Regex.Replace(line, REGSTR_HEAD_SPACE, "");
                if (string.IsNullOrWhiteSpace(line)) return true;
            }
            if(REG_NOMEANING.IsMatch(line))
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// 构造段落
        /// </summary>
        private void BuildLineWithHead(string input, string head, StringBuilder sb)
        {
            var line = input;
            if (REG_HEAD_SPACE.IsMatch(line))
            {
                line = Regex.Replace(line, REGSTR_HEAD_SPACE, head);
            }
            if (head == line || string.IsNullOrWhiteSpace(line))
            {
                sb.Append(NewLineHelper.NewLine);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(head) && !line.StartsWith(head))
                {
                    sb.Append(head);
                }
                sb.Append(line);
            }
        }
        /// <summary>
        /// 规范化段头
        /// </summary>
        public string NormalizeLineHead(string input, string head = "\t")
        {
            if (input == null)
                return null;

            StringBuilder b = new StringBuilder(input.Length);
            int start = 0, offset;
            string line;
            while ((offset = NewLineHelper.FindNextNewLine(input, start, out string newline)) > -1)
            {
                line = input.Substring(start, offset + newline.Length - start).TrimStart();
                BuildLineWithHead(line, head, b);
                start = offset + newline.Length;
            }
            line = input.Substring(start, input.Length - start);
            BuildLineWithHead(line, head, b);
            return b.ToString();
        }
        /// <summary>
        /// 文本分段
        /// </summary>
        public string MarkSection(string input)
        {
            if (input == null) return null;
            StringBuilder b = new StringBuilder(input.Length);
            int start = 0, offset;
            string line;
            while ((offset = NextSection(input, start, out string newline)) > -1)
            {
                line = input.Substring(start, offset + newline.Length - start);
                b.Append(line);
                // 检查可分段位置
                var pre = input.LastIndexOfAny(EXSECTION, offset, offset);
                // 检查是否在成对符号中间
                if (pre < 0 || CanSection.Contains(input[pre]))
                {
                    if (input.Length > offset + 1 && input[offset + 1] != '\r')
                    {
                        b.Append(NewLineHelper.NewLine);
                    }
                }
                start = offset + newline.Length;
            }
            line = input.Substring(start, input.Length - start);
            b.Append(line);
            return b.ToString();
        }

        /// <summary>
        /// 查找下一个分段位置
        /// </summary>
        public static int NextSection(string text, int offset, out string mark)
        {
            // 找到下一个强制分段字符
            int pos = text.IndexOfAny(SECTION, offset);
            if (pos >= 0)
            {
                // 连续的分段符, 取后一个, 例如。”或”。
                if (pos + 1 < text.Length && SECTION.Contains(text[pos + 1]))
                {
                    mark = (text[pos + 1]).ToString();
                    return pos + 1;
                }
                mark = (text[pos]).ToString();
                return pos;
            }
            mark = string.Empty;
            return -1;
        }
        /// <summary>
        /// 修改分段错误
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string MistakeNewline(string input)
        {
            if (input == null)
                return null;

            StringBuilder b = new StringBuilder(input.Length);
            int start = 0, offset;
            string line;
            //string preLine = string.Empty;
            bool allowNewline = true;
            while ((offset = NewLineHelper.FindNextNewLine(input, start, out string newline)) > -1)
            {
                line = input.Substring(start, offset + newline.Length - start);
                start = offset + newline.Length;
                string clearLine = line.Trim();

                if (string.IsNullOrWhiteSpace(clearLine))
                {
                    if (allowNewline)
                        b.Append(line);
                    continue;
                }
                // 如果是标题，强制下行换行
                bool isCatalog = CatalogHelper.IsCatalog(clearLine);
                if (isCatalog)
                {
                    b.Append("\r\n");
                    b.Append(line.TrimStart());
                    b.Append("\r\n");
                    allowNewline = true;
                    continue;
                }
                if(clearLine.StartsWith("作者："))
                {
                    b.Append(line);
                    allowNewline = true;
                    continue;
                }
                if (clearLine.StartsWith("内容简介：") || clearLine.StartsWith("简介："))
                {
                    b.Append(line);
                    allowNewline = true;
                    continue;
                }
                // 判断下行是否需要换行
                if (REG_ALLOW_NEWLINE_PUNCTUATION.IsMatch(clearLine))
                {
                    // 判断行首是否合规, 如果第一个字符在不可换行的范围内, 去除上一行的换行符
                    if (REG_NOTALLOW_NEWLINE_PUNCTUATION.Contains(clearLine[0]))
                    {
                        do
                        {
                            b.Length -= 2;
                        }
                        while (b.Length >= 2 && b[b.Length - 1] == '\n');
                        b.Append(line.TrimStart());
                    }
                    else
                    {
                        b.Append(allowNewline ? line : line.TrimStart());
                    }
                    allowNewline = true;
                }
                else
                {
                    b.Append(allowNewline ? line.TrimEnd() : line.Trim());
                    allowNewline = false;
                }
            }
            line = input.Substring(start, input.Length - start);
            b.Append(allowNewline ? line : line.TrimStart());
            return b.ToString();
        }
    }

    public class NormalizeOption : NotifyPropertyBase
    {
        private string _name;
        public string Name { get=>_name; set { _name = value; OnPropertyChanged("Name"); } }
        private string _full;
        public string Full { get => _full; set { _full = value; OnPropertyChanged("Full"); } }
        private Encoding _encoding;
        public Encoding Encoding { get => _encoding; set { _encoding = value; OnPropertyChanged("Encoding"); } }
        //规范换行
        private bool _newline = true;
        /// <summary>
        /// 规范换行为\r\n
        /// </summary>
        public bool Newline { get => _newline; set { _newline = value; OnPropertyChanged("Newline"); } }
        //分段
        private bool _section = true;
        /// <summary>
        /// 分段
        /// </summary>
        public bool Section { get => _section; set { _section = value; OnPropertyChanged("Section"); } }
        //最多只保留一个空行//规范换行
        private bool _oneNewline = true;
        /// <summary>
        /// 最多只保留一个空行
        /// </summary>
        public bool OneNewline { get => _oneNewline; set { _oneNewline = value; OnPropertyChanged("OneNewline"); } }
        //去除无意义行+++++
        private bool _meaninglessLine = true;
        /// <summary>
        /// 去除无意义行
        /// </summary>
        public bool MeaninglessLine { get => _meaninglessLine; set { _meaninglessLine = value; OnPropertyChanged("MeaninglessLine"); } }
        //段头统一用\t
        private bool _linehead = true;
        /// <summary>
        /// 段头统一用\t
        /// </summary>
        public bool Linehead { get => _linehead; set { _linehead = value; OnPropertyChanged("Linehead"); } }
        //转简体
        private bool _simplified = true;
        /// <summary>
        /// 转为简体中文
        /// </summary>
        public bool Simplified { get => _simplified; set { _simplified = value; OnPropertyChanged("Simplified"); } }
        //全角数字英文转半角
        private bool _SbctoDbc = true;
        /// <summary>
        /// 全角数字英文转半角
        /// </summary>
        public bool SbctoDbc { get => _SbctoDbc; set { _SbctoDbc = value; OnPropertyChanged("SbctoDbc"); } }
        /// <summary>
        /// 是否有至少一项需要整理的内容
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return Newline || Section || OneNewline || MeaninglessLine || Linehead || Simplified;
        }
    }
}
