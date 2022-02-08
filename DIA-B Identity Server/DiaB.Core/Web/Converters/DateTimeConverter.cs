using Newtonsoft.Json.Converters;

namespace DiaB.Core.Web.Converters
{
    public class DateTimeConverter : IsoDateTimeConverter
    {
        public DateTimeConverter(string dateTimeFormat)
        {
            DateTimeFormat = dateTimeFormat;
        }
    }
}
