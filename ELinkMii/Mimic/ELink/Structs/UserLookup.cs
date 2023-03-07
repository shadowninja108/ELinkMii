using System.Runtime.InteropServices;

namespace ELinkMii.Mimic.ELink.Structs
{
    [StructLayout(LayoutKind.Sequential, Size = 0x8)]
    public struct UserLookup
    {
        public uint Offset;
        public uint NameOffset;
    }
}
