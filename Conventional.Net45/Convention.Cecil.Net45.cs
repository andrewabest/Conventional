using Conventional.Conventions.Cecil;

namespace Conventional
{
    public static partial class Convention
    {
        public static LibraryCodeShouldCallConfigureAwaitWhenAwaitingTasksConventionSpecification LibraryCodeShouldCallConfigureAwaitWhenAwaitingTasks
        {
            get { return new LibraryCodeShouldCallConfigureAwaitWhenAwaitingTasksConventionSpecification(); }
        }
    }
}