using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BigBytes.JsonParticle.Converter
{
    /// <summary>
    /// 
    /// </summary>
    public class TomlToJson : IConverter<String>
    {
        #region Constants

        /// <summary>
        /// 
        /// </summary>
        public const string TOML_EXAMPLE = @"
# This is a TOML document

title = ""TOML Example""

[owner]
name = ""Tom Preston-Werner""
dob = 1979-05-27T07:32:00-08:00

[database]
enabled = true
ports = [ 8000, 8001, 8002 ]
data = [ [""delta"", ""phi""], [3.14] ]
temp_targets = { cpu = 79.5, case = 72.0 }

[servers]

[servers.alpha]
ip = ""10.0.0.1""
role = ""frontend""

[servers.beta]
ip = ""10.0.0.2""
role = ""backend""
";

        /// <summary>
        /// 
        /// </summary>
        public const string JSON_EXAMPLE = @"
{
  ""_"": {
    ""title"": ""TOML Example""
  },
  ""owner"": {
    ""name"": ""Tom Preston-Werner"",
    ""dob"": ""1979-05-27T07:32:00-08:00""
  },
  ""database"": {
    ""enabled"": ""true"",
    ""ports"": ""[ 8000, 8001, 8002 ]"",
    ""data"": ""[ [\""delta\"", \""phi\""], [3.14] ]"",
    ""tempTargets"": ""{ cpu = 79.5, case = 72.0 }""
  },
  ""serversAlpha"": {
    ""ip"": ""10.0.0.1"",
    ""role"": ""frontend""
  },
  ""serversBeta"": {
    ""ip"": ""10.0.0.2"",
    ""role"": ""backend""
  }
}
";

        /// <summary>
        /// 
        /// </summary>
        public const string TOML_BASIC_PATTERN = @"
# Basic match regex for TOML

(?:^\s*)
(?:
(?<comment>\#[^\r\n]*)
|
(?:^\s*)(?<section>\[[^\]]*\])
|
(?:^\s*)(?<key>[^=]+)
(?:\s*=\s*)
    (?:
        (?<value>[^\r\n]*)
    )
)
";

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Convert(string input) => Convert(input, true);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="indent"></param>
        /// <returns></returns>
        public string Convert(string input, bool indent)
        {
            if (null == input)
            {
                return input;
            }

            var jo = new JObject();

            var currentSection = "";
            var sectionCreated = false;
            var sectionObject = default(JObject);
            var options = RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace;
            var match = Regex.Match(input, TOML_BASIC_PATTERN, options);
            while (match.Success)
            {
                try
                {
                    var section = match.Groups["section"].Value;
                    if (!string.IsNullOrEmpty(section))
                    {
                        currentSection = section;
                        sectionCreated = false;
                        continue;
                    }
                    var key = match.Groups["key"].Value;
                    if (string.IsNullOrEmpty(key))
                    {
                        continue;
                    }
                    var value = match.Groups["value"].Value;
                    if (!sectionCreated)
                    {
                        sectionObject = new JObject();
                        var sectionName = "_";
                        if (!string.IsNullOrEmpty(currentSection))
                        {
                            sectionName = Utility.TomlSectionNameToCSharpName(currentSection);
                            sectionName = Utility.CSharpNameToJsonName(sectionName);
                        }
                        jo.Add(sectionName, sectionObject);
                        sectionCreated = true;
                    }
                    var keyName = Utility.TomlSectionNameToCSharpName(key);
                    keyName = Utility.CSharpNameToJsonName(keyName);
                    if (value.Length >= 2 && value[0] == '"' && value[value.Length - 1] == '"')
                    {
                        value = value.Substring(1, value.Length - 2);
                        value = value.Replace("\\\"", "\"");
                    }
                    sectionObject.Add(keyName, value);
                }
                finally
                {
                    match = match.NextMatch();
                }
            }

            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy(),
            };
            var settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = contractResolver,
            };
            Formatting formatting = indent ? Formatting.Indented : Formatting.None;
            string json = JsonConvert.SerializeObject(jo, formatting, settings);
            return json;
        }
    }
}
