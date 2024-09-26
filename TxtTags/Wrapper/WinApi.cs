using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TxtTags
{
    public class WinApi
    {
        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        //[DllImport("msvcrt.dll")]
        //private static extern unsafe int memcmp(byte* b1, byte* b2, int count);

        //public static unsafe bool MemcmpCompare(byte[] x, byte[] y, int len)
        //{
        //    fixed (byte* xPtr = x, yPtr = y)
        //    {
        //        return memcmp(xPtr, yPtr, len) == 0;
        //    }
        //}
        [DllImport("shell32.dll", ExactSpelling = true)]
        public static extern void ILFree(IntPtr pidlList);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern IntPtr ILCreateFromPathW(string pszPath);

        [DllImport("shell32.dll", ExactSpelling = true)]
        public static extern int SHOpenFolderAndSelectItems(IntPtr pidlList, uint cild, IntPtr children, uint dwFlags);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteFile(string lpFileName);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CopyFile(string lpExistingFileName, string lpNewFileName, bool bFailIfExists);

        [DllImport("kernel32.dll")]
        public static extern Boolean AllocConsole();

        [DllImport("kernel32.dll")]
        public static extern Boolean FreeConsole();

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("kernel32.dll")]
        private static extern int GetConsoleOutputCP();

        [DllImport("gdi32")]
        public static extern int DeleteObject(IntPtr o); 
        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern int SHFileOperation([In] ref SHFILEOPSTRUCT lpFileOp);
    }
    public enum FileFuncFlags : uint
    {
        FO_MOVE = 0x1,
        FO_COPY = 0x2,
        FO_DELETE = 0x3,
        FO_RENAME = 0x4
    }
    [Flags]
    public enum FILEOP_FLAGS : ushort
    {
        /// <summary>
        /// pTo 指定了多个目标文件，而不是单个目录
        /// </summary>
        FOF_MULTIDESTFILES = 0x1,
        /// <summary>
        /// 未使用
        /// </summary>
        FOF_CONFIRMMOUSE = 0x2,
        /// <summary>
        /// 不显示一个进度对话框
        /// </summary>
        FOF_SILENT = 0x4,
        /// <summary>
        /// 碰到有抵触的名字时，自动分配前缀
        /// </summary>
        FOF_RENAMEONCOLLISION = 0x8,
        /// <summary>
        /// 不对用户显示提示
        /// </summary>
        FOF_NOCONFIRMATION = 0x10,
        /// <summary>
        /// 填充 hNameMappings 字段，必须使用 SHFreeNameMappings 释放
        /// </summary>
        FOF_WANTMAPPINGHANDLE = 0x20,
        /// <summary>
        /// 允许撤销
        /// </summary>
        FOF_ALLOWUNDO = 0x40,
        /// <summary>
        /// 使用 *.* 时, 只对文件操作
        /// </summary>
        FOF_FILESONLY = 0x80,
        /// <summary>
        /// 简单进度条，意味者不显示文件名。
        /// </summary>
        FOF_SIMPLEPROGRESS = 0x100,
        /// <summary>
        /// 建新目录时不需要用户确定
        /// </summary>
        FOF_NOCONFIRMMKDIR = 0x200,
        /// <summary>
        /// 不显示出错用户界面
        /// </summary>
        FOF_NOERRORUI = 0x400,
        /// <summary>
        /// 版本 4.71。 请勿复制文件的安全属性。 目标文件接收其新文件夹的安全属性。
        /// </summary>
        FOF_NOCOPYSECURITYATTRIBS = 0x800,
        /// <summary>
        /// 不递归目录
        /// </summary>
        FOF_NORECURSION = 0x1000,
        /// <summary>
        /// 版本 5.0。 不要将连接的文件作为组移动。 仅移动指定的文件。
        /// </summary>
        FOF_NO_CONNECTED_ELEMENTS = 0x2000,
        /// <summary>
        /// 版本 5.0。 如果在删除操作期间永久销毁文件而不是回收文件，则发送警告。 此标志部分覆盖 FOF_NOCONFIRMATION。
        /// </summary>
        FOF_WANTNUKEWARNING = 0x4000,
        /// <summary>
        /// 未使用
        /// </summary>
        FOF_NORECURSEREPARSE = 0x8000
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct SHFILEOPSTRUCT
    {
        /// <summary>
        /// 对话框的窗口句柄，用于显示有关文件操作状态的信息
        /// </summary>
        public IntPtr hwnd;
        public FileFuncFlags wFunc;
        /// <summary>
        /// 指向一个或多个源文件名称的指针。 这些名称应是完全限定的路径，以防止意外的结果。
        /// </summary>
        /// <remarks>
        /// 注意 此字符串必须以双 null 结尾。
        /// </remarks>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pFrom;
        /// <summary>
        /// 指向目标文件或目录名称的指针。 如果未使用此参数，则必须将其设置为 NULL。 不允许使用通配符。
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pTo;
        public FILEOP_FLAGS fFlags;
        /// <summary>
        /// 当函数返回时，如果任何文件操作在完成之前中止，则此成员包含 TRUE;否则，FALSE。 如果设置了FOF_NOERRORUI或FOF_NOCONFIRMATION标志，则可以通过 UI 手动中止操作，或者系统可以以无提示方式中止该操作。
        /// </summary>
        [MarshalAs(UnmanagedType.Bool)]
        public bool fAnyOperationsAborted;
        /// <summary>
        /// 函数返回时，此成员包含名称映射对象的句柄，该对象包含重命名文件的旧名称和新名称。 仅当 fFlags 成员包含 FOF_WANTMAPPINGHANDLE 标志时，才使用此成员。
        /// </summary>
        public IntPtr hNameMappings;
        /// <summary>
        /// 指向进度对话框标题的指针。 这是以 null 结尾的字符串。 仅当 fFlags 包含 FOF_SIMPLEPROGRESS 标志时，才使用此成员。
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpszProgressTitle;
    }
}
