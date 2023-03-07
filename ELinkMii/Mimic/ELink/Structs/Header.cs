using System.Runtime.InteropServices;

namespace ELinkMii.Mimic.ELink
{
    [StructLayout(LayoutKind.Sequential, Size = 0x10)]
    public struct Header
    {
        public static uint ExpectedMagic = 0x6B6C6665;

        public uint Magic;
        public uint Version;
        public int Count;
        public uint StringTableOffset;
    }
}
