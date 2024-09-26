using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TxtTags.Entity
{
    public class BookmarkInfo: IComparable<BookmarkInfo>
    {
        public int Offset { get; set; }
        public string Text { get; set; }
        public int CompareTo(BookmarkInfo other)
        {
            if (this.Offset < other.Offset)
                return -1;
            else if (this.Offset > other.Offset)
                return 1;
            else
                return 0;
        }
    }
}
