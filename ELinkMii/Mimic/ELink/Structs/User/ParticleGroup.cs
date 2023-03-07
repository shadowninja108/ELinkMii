using System.Runtime.InteropServices;

namespace ELinkMii.Mimic.ELink.Structs.User
{
#if PLATFORM_SWITCH
    [StructLayout(LayoutKind.Sequential, Size = 0x20)]
#elif PLATFORM_3DS
    [StructLayout(LayoutKind.Sequential, Size = 0x14)]
#else
#error "Invalid platform!"
#endif
    public unsafe struct ParticleGroup
    {
        public uint LabelOffset;
#if PLATFORM_SWITCH
        public uint field_4; /* Padding? */
        public ulong String;
#elif PLATFORM_3DS
        public uint String;
#else
#error "Invalid platform!"
#endif
        public uint Hash; 
        public fixed byte field_18[4]; /* Padding? */
        public ushort DefinitionIdxStart;
        public ushort DefinitionIdxEnd;
#if PLATFORM_SWITCH
        public fixed byte field_1C[4];  /* Padding? */
#endif
    }
}
