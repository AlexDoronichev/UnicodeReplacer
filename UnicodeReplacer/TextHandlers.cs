using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.IO;

namespace UnicodeReplacer
{
    public static class TextHandlers
    {
        static string progName = "UnicodeReplacer";

        public static HashSet<int> validCodesHashSet = new HashSet<int>();

        // Text processing methods
        public static string CutFileFormat(string filename)
        {
            int delimInd = filename.LastIndexOf('.');
            if (delimInd < 0)
                return filename;

            string format = filename.Substring(delimInd + 1, filename.Length - delimInd - 1);

            return format.Contains(" ") ? filename : filename.Substring(0, delimInd);
        }

        public static string GetFileFormat(string filename)
        {
            int delimInd = filename.LastIndexOf('.');
            if (delimInd < 0)
                return string.Empty;

            string format = filename.Substring(delimInd, filename.Length - delimInd);

            return format.Contains(" ") ? string.Empty : format;
        }

        public static bool IsCharCyrilic(int charCode)
        {
            return validCodesHashSet.Contains(charCode) || charCode < 128;
        }

        public static bool IsCharCyrilic(char c)
        {
            int charCode = (int)c;
            return validCodesHashSet.Contains(charCode) || charCode < 128;
        }

        public static bool IsUnicodeInText(string str)
        {
            foreach (char c in str)
                if (!IsCharCyrilic(c))
                    return true;

            return false;
        }

        public static string GetUnicodeFromText(string str)
        {
            string unicodeStr = string.Empty;

            foreach (char c in str)
                if (!IsCharCyrilic(c))
                    unicodeStr += c;

            return unicodeStr;
        }

        public static string GetNewFilename(string fullPath)
        {
            int count = 1;

            string fileNameOnly = Path.GetFileNameWithoutExtension(fullPath);
            string extension = Path.GetExtension(fullPath);
            string path = Path.GetDirectoryName(fullPath);
            string newFullPath = fullPath;

            while (File.Exists(newFullPath))
            {
                string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                newFullPath = Path.Combine(path, tempFileName + extension);
            }

            return newFullPath;
        }
    }
}
