using System.IO;
using System.Reflection;

namespace JJ.SmartHome.Tests.Mocks.Fixtures
{
    public class FixtureUtils
    {
        public static string LoadFixture(string fixture)
        {
            var payloadStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{typeof(FixtureUtils).Namespace}.{fixture}");
            using (var reader = new StreamReader(payloadStream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
