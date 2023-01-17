using System.Globalization;

namespace AyBorg.Diagrams.Core.Extensions
{
    public static class NumberExtensions
    {
        public static string ToInvariantString(this double n) => n.ToString(CultureInfo.InvariantCulture);
    }
}
