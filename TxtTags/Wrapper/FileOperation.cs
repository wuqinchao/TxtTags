using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace TxtTags
{
    public static class FileOperation
    {
        public const string MODULE_NAME = "通用";
        
        public static void Copy(this string sourceFileName, string targetFileName, bool bFailIfExists = true)
        {
            if (File.Exists(sourceFileName))
            {
                WinApi.CopyFile(sourceFileName, targetFileName, bFailIfExists);
            }
        }

        public static void Delete(string path)
        {
            if(File.Exists(path))
            {
                try
                {
                    WinApi.DeleteFile(path);
                }
                catch(Exception e)
                {
                    Console.WriteLine(MODULE_NAME, $"删除{path}异常", e);
                }
            }
        }

        public static void ExplorerFile(string filePath)
        {
            if (!File.Exists(filePath) && !Directory.Exists(filePath))
                return;

            if (Directory.Exists(filePath))
                Process.Start(@"explorer.exe", "/select,\"" + filePath + "\"");
            else
            {
                IntPtr pidlList = WinApi.ILCreateFromPathW(filePath);
                if (pidlList != IntPtr.Zero)
                {
                    try
                    {
                        Marshal.ThrowExceptionForHR(WinApi.SHOpenFolderAndSelectItems(pidlList, 0, IntPtr.Zero, 0));
                    }
                    finally
                    {
                        WinApi.ILFree(pidlList);
                    }
                }
            }
        }
    
        public static int ApiDelete(IEnumerable<string> paths, bool anywhere=false)
        {
            SHFILEOPSTRUCT op = new SHFILEOPSTRUCT()
            {
                hwnd = IntPtr.Zero,
                wFunc = FileFuncFlags.FO_DELETE,
                fFlags = FILEOP_FLAGS.FOF_NOCONFIRMATION | FILEOP_FLAGS.FOF_NOERRORUI,
                fAnyOperationsAborted = true
            };
            if(paths.Count()>1)
            {
                op.fFlags |= FILEOP_FLAGS.FOF_MULTIDESTFILES;
            }
            if (anywhere)
            {
                op.fFlags &= ~FILEOP_FLAGS.FOF_ALLOWUNDO;
            }
            foreach(var f in paths)
            {
                op.pFrom += f + "\0";
            }
            return WinApi.SHFileOperation(ref op);
        }
    }
}
