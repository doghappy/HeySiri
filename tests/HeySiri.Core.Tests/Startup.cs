using System.Globalization;

namespace HeySiri.Core.Tests;

[TestClass]
internal class Startup
{
    [AssemblyInitialize]
    public static void Initialize(TestContext testContext)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US"); 
    }
}
