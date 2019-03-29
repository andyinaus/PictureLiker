using System;

namespace PictureLiker.Extensions
{
    public static class StringExtensions
    {
        public static bool EqualsIgnoreCase(this string value, string valueToCompare)
        {
            return value.Equals(valueToCompare, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
