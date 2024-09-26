using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TxtTags.Common
{
    public static class NewLineHelper
    {
        public const string NewLine = "\r\n";

        static readonly char[] newline = { '\r', '\n' };

        public static readonly string[] NewlineStrings = { "\r\n", "\r", "\n" };

        /// <summary>
        /// Gets the location of the next new line character, or SimpleSegment.Invalid
        /// if none is found.
        /// </summary>
        public static TextSegment NextNewLine(string text, int offset)
        {
            int pos = text.IndexOfAny(newline, offset);
            if (pos >= 0)
            {
                if (text[pos] == '\r')
                {
                    if (pos + 1 < text.Length && text[pos + 1] == '\n')
                        return new TextSegment(pos, 2);
                }
                return new TextSegment(pos, 1);
            }
            return TextSegment.Invalid;
        }
        /// <summary>
		/// 查找文本中的换行符的位置, 如果没有找到则返回-1
		/// </summary>
		/// <param name="text">文本</param>
		/// <param name="offset">开始查找的位置</param>
		/// <param name="newLineType">如果有换行符,则返回该换行符</param>
		public static int FindNextNewLine(string text, int offset, out string newLineType)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                if (offset >= 0 && offset <= text.Length)
                {
                    TextSegment s = NextNewLine(text, offset);
                    if (s != TextSegment.Invalid)
                    {
                        if (s.Length == 2)
                        {
                            newLineType = "\r\n";
                        }
                        else if (text[s.Offset] == '\n')
                        {
                            newLineType = "\n";
                        }
                        else
                        {
                            newLineType = "\r";
                        }
                        return s.Offset;
                    }
                }
            }
            newLineType = null;
            return -1;
        }
        /// <summary>
		/// 是否是有效的换行符
		/// </summary>
		public static bool IsNewLine(string newLine)
        {
            return newLine == "\r\n" || newLine == "\n" || newLine == "\r";
        }
        /// <summary>
        /// 文本中是否存在换行
        /// </summary>
        public static bool ExistNewLine(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;
            TextSegment ds = NextNewLine(input, 0);
            return ds != TextSegment.Invalid;
        }
        /// <summary>
        /// 重新规范化文本中的换行符
        /// </summary>
        public static string NormalizeNewLines(string input, string newLine = "\r\n")
        {
            if (input == null)
                return null;
            if (!IsNewLine(newLine))
                throw new ArgumentException("newLine must be one of the known newline sequences");
            TextSegment ds = NextNewLine(input, 0);
            if (ds == TextSegment.Invalid) // text does not contain any new lines
                return input;
            StringBuilder b = new StringBuilder(input.Length);
            int lastEndOffset = 0;
            do
            {
                b.Append(input, lastEndOffset, ds.Offset - lastEndOffset);
                b.Append(newLine);
                lastEndOffset = ds.EndOffset;
                ds = NextNewLine(input, lastEndOffset);
            } while (ds != TextSegment.Invalid);
            // remaining string (after last newline)
            b.Append(input, lastEndOffset, input.Length - lastEndOffset);
            return b.ToString();
        }



        /// <summary>
		/// 查找文本中的换行符的位置, 如果没有找到则返回-1
		/// </summary>
		/// <param name="text">文本</param>
		/// <param name="offset">开始查找的位置</param>
		/// <param name="newLineType">如果有换行符,则返回该换行符</param>
		public static int FindNextSection(string text, int offset, out string newLineType)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                if (offset >= 0 && offset <= text.Length)
                {
                    TextSegment s = NextNewLine(text, offset);
                    if (s != TextSegment.Invalid)
                    {
                        if (s.Length == 2)
                        {
                            newLineType = "\r\n";
                        }
                        else if (text[s.Offset] == '\n')
                        {
                            newLineType = "\n";
                        }
                        else
                        {
                            newLineType = "\r";
                        }
                        return s.Offset;
                    }
                }
            }
            newLineType = null;
            return -1;
        }
    }
}
