using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace YuKitsune.Configuration.Env
{    
    /// <summary>
    /// Extension methods for adding <see cref="EnvConfigurationProvider"/>.
    /// </summary>
    public static class EnvConfigurationExtensions
    {
        /// <summary>
        /// Adds the ENV configuration provider at <paramref name="path"/> to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="path">Path relative to the base path stored in
        /// <see cref="IConfigurationBuilder.Properties"/> of <paramref name="builder"/>.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddEnvFile(this IConfigurationBuilder builder, string path)
        {
            return AddEnvFile(builder, provider: null, path: path, optional: false, reloadOnChange: false);
        }

        /// <summary>
        /// Adds the ENV configuration provider at <paramref name="path"/> to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="path">Path relative to the base path stored in
        /// <see cref="IConfigurationBuilder.Properties"/> of <paramref name="builder"/>.</param>
        /// <param name="optional">Whether the file is optional.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddEnvFile(this IConfigurationBuilder builder, string path, bool optional)
        {
            return AddEnvFile(builder, provider: null, path: path, optional: optional, reloadOnChange: false);
        }

        /// <summary>
        /// Adds the ENV configuration provider at <paramref name="path"/> to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="path">Path relative to the base path stored in
        /// <see cref="IConfigurationBuilder.Properties"/> of <paramref name="builder"/>.</param>
        /// <param name="optional">Whether the file is optional.</param>
        /// <param name="reloadOnChange">Whether the configuration should be reloaded if the file changes.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddEnvFile(
            this IConfigurationBuilder builder,
            string path,
            bool optional,
            bool reloadOnChange)
        {
            return AddEnvFile(builder, provider: null, path: path, optional: optional, reloadOnChange: reloadOnChange);
        }

        /// <summary>
        /// Adds a ENV configuration source to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="provider">The <see cref="IFileProvider"/> to use to access the file.</param>
        /// <param name="path">Path relative to the base path stored in
        /// <see cref="IConfigurationBuilder.Properties"/> of <paramref name="builder"/>.</param>
        /// <param name="optional">Whether the file is optional.</param>
        /// <param name="reloadOnChange">Whether the configuration should be reloaded if the file changes.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddEnvFile(
            this IConfigurationBuilder builder,
            IFileProvider provider,
            string path,
            bool optional,
            bool reloadOnChange)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("File path must be a non-empty string.", nameof(path));
            }

            return builder.AddEnvFile(s =>
            {
                s.FileProvider = provider;
                s.Path = path;
                s.Optional = optional;
                s.ReloadOnChange = reloadOnChange;
            });
        }

        /// <summary>
        /// Adds a ENV configuration source to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="configureSource">Configures the source.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddEnvFile(
            this IConfigurationBuilder builder,
            Action<EnvConfigurationSource> configureSource)
            => builder.Add(configureSource);

        /// <summary>
        /// Adds a ENV configuration source to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="stream">The <see cref="Stream"/> to read the env configuration data from.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddEnvStream(this IConfigurationBuilder builder, Stream stream)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.Add<EnvStreamConfigurationSource>(s => s.Stream = stream);
        }
    }
}