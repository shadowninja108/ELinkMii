using System.Text;

namespace ELinkMii
{
    public class StringTable
    {
        public struct Binary
        {
            public byte[] Data;

            public string[] Strings;
            public uint[] Indices;

            public uint FindIndex(string str)
            {
                return Indices[Array.IndexOf(Strings, str)];
            }
        }

        public readonly List<string> StringList = new();

        public int IndexOf(string str)
        {
            return StringList.IndexOf(str);
        }

        public bool IsEmpty() => StringList.Count == 0;

        public void TryAdd(string str)
        {
            var idx = Utils.BinarySearch(StringList, str);

            if (str == string.Empty && StringList.Contains(string.Empty))
                return;

            /* Don't add it if we already have it. */
            if (idx >= 0)
                return;

            StringList.Add(str);
        }

        public Binary Build()
        {
            var ret = new Binary()
            {
                Strings = new string[StringList.Count],
                Indices = new uint[StringList.Count]
            };

            var binaryStream = new MemoryStream(StringList.Sum(x => x.Length + 1));
            var binaryWriter = new BinaryWriter(binaryStream, Encoding.GetEncoding("Shift-JIS"));

            for (var i = 0; i < StringList.Count; i++)
            {
                var s = StringList[i];

                ret.Strings[i] = s;
                ret.Indices[i] = (uint)binaryStream.Position;

                binaryWriter.Write(s.AsSpan());
                binaryWriter.Write('\0');
            }

            ret.Data = binaryStream.ToArray();

            return ret;
        }
    }
}
