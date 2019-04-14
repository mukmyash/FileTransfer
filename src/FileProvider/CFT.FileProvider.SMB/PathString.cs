using SharpCifs.Smb;
using System;
using System.Collections.Generic;
using System.Text;

namespace CFT.FileProvider.SMB
{
    internal static class PathString
    {
        public static string PrepareStringPath(string path, string startPath, bool isDirectory)
        {
            return PrepareSubPath(ExtractSubPath(path, startPath), isDirectory);
        }

        private static string ExtractSubPath(string path, string startPath)
        {
            var spanPath = path.AsSpan();

            if (spanPath.StartsWith(startPath.AsSpan()))
                return spanPath.Slice(startPath.Length + 1).ToString();

            if (spanPath.StartsWith("smb://".AsSpan()))
            {
                var startIndex = "smb://".Length;
                var indexFirstSlash = spanPath.Slice(startIndex).IndexOf('/') + startIndex + 1;
                return spanPath.Slice(indexFirstSlash).ToString();
            }

            return path;
        }

        private static string PrepareSubPath(string path, bool isDirectory)
        {
            var spanPath = path.AsSpan();

            if (spanPath.StartsWith("/".AsSpan()))
                spanPath = spanPath.Slice(1);

            if (isDirectory && !spanPath.EndsWith("/".AsSpan()))
                return string.Concat(spanPath.ToString(), "/");

            return spanPath.ToString();
        }
    }
}
