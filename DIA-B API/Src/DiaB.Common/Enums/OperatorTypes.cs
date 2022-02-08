using System.ComponentModel;

namespace DiaB.Common.Enums
{
    [Description("các loại phép tính")]
    public enum OperatorTypes
    {
        [Description("<")]
        LessThan = 1,
        [Description(">")]
        GreaterThan = 2,
        [Description("<=")]
        LessThanOrEqual = 3,
        [Description(">=")]
        GreaterThanOrEqual = 4,
    }
}
