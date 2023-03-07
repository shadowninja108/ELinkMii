using ELinkMii.Mimic.ELink.Structs.User;

namespace ELinkMii.Mimic.ELink
{
    public class ParticleDefinitionEx : ITableEx
    {
        private ParticleDefinition Table;

        public uint Index { get; set; }

#if PLATFORM_SWITCH
        public uint field_4 { get; set; }
#endif
        public string String1 { get; set; }
        public string Group { get; set; }
        public string String3 { get; set; }
        public string PtclName { get; set; }
        public ushort field_2C { get; set; }
        public ushort field_2E { get; set; }
        public string Bone { get; set; }
        public string String5 { get; set; }
        public ushort field_38 { get; set; }
        public byte field_3A { get; set; }
        public byte field_3C { get; set; }
        public byte field_3D { get; set; }
        public byte field_3E { get; set; }
        public byte field_3F { get; set; }
        public uint field_40 { get; set; }
        public float Scale { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float field_54 { get; set; }
        public float field_58 { get; set; }
        public float field_5C { get; set; }
        public float field_60 { get; set; }
        public float field_64 { get; set; }
        public float field_68 { get; set; }
        public float field_6C { get; set; }

        public ParticleDefinitionEx() { }

        public ParticleDefinitionEx(ParticleDefinition table, User.ReadContext ctx)
        {
            Index = table.Index;
#if PLATFORM_SWITCH
            field_4 = table.field_4;
#endif

            using (ctx.TemporarySeekToStringTable(table.StringOffset1))
                String1 = ctx.Reader.ReadAsciiZ();
            using (ctx.TemporarySeekToStringTable(table.GroupOffset))
                Group = ctx.Reader.ReadAsciiZ();
            using (ctx.TemporarySeekToStringTable(table.StringOffset3))
                String3 = ctx.Reader.ReadAsciiZ();
            using (ctx.TemporarySeekToStringTable(table.PtclNameOffset))
                PtclName = ctx.Reader.ReadAsciiZ();

            field_2C = table.field_2C;
            field_2E = table.field_2E;

            using (ctx.TemporarySeekToStringTable(table.BoneOffset))
                Bone = ctx.Reader.ReadAsciiZ();
            using (ctx.TemporarySeekToStringTable(table.StringOffset5))
                String5 = ctx.Reader.ReadAsciiZ();

            field_38 = table.field_38;
            field_3A = table.field_3A;
            field_3C = table.field_3C;
            field_3D = table.field_3D;
            field_3E = table.field_3E;
            field_3F = table.field_3F;
            field_40 = table.field_40;
            Scale = table.Scale;
            X = table.X;
            Y = table.Y;
            Z = table.Z;
            field_54 = table.field_54;
            field_58 = table.field_58;
            field_5C = table.field_5C;
            field_60 = table.field_60;
            field_64 = table.field_64;
            field_68 = table.field_68;
            field_6C = table.field_6C;

            Table = table;
        }

        public void Write()
        {
            Table.Index  = Index;
#if PLATFORM_SWITCH
            Table.field_4 = field_4;
#endif
            Table.field_2C = field_2C;
            Table.field_2E = field_2E;
            Table.field_38 = field_38;
            Table.field_3A = field_3A;
            Table.field_3C = field_3C;
            Table.field_3D = field_3D;
            Table.field_3E = field_3E;
            Table.field_3F = field_3F;
            Table.field_40 = field_40;
            Table.Scale = Scale;
            Table.X = X;
            Table.Y = Y;
            Table.Z = Z;
            Table.field_54 = field_54;
            Table.field_58 = field_58;
            Table.field_5C = field_5C;
            Table.field_60 = field_60;
            Table.field_64 = field_64;
            Table.field_68 = field_68;
            Table.field_6C = field_6C;
        }

        public void WriteTo(Stream stream)
        {
             stream.Write(Utils.AsSpan(ref Table));
        }

        public void AddToStringTable(StringTable stringTable)
        {
            stringTable.TryAdd(String1);
            stringTable.TryAdd(Group);
            stringTable.TryAdd(String3);
            stringTable.TryAdd(PtclName);
            stringTable.TryAdd(Bone);
            stringTable.TryAdd(String5);
        }

        public void WriteStrings(StringTable.Binary strings)
        {
            Table.StringOffset1     = strings.FindIndex(String1);
            Table.GroupOffset       = strings.FindIndex(Group);
            Table.StringOffset3     = strings.FindIndex(String3);
            Table.PtclNameOffset    = strings.FindIndex(PtclName);
            Table.BoneOffset        = strings.FindIndex(Bone);
            Table.StringOffset5     = strings.FindIndex(String5);
        }
    }
}
