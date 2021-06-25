using System.IO;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace YuKitsune.Configuration.Env.Tests
{
    public class EnvConfigurationSourceTests
    {
        [Fact]
        public void CanLoadDotPrefixedFiles()
        {
            var tempDirectory = Path.GetTempPath();
            var envFile = Path.Combine(tempDirectory, ".env.test");
            File.WriteAllText(envFile,"var=val");

            var config = new ConfigurationBuilder().AddEnvFile(envFile).Build();
            Assert.Equal("val", config["var"]);
        }
    }
}