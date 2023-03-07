using ELinkMii.Mimic.ELink.Structs.User;

namespace ELinkMii.Mimic.ELink
{
    public class EffectCalls : ITableEx
    {
        private EffectCall Table;

        public string Label { get; set; }
        public ushort DefinitionIdxStart { get; set; }
        public ushort DefinitionIdxEnd { get; set; }

        public EffectCalls() { }

        public EffectCalls(EffectCall table, User.ReadContext ctx)
        {
            using (ctx.TemporarySeekToStringTable(table.LabelOffset))
                Label = ctx.Reader.ReadAsciiZ();

            DefinitionIdxStart = table.DefinitionIdxStart;
            DefinitionIdxEnd = table.DefinitionIdxEnd;

            Table = table;
        }

        public void Write()
        {
            Table.DefinitionIdxStart = DefinitionIdxStart;
            Table.DefinitionIdxEnd = DefinitionIdxEnd;
        }
        public void WriteTo(Stream stream)
        {
            stream.Write(Utils.AsSpan(ref Table));
        }

        public void AddToStringTable(StringTable stringTable)
        {
            stringTable.TryAdd(Label);
        }

        public void WriteStrings(StringTable.Binary strings)
        {
            Table.LabelOffset = strings.FindIndex(Label);
        }
    }
}
