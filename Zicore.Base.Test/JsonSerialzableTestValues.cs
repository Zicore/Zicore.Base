using Zicore.Base.Json;

namespace Zicore.Base.Test
{
    /// <summary>
    /// Some arbitrary values.
    /// </summary>
    public class JsonSerialzableTestValues : JsonSerializable
    {
        public string StringValue { get; set; } = "String Value";
        public int IntegerValue { get; set; } = 31;
        public double DoubleValue { get; set; } = 519248.515124;
        public decimal DecimalValue { get; set; } = 1035912.54124m;
    }
}