namespace ELinkMii.Mimic.ELink
{
    public interface ITableEx
    {
        void Write();
        void WriteTo(Stream stream);
        void AddToStringTable(StringTable stringTable);
        void WriteStrings(StringTable.Binary strings);
    }
}
