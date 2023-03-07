using System.Runtime.CompilerServices;
using System.Text;
using ELinkMii.Mimic.ELink.Structs.User;

namespace ELinkMii.Mimic.ELink
{
    public class User
    {
        public string Name { get; set; }
        public UserHeaderEx Header { get; set; }

        public ParticleDefinitionEx[]   ParticleDefinitions { get; set; }
        public int[]                    Table2 { get; set; }
        public ParticleGroupEx[]        ParticleGroups { get; set; }
        public EffectDefinitionEx[]     EffectDefinitions { get; set; }
        public EffectCalls[]            EffectCalls { get; set; }

        public class ReadContext
        {
            public Stream Stream;
            public BinaryReader Reader;
            public long Start;
            public UserHeaderEx Header;

            public TemporarySeekHandle TemporarySeekToStringTable(uint pos)
            {
                return Stream.TemporarySeek(Start + Header.Header.StringTableOffset + pos, SeekOrigin.Begin);
            }
        }
        
        public User() { }

        public User(string name, Stream stream)
        {
            Name = name;
            var start = stream.Position;

            var header = new UserHeader();
            stream.Read(Utils.AsSpan(ref header));
            Header = new UserHeaderEx(header);

            var particles = stream.ReadArray<ParticleDefinition>(Header.Header.ParticleDefinitionCount);
            Table2 = stream.ReadArray<int>((uint)(Header.Header.field_18 * Header.Header.ParticleDefinitionCount));
            var particleGroups = stream.ReadArray<ParticleGroup>(Header.Header.ParticleGroupCount);
            var effectDefinitions = stream.ReadArray<EffectDefinition>(Header.Header.EffectDefinitionCount);
            var effectCalls = stream.ReadArray<EffectCall>(Header.Header.EffectCallCount);

            var ctx = new ReadContext()
            {
                Stream = stream,
                Reader = new BinaryReader(stream, Encoding.ASCII),
                Start = start,
                Header = Header
            };

            ParticleDefinitions = particles.Select(x => new ParticleDefinitionEx(x, ctx)).ToArray();
            ParticleGroups = particleGroups.Select(x => new ParticleGroupEx(x, ctx)).ToArray();
            EffectDefinitions = effectDefinitions.Select(x => new EffectDefinitionEx(x, ctx)).ToArray();
            EffectCalls = effectCalls.Select(x => new EffectCalls(x, ctx)).ToArray();
        }

        public void WriteTo(Stream stream)
        {
            var start = stream.Position;

            stream.Position += Unsafe.SizeOf<UserHeader>();

            foreach (var table in ParticleDefinitions) { table.Write(); }
            foreach (var table in ParticleGroups) { table.Write(); }
            foreach (var table in EffectDefinitions) { table.Write(); }
            foreach (var table in EffectCalls) { table.Write(); }

            var stringTable = new StringTable();

            foreach (var table in ParticleDefinitions) { table.AddToStringTable(stringTable); }
            foreach (var table in ParticleGroups) { table.AddToStringTable(stringTable); }
            foreach (var table in EffectDefinitions) { table.AddToStringTable(stringTable); }
            foreach (var table in EffectCalls) { table.AddToStringTable(stringTable); }

            var stringData = stringTable.Build();

            foreach (var table in ParticleDefinitions) { table.WriteStrings(stringData); }
            foreach (var table in ParticleGroups) { table.WriteStrings(stringData); }
            foreach (var table in EffectDefinitions) { table.WriteStrings(stringData); }
            foreach (var table in EffectCalls) { table.WriteStrings(stringData); }

            foreach (var table in ParticleDefinitions) { table.WriteTo(stream); }
            stream.WriteArray(Table2);
            foreach (var table in ParticleGroups) { table.WriteTo(stream); }
            foreach (var table in EffectDefinitions) { table.WriteTo(stream); }
            foreach (var table in EffectCalls) { table.WriteTo(stream); }

            var stringTablePos = stream.Position;
            using (stream.TemporarySeek(start, SeekOrigin.Begin))
            {
                Header.Write(this);
                Header.Header.StringTableOffset = (uint)(stringTablePos - start);
                Header.WriteTo(stream);
            }

            stream.WriteArray(stringData.Data);

            /* Pad between users. */
            while ((stream.Position % 0x10) != 0)
                stream.WriteByte(0xff);
        }
    }
}
