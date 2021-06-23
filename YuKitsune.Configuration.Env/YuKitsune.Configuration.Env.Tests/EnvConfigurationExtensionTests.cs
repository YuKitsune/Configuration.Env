using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace YuKitsune.Configuration.Env.Tests
{
    public class EnvConfigurationExtensionsTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void AddEnvFile_ThrowsIfFilePathIsNullOrEmpty(string path)
        {
            // Arrange
            var configurationBuilder = new ConfigurationBuilder();

            // Act and Assert
            var ex = Assert.Throws<ArgumentException>(
                () => EnvConfigurationExtensions.AddEnvFile(configurationBuilder, path));
            Assert.Equal("path", ex.ParamName);
            Assert.StartsWith("File path must be a non-empty string.", ex.Message);
        }

        [Fact]
        public void AddEnvFile_ThrowsIfFileDoesNotExistAtPath()
        {
            // Arrange
            var path = "file-does-not-exist.env";
 
            // Act and Assert
            var ex = Assert.Throws<FileNotFoundException>(() => new ConfigurationBuilder().AddEnvFile(path).Build());
            Assert.StartsWith(
                $"The configuration file '{path}' was not found and is not optional.",
                ex.Message);
        }

        [Fact]
        public void AddEnvFile_DoesNotThrowsIfFileDoesNotExistAtPathAndOptional()
        {
            // Arrange
            var path = "file-does-not-exist.env";

            // Act and Assert
            new ConfigurationBuilder().AddEnvFile(path, optional: true).Build();
        }

    }
}