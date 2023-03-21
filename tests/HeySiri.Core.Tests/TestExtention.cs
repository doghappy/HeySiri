namespace HeySiri.Core.Tests;

public static class TestExtention
{
    public static bool EqualsIgnoreCarriageReturn(this string s, string d)
    {
        return string.Equals(s.Replace("\r", string.Empty), d.Replace("\r", string.Empty));
    }
}