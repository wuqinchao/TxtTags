using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TxtTags.Common
{
    public struct TextSegment
    {
        public static readonly TextSegment Invalid = new TextSegment(-1, -1);

        private int offset, length;

        public int Offset { get => offset; }
        public int Length { get => length; }
        public int EndOffset { get => offset + length; }
        public TextSegment(int offset, int length)
        {
            this.offset = offset;
            this.length = length;
        }
        public TextSegment(TextSegment segment)
        {
            Debug.Assert(segment != null);
            this.offset = segment.Offset;
            this.length = segment.Length;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                return Offset + 10301 * Length;
            }
        }
        public override bool Equals(object obj)
        {
            return (obj is TextSegment) && Equals((TextSegment)obj);
        }
        public bool Equals(TextSegment other)
        {
            return this.Offset == other.Offset && this.Length == other.Length;
        }
        public static bool operator ==(TextSegment left, TextSegment right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(TextSegment left, TextSegment right)
        {
            return !left.Equals(right);
        }
        public override string ToString()
        {
            return "[Offset=" + Offset.ToString(CultureInfo.InvariantCulture) + ", Length=" + Length.ToString(CultureInfo.InvariantCulture) + "]";
        }
    }
}
