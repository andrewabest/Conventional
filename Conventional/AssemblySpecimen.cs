namespace Conventional
{
    public class AssemblySpecimen
    {
        public string ProjectFilePath { get; private set; }

        public AssemblySpecimen(string projectFilePath)
        {
            ProjectFilePath = projectFilePath;
        }
    }
}