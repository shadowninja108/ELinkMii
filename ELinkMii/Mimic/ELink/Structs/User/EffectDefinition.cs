using System.Runtime.InteropServices;

namespace ELinkMii.Mimic.ELink.Structs.User
{
#if PLATFORM_SWITCH
    [StructLayout(LayoutKind.Sequential, Size = 0x48, Pack = 1)]
#elif PLATFORM_3DS
    [StructLayout(LayoutKind.Sequential, Size = 0x3C, Pack = 1)]
#else
#error "Invalid platform!"
#endif
    public unsafe struct EffectDefinition
    {
        public uint field_0;
        public uint field_4;
        public uint StringOffset;
#if PLATFORM_SWITCH
        public ulong String;
        public fixed byte field_14[4]; /* Padding...? */
#elif PLATFORM_3DS
        public uint String;
#else
#error "Invalid platform!"
#endif
        public uint field_18;
        public uint field_1C;
        public int field_20;
        public uint field_24;
        public uint field_28;
        public uint field_2C;
        public uint field_30;
        public uint field_34;
        public uint field_38;
        public uint field_3C;
        public uint field_40;
#if PLATFORM_SWITCH
        public uint field_44;
#endif
    }
}
