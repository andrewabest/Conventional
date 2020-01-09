using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Conventional.Extensions
{
    internal static class DirectoryEx
    {
        private static readonly string[] ProjectOutputFolders = { "obj", "bin" };

        public static IEnumerable<string> GetFilesExceptOutput(
            string rootPath,
            string filePattern)
        {
            return GetFiles(rootPath,
                x => ProjectOutputFolders.Select(folder => $"\\{folder}\\").None(x.Contains), filePattern);
        }

        public static IEnumerable<string> GetFiles(
            string rootPath,
            Func<string, bool> directoryFilterPredicate,
            string filePattern)
        {
            foreach (var file in Directory.GetFiles(rootPath, filePattern, SearchOption.TopDirectoryOnly))
            {
                yield return file;
            }

            var directories = Directory.GetDirectories(rootPath, "*.*", SearchOption.TopDirectoryOnly)
                .Where(directoryFilterPredicate);

            foreach (var dir in directories)
            {
                foreach (var file in GetFiles(dir, directoryFilterPredicate, filePattern))
                {
                    yield return file;
                }
            }
        }
    }
}