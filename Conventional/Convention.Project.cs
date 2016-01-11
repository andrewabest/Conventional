using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Conventional.Conventions.Projects;

namespace Conventional
{
    public static partial class Convention
    {
        public static MustIncludeAllMatchingFilesInFolderConventionSpecification MustIncludeAllMatchingFilesInFolder(string filePattern)
        {
            return new MustIncludeAllMatchingFilesInFolderConventionSpecification(filePattern);
        }
    }
}
