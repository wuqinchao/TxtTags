using System;
using System.IO;

namespace TxtTags.Common
{
    public class FilePath
    {
        static FilePath()
        {
            if (!Directory.Exists(AppPath))
            {
                Directory.CreateDirectory(AppPath);
            }
        }
        public static string BaseFolder
        {
            get { return System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData); }
        }
        public static string TempFolder
        {
            get { return Path.GetTempPath(); }
        }
        public static string AppName
        {
            get { return "TxtTags"; }
        }
        public static string AppPath
        {
            get { return $"{BaseFolder}\\{AppName}"; }
        }
        public static string ReposPath
        {
            get { return $"{BaseFolder}\\{AppName}\\repos.json"; }
        }
    }
}
