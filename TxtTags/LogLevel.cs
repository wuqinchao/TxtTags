using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TxtTags
{
    public enum LogLevel
    {
        [Description("不限级别")]
        ALL = 0,
        [Description("调试")]
        Debug = 1,
        [Description("信息")]
        Info = 2,
        [Description("警告")]
        Warn = 3,
        [Description("错误")]
        Error = 4,
        [Description("严重错误")]
        Fatal = 5,
        [Description("关闭日志")]
        None = 10,
    }
}
