using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace PortfolioPerformance.Service.DivvyDiary
{
    // DateOnly is not yet supported
    // https://github.com/JamesNK/Newtonsoft.Json/issues/2521
    // Not sure why this didn't work
    //public class DateOnlyJsonConverter : JsonConverter<DateOnly>
    //{
    //    private const string DateFormat = "yyyy-MM-dd";

    //    public override DateOnly ReadJson(JsonReader reader, Type objectType, DateOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
    //    {
    //        return DateOnly.ParseExact((string)reader.Value, DateFormat, CultureInfo.InvariantCulture);
    //    }

    //    public override void WriteJson(JsonWriter writer, DateOnly value, JsonSerializer serializer)
    //    {
    //        writer.WriteValue(value.ToString(DateFormat, CultureInfo.InvariantCulture));
    //    }
    //}

    public class DateOnlyFormatConverter : IsoDateTimeConverter
    {
        public DateOnlyFormatConverter(string format)
        {
            DateTimeFormat = format;
        }
    }
}
