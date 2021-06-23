using System.IO;
using Microsoft.Extensions.Configuration;

namespace YuKitsune.Configuration.Env
{
    /// <summary>
    /// An ENV file based <see cref="ConfigurationProvider"/>.
    /// </summary>
    /// <examples>
    /// key1=value1
    /// # comment
    /// </examples>
    public class EnvConfigurationProvider : FileConfigurationProvider
    {
        /// <summary>
        /// Initializes a new instance with the specified source.
        /// </summary>
        /// <param name="source">The source settings.</param>
        public EnvConfigurationProvider(EnvConfigurationSource source) : base(source) { }

        /// <summary>
        /// Loads the ENV data from a stream.
        /// </summary>
        /// <param name="stream">The stream to read.</param>
        public override void Load(Stream stream)
            => Data = EnvStreamConfigurationProvider.Read(stream);
    }
}