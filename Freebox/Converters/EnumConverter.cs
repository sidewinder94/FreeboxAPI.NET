using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Freebox.Converters
{
    public class EnumConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return false;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Ne pas passer de littéraux en paramètres localisés", Justification = "This exception is targeted towards developers")]
        public override object ReadJson(JsonReader reader, Type objectType,
            object existingValue, JsonSerializer serializer)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if(objectType == null)
            {
                throw new ArgumentNullException(nameof(objectType));
            }

            var value = (string)reader.Value;

            bool isEnumerable = objectType.IsEnum
                || (objectType.IsGenericType
                    && objectType.GetGenericTypeDefinition() == typeof(Nullable<>)
                    && objectType.GetGenericArguments()[0].IsEnum);

            if (!isEnumerable)
            {
                throw new InvalidOperationException("Only works on enums");
            }

            bool isNullable = !objectType.IsEnum;

            Type enumType = objectType;

            if (isNullable)
            {
                enumType = objectType.GetGenericArguments()[0];
            }

            var enumNames = Enum.GetNames(enumType);

            foreach (var enumName in enumNames)
            {
                var lEnumName = enumName.ToUpperInvariant();
                var lValue = value.ToUpperInvariant().Replace("_", "").Trim();

                if (lEnumName == lValue)
                {
                    return Enum.Parse(enumType, enumName);
                }
            }

            if (isNullable)
            {
                return null;
            }

            throw new KeyNotFoundException($"{value} couldn't be found in {objectType.Name}");
        }

        public override void WriteJson(JsonWriter writer, object value,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
