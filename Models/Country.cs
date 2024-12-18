namespace TechnicalAxos_OscarBarrera.Models
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    using TechnicalAxos_OscarBarrera.Helpers.Json;

    public partial class CountryInfo
    {
        [JsonProperty("flags")]
        public Flags Flags { get; set; }

        [JsonProperty("name")]
        public Name Name { get; set; }

        [JsonProperty("capital")]
        public string[] Capital { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("subregion")]
        public string Subregion { get; set; }

        [JsonProperty("languages")]
        [JsonConverter(typeof(LanguagesToStringConverter))]
        public string Languages { get; set; }

        [JsonProperty("population")]
        public long Population { get; set; }
    }

    public partial class Flags
    {
        [JsonProperty("png")]
        public Uri Png { get; set; }

        [JsonProperty("svg")]
        public Uri Svg { get; set; }

        [JsonProperty("alt")]
        public string Alt { get; set; }
    }

    public partial class Name
    {
        [JsonProperty("common")]
        public string Common { get; set; }

        [JsonProperty("official")]
        public string Official { get; set; }

        [JsonProperty("nativeName")]
        public Dictionary<string, NativeName> NativeName { get; set; }
    }

    public partial class NativeName
    {
        [JsonProperty("official")]
        public string Official { get; set; }

        [JsonProperty("common")]
        public string Common { get; set; }
    }
}
