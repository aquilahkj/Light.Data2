using System;
using System.IO;
using System.Reflection;

namespace Light.Data
{
    internal static class FileHelper
    {
        public static FileInfo GetFileInfo(string path, out bool absolute)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));
            var c = path[0];
            if (c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar)
            {
                absolute = true;
                return new FileInfo(path);
            }
            else if (Environment.OSVersion.Platform == PlatformID.Win32NT && path.Length >= 2 && path[1] == ':' && ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z')))
            {
                absolute = true;
                return new FileInfo(path);
            }
            else
            {
                

                var gg = AppContext.BaseDirectory;
                absolute = false;
                var currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                var newPath = Path.Combine(currentDirectory, path);
                return new FileInfo(newPath);
            }
        }
    }
}
