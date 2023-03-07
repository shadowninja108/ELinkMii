using ELinkMii.Mimic.ELink.Structs.User;

namespace ELinkMii.Mimic.ELink
{
    public class UserHeaderEx
    {
        public UserHeader Header;

        public uint Version { get; set; }

        public UserHeaderEx() {}

        public UserHeaderEx(UserHeader header)
        {
            Version = header.Version;

            Header = header;
        }
        public void Write(User user)
        {
            Header.Version = Version;

            Header.ParticleDefinitionCount = (uint)user.ParticleDefinitions.Length;
            Header.ParticleGroupCount = (uint)user.ParticleGroups.Length;
            Header.EffectDefinitionCount = (uint)user.EffectDefinitions.Length;
            Header.EffectCallCount = (uint)user.EffectCalls.Length;

            var field_18 = (float)user.Table2.Length / Header.ParticleDefinitionCount;

            /* Catch divide by zero. */
            if (float.IsNaN(field_18))
                field_18 = 0;

            if (field_18 % 1 != 0)
            {
                throw new Exception("Table 1 count must be a multiple of Table 2 count!");
            }

            Header.field_18 = (int)field_18;
        }

        public void WriteTo(Stream stream)
        {
            var s = Utils.AsSpan(ref Header);
            stream.Write(s);
        }
        
    }
}
