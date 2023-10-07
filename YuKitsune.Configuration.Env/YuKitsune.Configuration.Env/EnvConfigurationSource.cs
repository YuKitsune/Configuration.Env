using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;

namespace YuKitsune.Configuration.Env
{
    /// <summary>
    /// Represents an Env file as an <see cref="IConfigurationSource"/>.
    /// </summary>
    /// <examples>
    /// key1=value1
    /// # comment
    /// </examples>
    public class EnvConfigurationSource : FileConfigurationSource
    {
        /// <summary>
        /// Builds the <see cref="EnvConfigurationProvider"/> for this source.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
        /// <returns>An <see cref="EnvConfigurationProvider"/></returns>
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            // This is necessary as the default PhysicalFileProvider filters out dot-prefixed files (.env)
            FileProvider ??= MakeFileProvider(AppContext.BaseDirectory);

            EnsureDefaults(builder);
            return new EnvConfigurationProvider(this);
        }

        /// <summary>
        /// If no file provider has been set, for absolute Path, this will creates a physical file provider
        /// for the nearest existing directory.
        /// </summary>
        public new void ResolveFileProvider()
        {
            if (FileProvider == null &&
                !string.IsNullOrEmpty(Path) &&
                System.IO.Path.IsPathRooted(Path))
            {
                string directory = System.IO.Path.GetDirectoryName(Path);
                string pathToFile = System.IO.Path.GetFileName(Path);
                while (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    pathToFile = System.IO.Path.Combine(System.IO.Path.GetFileName(directory), pathToFile);
                    directory = System.IO.Path.GetDirectoryName(directory);
                }

                if (Directory.Exists(directory))
                {
                    FileProvider = MakeFileProvider(directory);
                    Path = pathToFile;
                }
            }
        }

        IFileProvider MakeFileProvider(string path) => new PhysicalFileProvider(path, ExclusionFilters.System);
    }
}