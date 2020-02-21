using Freebox.Data;
using Makaretu.Dns;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Freebox.Parser
{
    /// <summary>
    /// Utility class to use Dns records more easily
    /// </summary>
    static class TXTRecordsStringsParserExtension
    {
        /// <summary>
        /// Method to convert a <see cref="TXTRecord"/> into a <see cref="Dictionary{TKey, TValue}">Dictionary{string, string}</see>"/>
        /// The record strings need to all contain key value pairs separated by the character "="
        /// </summary>
        /// <param name="record">The record to parse</param>
        /// <returns>A dictionary containg a key value pair for each string in the record</returns>
        internal static Dictionary<string, string> StringsDictionary(this TXTRecord record)
        {
            return record.Strings.Select(kv =>
            {
                var splitted = kv.Split('=');
                return new KeyValuePair<string, string>(splitted[0], splitted[1]);
            }).ToDictionary(k => k.Key, v => v.Value);
        }

        /// <summary>
        /// Parses a <see cref="TXTRecord"/> into a <see cref="Freebox.Data.ApiInfo"/> object
        /// </summary>
        /// <param name="record">The record to try and parse</param>
        /// <returns></returns>
        internal static ApiInfo ApiInfo(this TXTRecord record)
        {
            var result = new ApiInfo();


            if (TryParse(record, out Dictionary<string, string> strings))
            {
                var setters = typeof(ApiInfo).GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
                               .SelectMany(p =>
                                p.GetCustomAttributes(typeof(JsonPropertyAttribute))
                                    .ToDictionary(
                                        k => ((JsonPropertyAttribute)k).PropertyName,
                                        v => p.SetMethod))
                                .ToDictionary(k => k.Key, v => v.Value);

                foreach (var kvp in strings)
                {
                    if (setters.ContainsKey(kvp.Key))
                    {
                        setters[kvp.Key].Invoke(result, new[] { kvp.Value });
                    }
                }

                return result;
            }

            return null;
        }

        /// <summary>
        /// Method to try and parse a <see cref="TXTRecord"/> into a <see cref="Dictionary{TKey, TValue}"/> while suppressing any exception
        /// </summary>
        /// <param name="record">The record to parse</param>
        /// <param name="result">The parsed value in case of success null if the method fails</param>
        /// <returns>true in case of success, false otherwise</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Ne pas intercepter les types d'exception générale", Justification = "Implementing the Try-Parse pattern here, we do want to suppress any exceptions")]
        internal static bool TryParse(TXTRecord record, out Dictionary<string, string> result)
        {
            result = null;

            try
            {
                result = StringsDictionary(record);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
