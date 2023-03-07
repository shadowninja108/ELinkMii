using System.Runtime.InteropServices;

namespace ELinkMii.Mimic.ELink.Structs.User
{
#if PLATFORM_SWITCH
    [StructLayout(LayoutKind.Sequential, Size = 0x70, Pack = 1)]
#elif PLATFORM_3DS
    [StructLayout(LayoutKind.Sequential, Size = 0x68, Pack = 1)]
#else
#error "Invalid platform!"
#endif
    public struct ParticleDefinition
    {
        public uint Index;
#if PLATFORM_SWITCH
        public uint field_4;
        public ulong EffectCallPtr;
#elif PLATFORM_3DS
        public uint EffectCallPtr;
#else
#error "Invalid platform!"
#endif
        public uint StringOffset1;
        public uint StringLookup1;
        public uint GroupOffset;
        public uint StringLookup2;
        public uint StringOffset3;
        public uint StringLookup3;
        public uint PtclNameOffset;
        public ushort field_2C;
        public ushort field_2E;
        public uint BoneOffset;
        public uint StringOffset5;
        public ushort field_38;
        public byte field_3A;
        public byte String5Lookup;
        public byte field_3C;
        public byte field_3D;
        public byte field_3E;
        public byte field_3F;
        public uint field_40;
        public float Scale;
        public float X;
        public float Y;
        public float Z;
        public float field_54;
        public float field_58;
        public float field_5C;
        public float field_60;
        public float field_64;
        public float field_68;
        public float field_6C;
    }
}
