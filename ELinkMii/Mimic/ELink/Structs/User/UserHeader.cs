using System.Runtime.InteropServices;

namespace ELinkMii.Mimic.ELink.Structs.User
{
    [StructLayout(LayoutKind.Sequential, Size = 0x1C)]
    public struct UserHeader
    {
        public uint Version;
        public uint ParticleDefinitionCount;
        public uint EffectDefinitionCount;
        public uint ParticleGroupCount;
        public uint EffectCallCount;
        public uint StringTableOffset;
        public int field_18;
    }
}
