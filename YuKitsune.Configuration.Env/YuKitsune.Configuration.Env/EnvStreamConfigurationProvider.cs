using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace YuKitsune.Configuration.Env
{
    /// <summary>
    /// An ENV file based <see cref="StreamConfigurationProvider"/>.
    /// </summary>
    public class EnvStreamConfigurationProvider : StreamConfigurationProvider
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="source">The <see cref="EnvStreamConfigurationSource"/>.</param>
        public EnvStreamConfigurationProvider(EnvStreamConfigurationSource source) : base(source) { }

        /// <summary>
        /// Read a stream of ENV values into a key/value dictionary.
        /// </summary>
        /// <param name="stream">The stream of ENV data.</param>
        /// <returns>The <see cref="IDictionary{String, String}"/> which was read from the stream.</returns>
        public static IDictionary<string, string> Read(Stream stream)
        {
            var data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            using (var reader = new StreamReader(stream))
            {
                while (reader.Peek() != -1)
                {
                    string rawLine = reader.ReadLine();
                    string line = rawLine.Trim();

                    // Ignore blank lines
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }
                    // Ignore comments
                    if (line[0] == '#')
                    {
                        continue;
                    }

                    // key = value OR "value"
                    int separator = line.IndexOf('=');
                    if (separator < 0)
                    {
                        throw new FormatException($"Unrecognized line format: '{rawLine}'.");
                    }

                    string key = NormalizeKey(line.Substring(0, separator).Trim());
                    string value = line.Substring(separator + 1).Trim();

                    if (data.ContainsKey(key))
                    {
                        throw new FormatException($"A duplicate key '{key}' was found.");
                    }

                    data[key] = value;
                }
            }
            return data;
        }

        /// <summary>
        /// Loads ENV configuration key/values from a stream into a provider.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to load env configuration data from.</param>
        public override void Load(Stream stream)
        {
            Data = Read(stream);
        }

        private static string NormalizeKey(string key) => key.Replace("__", ConfigurationPath.KeyDelimiter);
    }
}