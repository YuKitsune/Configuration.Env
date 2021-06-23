using Microsoft.Extensions.Configuration;

namespace YuKitsune.Configuration.Env
{
    
    /// <summary>
    /// Represents an ENV file as an <see cref="IConfigurationSource"/>.
    /// </summary>
    /// <examples>
    /// key1=value1
    /// # comment
    /// </examples>
    public class EnvStreamConfigurationSource : StreamConfigurationSource
    {
        /// <summary>
        /// Builds the <see cref="EnvConfigurationProvider"/> for this source.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
        /// <returns>An <see cref="EnvConfigurationProvider"/></returns>
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
            => new EnvStreamConfigurationProvider(this);
    }
}