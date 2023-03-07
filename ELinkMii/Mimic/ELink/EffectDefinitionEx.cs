using ELinkMii.Mimic.ELink.Structs.User;

namespace ELinkMii.Mimic.ELink
{
    public class EffectDefinitionEx : ITableEx
    {
        private EffectDefinition Table;

        public uint field_0 { get; set; }
        public uint field_4 { get; set; }
        public string String { get; set; }
        public uint field_18 { get; set; }
        public uint field_1C { get; set; }
        public int field_20 { get; set; }
        public uint field_24 { get; set; }
        public uint field_28 { get; set; }
        public uint field_2C { get; set; }
        public uint field_30 { get; set; }
        public uint field_34 { get; set; }
        public uint field_38 { get; set; }
        public uint field_3C { get; set; }
        public uint field_40 { get; set; }
#if PLATFORM_SWITCH
        public uint field_44 { get; set; }
#endif

        public EffectDefinitionEx() { }

        public EffectDefinitionEx(EffectDefinition table, User.ReadContext ctx)
        {
            field_0 = table.field_0;
            field_4 = table.field_4;

            using (ctx.TemporarySeekToStringTable(table.StringOffset))
                String = ctx.Reader.ReadAsciiZ();

            field_18 = table.field_18;
            field_1C = table.field_1C;
            field_20 = table.field_20;
            field_24 = table.field_24;
            field_28 = table.field_28;
            field_2C = table.field_2C;
            field_30 = table.field_30;
            field_34 = table.field_34;
            field_38 = table.field_38;
            field_3C = table.field_3C;
            field_40 = table.field_40;
#if PLATFORM_SWITCH
            field_44 = table.field_44;
#endif

            Table = table;
        }

        public void Write()
        {
            Table.field_0 = field_0;
            Table.field_4 = field_4;
            Table.field_18 = field_18;
            Table.field_1C = field_1C;
            Table.field_20 = field_20;
            Table.field_24 = field_24;
            Table.field_28 = field_28;
            Table.field_2C = field_2C;
            Table.field_30 = field_30;
            Table.field_34 = field_34;
            Table.field_38 = field_38;
            Table.field_3C = field_3C;
            Table.field_40 = field_40;
#if PLATFORM_SWITCH
            Table.field_44 = field_44;
#endif
        }

        public void AddToStringTable(StringTable stringTable)
        {
            stringTable.TryAdd(String);
        }

        public void WriteTo(Stream stream)
        {
            var s = Utils.AsSpan(ref Table);
            stream.Write(s);
        }

        public void WriteStrings(StringTable.Binary strings)
        {
            Table.StringOffset = strings.FindIndex(String);
        }
    }
}
