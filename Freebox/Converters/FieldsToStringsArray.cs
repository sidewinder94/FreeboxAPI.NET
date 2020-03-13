using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using System.Linq;

namespace Freebox.Converters
{
    public class FieldsToStringsArray : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType != null && 
                objectType.IsEnum && 
                objectType.IsDefined(typeof(FlagsAttribute), inherit: false);
        }


        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if(value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var definedFlags = Enum.GetValues(value.GetType()).Cast<Enum>().Where(((Enum)value).HasFlag).ToList();

            var fieldsList = value.GetType()
                                   .GetFields(BindingFlags.Public | BindingFlags.Static)
                                   .Where(v => definedFlags.Any(f => ((Enum)f).ToString() == v.Name))
                                   .Select(fi => fi.GetCustomAttribute<JsonPropertyAttribute>().PropertyName)
                                   .Distinct()
                                   .ToList();

            serializer.Serialize(writer, fieldsList);
        }
    }
}
