using ByteSizeLib;

namespace HeySiri.Core.Extentions;

public static class ByteSizeExtention
{
    public static double ByteToMebi(this long size)
    {
        return ByteSize.FromBytes(size).MebiBytes;
    }

    public static double KibiToMebi(this int size)
    {
        return ByteSize.FromKibiBytes(size).MebiBytes;
    }

    public static double ByteToGibi(this long size)
    {
        return ByteSize.FromBytes(size).GibiBytes;
    }
}