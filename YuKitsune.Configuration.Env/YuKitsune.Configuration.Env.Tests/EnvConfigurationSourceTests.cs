using System.IO;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace YuKitsune.Configuration.Env.Tests
{
    public class EnvConfigurationSourceTests
    {
        [Fact]
        public void CanLoadDotPrefixedFiles_FromRootedPath()
        {
            var baseDirectory = Path.GetTempPath();
            var envFile = Path.Combine(baseDirectory, ".env.test");
            File.WriteAllText(envFile,"var=val");

            var config = new ConfigurationBuilder().AddEnvFile(envFile).Build();
            Assert.Equal("val", config["var"]);
        }

        [Fact]
        public void CanLoadDotPrefixedFiles_FromRelativePath()
        {
            var envFile = ".env.test";
            File.WriteAllText(envFile,"var=val");

            var config = new ConfigurationBuilder().AddEnvFile(envFile).Build();
            Assert.Equal("val", config["var"]);
        }
    }
}