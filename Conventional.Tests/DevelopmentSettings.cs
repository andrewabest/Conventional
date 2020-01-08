using System.IO;
using Newtonsoft.Json;

namespace Conventional.Tests
{
    public class DevelopmentSettings
    {
        private DevelopmentSettings()
        {
        }

#if DEBUG
        public string ConnectionString { get; set; } = @"Server=.\SQLEXPRESS;Database=Conventional;Integrated Security=true;";
#else
        public string ConnectionString { get; set; } = @"Server=(local)\SQL2017;Database=Conventional;User ID=sa;Password=Password12!";
#endif

        public static DevelopmentSettings Create()
        {
            DevelopmentSettings settings = null;

            var developmentSettingsFile = Path.Combine(KnownPaths.SolutionRoot, "development.json");
            if (File.Exists(developmentSettingsFile))
            {
                settings = JsonConvert.DeserializeObject<DevelopmentSettings>(File.ReadAllText(developmentSettingsFile));
            }

            return settings ?? new DevelopmentSettings();
        }
    }
}