namespace LocatePlate.Infrastructure.Extentions
{
    public static class StringHelper
    {
        public static string Truncate(this string s, int length)
        {
            if (!string.IsNullOrEmpty(s) && s.Length > length) return s.Substring(0, length);
            return s;
        }
        public static string AppendImagePath(this string s, int length)
        {
            if (!string.IsNullOrEmpty(s) && s.Length > length) return s.Substring(0, length);
            return s;
        }

        public static string ReplaceSpace(this string s, char replacedChar)
        {
            if (!string.IsNullOrEmpty(s) && s.Contains(' ')) return s.Replace(' ', replacedChar);
            return s;
        }
    }
}
