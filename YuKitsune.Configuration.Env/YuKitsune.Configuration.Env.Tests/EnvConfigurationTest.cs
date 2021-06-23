using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace YuKitsune.Configuration.Env.Tests
{
    public class EnvConfigurationTest
    {
        [Fact]
        public void CanLoadValidEnvFromStreamProvider()
        {
            var env = @"DefaultConnection__ConnectionString=TestConnectionString
DefaultConnection__Provider=SqlClient
Data__Inventory__ConnectionString=AnotherTestConnectionString
Data__Inventory__SubHeader__Provider=MySql";
            var config = new ConfigurationBuilder()
                .AddEnvStream(StreamHelpers.StringToStream(env)).Build();

            Assert.Equal("TestConnectionString", config["defaultconnection:ConnectionString"]);
            Assert.Equal("SqlClient", config["DEFAULTCONNECTION:PROVIDER"]);
            Assert.Equal("AnotherTestConnectionString", config["Data:Inventory:CONNECTIONSTRING"]);
            Assert.Equal("MySql", config["Data:Inventory:SubHeader:Provider"]);
        }

        [Fact]
        public void ReloadThrowsFromEnvStreamProvider()
        {
            var env = @"DefaultConnection__ConnectionString=TestConnectionString
DefaultConnection__Provider=SqlClient
Data__Inventory__ConnectionString=AnotherTestConnectionString
Data__Inventory__SubHeader__Provider=MySql";
            var config = new ConfigurationBuilder()
                .AddEnvStream(StreamHelpers.StringToStream(env)).Build();
            Assert.Throws<InvalidOperationException>(() => config.Reload());
        }

        [Fact]
        public void LoadKeyValuePairsFromValidEnvFile()
        {
            var env = @"DefaultConnection__ConnectionString=TestConnectionString
DefaultConnection__Provider=SqlClient
Data__Inventory__ConnectionString=AnotherTestConnectionString
Data__Inventory__SubHeader__Provider=MySql";
            var envConfigSrc = new EnvConfigurationProvider(new EnvConfigurationSource());

            envConfigSrc.Load(StreamHelpers.StringToStream(env));

            Assert.Equal("TestConnectionString", envConfigSrc.Get("defaultconnection:ConnectionString"));
            Assert.Equal("SqlClient", envConfigSrc.Get("DEFAULTCONNECTION:PROVIDER"));
            Assert.Equal("AnotherTestConnectionString", envConfigSrc.Get("Data:Inventory:CONNECTIONSTRING"));
            Assert.Equal("MySql", envConfigSrc.Get("Data:Inventory:SubHeader:Provider"));
        }

        [Fact]
        public void LoadMethodCanHandleEmptyValue()
        {
            var env = @"DefaultKey=";
            var envConfigSrc = new EnvConfigurationProvider(new EnvConfigurationSource());

            envConfigSrc.Load(StreamHelpers.StringToStream(env));

            Assert.Equal(string.Empty, envConfigSrc.Get("DefaultKey"));
        }

        [Fact]
        public void SupportAndIgnoreComments()
        {
            var env = @"
            # Comments
            DefaultConnection__ConnectionString=TestConnectionString
            # Comments
            DefaultConnection__Provider=SqlClient
            # Comments
            Data__Inventory__ConnectionString=AnotherTestConnectionString
            Data__Inventory__Provider=MySql
            ";
            var envConfigSrc = new EnvConfigurationProvider(new EnvConfigurationSource());

            envConfigSrc.Load(StreamHelpers.StringToStream(env));

            Assert.Equal("TestConnectionString", envConfigSrc.Get("DefaultConnection:ConnectionString"));
            Assert.Equal("SqlClient", envConfigSrc.Get("DefaultConnection:Provider"));
            Assert.Equal("AnotherTestConnectionString", envConfigSrc.Get("Data:Inventory:ConnectionString"));
            Assert.Equal("MySql", envConfigSrc.Get("Data:Inventory:Provider"));
        }

        [Fact]
        public void ThrowExceptionWhenFoundInvalidLine()
        {
            var env = @"
ConnectionString
            ";
            var envConfigSrc = new EnvConfigurationProvider(new EnvConfigurationSource());
            var expectedMsg = "Unrecognized line format: 'ConnectionString'.";

            var exception =
                Assert.Throws<FormatException>(() => envConfigSrc.Load(StreamHelpers.StringToStream(env)));

            Assert.Equal(expectedMsg, exception.Message);
        }

        [Fact]
        public void ThrowExceptionWhenPassingNullAsFilePath()
        {
            var expectedMsg = new ArgumentException("File path must be a non-empty string.", "path").Message;

            var exception = Assert.Throws<ArgumentException>(() => 
                new ConfigurationBuilder().AddEnvFile(path: null));

            Assert.Equal(expectedMsg, exception.Message);
        }

        [Fact]
        public void ThrowExceptionWhenPassingEmptyStringAsFilePath()
        {
            var expectedMsg = new ArgumentException("File path must be a non-empty string.", "path").Message;

            var exception = Assert.Throws<ArgumentException>(() => 
                new ConfigurationBuilder().AddEnvFile(string.Empty));

            Assert.Equal(expectedMsg, exception.Message);
        }

        [Fact]
        public void ThrowExceptionWhenKeyIsDuplicated()
        {
            var env = @"
            Data__DefaultConnection__ConnectionString=TestConnectionString
            Data__DefaultConnection__Provider=SqlClient
            Data__DefaultConnection__ConnectionString=AnotherTestConnectionString
            Data__DefaultConnection__Provider=MySql
            ";
            var envConfigSrc = new EnvConfigurationProvider(new EnvConfigurationSource());
            var expectedMsg = "A duplicate key 'Data:DefaultConnection:ConnectionString' was found.";

            var exception =
                Assert.Throws<FormatException>(() => envConfigSrc.Load(StreamHelpers.StringToStream(env)));

            Assert.Equal(expectedMsg, exception.Message);
        }

        [Fact]
        public void EnvConfiguration_Throws_On_Missing_Configuration_File()
        {
            var exception = Assert.Throws<FileNotFoundException>(() => 
                new ConfigurationBuilder().AddEnvFile("NotExistingConfig.env").Build());

            // Assert
            Assert.StartsWith(
                $"The configuration file 'NotExistingConfig.env' was not found and is not optional.",
                exception.Message);
        }

        [Fact]
        public void EnvConfiguration_Does_Not_Throw_On_Optional_Configuration()
        {
            _ = new ConfigurationBuilder()
                .AddEnvFile("NotExistingConfig.env", optional: true).Build();
        }
    }
}