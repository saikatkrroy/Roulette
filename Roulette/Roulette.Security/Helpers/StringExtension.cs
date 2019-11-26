// ReSharper disable once CheckNamespace - Extensions should be in the same namespace as what they are extending for ease of use.
namespace System
{
    public static class StringExtension
    {

        public static string Truncate(this string val, int truncateToSize)
        {
            if (string.IsNullOrEmpty(val))
            {
                return val;
            }
            if (val.Length > truncateToSize)
            {
                return val.Substring(0, truncateToSize);
            }
            return val;

        }

        public static bool IsValidGuid(this string str)
        {
            Guid temp;
            return Guid.TryParse(str, out temp);
        }

        public static bool EqualsText(this string one, string two)
        {
            return StringComparer.OrdinalIgnoreCase.Equals(one, two);
        }

        public static string Base64ToBase64URL(this string base64string)
        {
            return base64string
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", ".");
        }

        public static string Base64URLToBase64(this string base64URLstring)
        {
            return base64URLstring
                .Replace("-", "+")
                .Replace("_", "/")
                .Replace(".", "=");
        }
    }
}


