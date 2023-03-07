using System.Runtime.InteropServices;

namespace ELinkMii.Mimic.ELink.Structs.User
{
#if PLATFORM_SWITCH
    [StructLayout(LayoutKind.Sequential, Size = 0x18)]
#elif PLATFORM_3DS
    [StructLayout(LayoutKind.Sequential, Size = 0xC)]
#else
#error "Invalid platform!"
#endif
    public unsafe struct EffectCall
    {
        public uint LabelOffset;
#if PLATFORM_SWITCH
        public fixed byte field_4[4]; /* Padding? */
        public ulong String;
#elif PLATFORM_3DS
        public uint String;
#else
#error "Invalid platform!"
#endif
        public ushort DefinitionIdxStart;
        public ushort DefinitionIdxEnd;

#if PLATFORM_SWITCH
        /* Padding? */
        public fixed byte field_14[4];
#endif
    }
}
