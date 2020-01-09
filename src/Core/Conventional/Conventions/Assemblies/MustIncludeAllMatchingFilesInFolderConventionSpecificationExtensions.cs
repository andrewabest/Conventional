namespace Conventional.Conventions.Assemblies
{
    public static class MustIncludeAllMatchingFilesInFolderConventionSpecificationExtensions
    {
        public static MustIncludeAllMatchingFilesInFolderConventionSpecification WithExcludedExtensions(this MustIncludeAllMatchingFilesInFolderConventionSpecification convention, params string[] extensions)
        {
            convention.AddFilenameExtensionExclusions(extensions);
            return convention;
        }
    }
}