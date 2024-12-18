namespace TechnicalAxos_OscarBarrera.Helpers.Json
{

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    
    public class LanguagesToStringConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);

            // Create a list to hold the language names
            List<string> languages = new List<string>();

            // Loop through the JSON properties and add the language names to the list
            foreach (var property in jsonObject.Properties())
            {
                languages.Add(property.Value.ToString());
            }

            // Join the list into a single string with a separator (e.g., comma)
            return string.Join(", ", languages);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Optional: Implement this if you need to serialize this string back to JSON
            writer.WriteValue(value);
        }
    }

}

