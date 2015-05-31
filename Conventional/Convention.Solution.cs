using Conventional.Conventions.Solution;

namespace Conventional
{
    public static partial class Convention
    {
        public static MustOnlyContainInformativeCommentsConventionSpecification MustOnlyContainInformativeComments(string[] permittedCommentDelimiters, string[] fileExemptions, string fileSearchPattern)
        {
            return new MustOnlyContainInformativeCommentsConventionSpecification(permittedCommentDelimiters, fileExemptions, fileSearchPattern);
        }

        public static MustOnlyContainInformativeCommentsConventionSpecification MustOnlyContainToDoAndNoteComments
        {
            get { return new MustOnlyContainInformativeCommentsConventionSpecification(new[] { "Todo", "Note" }, new[] { "AssemblyInfo.cs", "GlobalAssemblyInfo.cs" }, "*.cs"); }
        }
    }
}