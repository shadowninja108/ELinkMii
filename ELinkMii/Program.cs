using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using ELinkMii.Mimic.ELink;
using ELinkMii.Mimic.ELink.Structs;

namespace ELinkMii
{
    internal class Program
    {
        private const string ELinkUserExtension = "elu";
        private const string ExtractSuffix = " (extracted)";

        private const string ExtractAction = "extract";
        private const string PackageAction = "package";

        static void Main(string[] args)
        {
            /* For supporting Shift-JIS. */
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (args.Length != 2)
            {
                Console.WriteLine($"Args are [{ExtractAction}/{PackageAction}] [path]");
                return;
            }

            var action = args[0];
            var path = args[1];
            switch (action)
            {
                case ExtractAction:
                    DoExtract(path);
                    return;
                case PackageAction:
                    DoPackage(path);
                    return;
                default:
                    Console.WriteLine($"Invalid action. Valid options are {ExtractAction}/{PackageAction}.");
                    return;
            }
        }

        static void DoExtract(string path)
        {
            var fi = new FileInfo(path);

            if (!fi.Exists)
            {
                Console.WriteLine($"\"{fi.FullName}\" is not a valid file path.");
                return;
            }

            var od = fi.Directory.GetDirectory($"{fi.Name}{ExtractSuffix}");
            od.Create();

            using var input = fi.OpenRead();
            var inputReader = new BinaryReader(input, Encoding.ASCII);

            var header = new Header();
            input.Read(Utils.AsSpan(ref header));

            if (header.Magic != Header.ExpectedMagic)
            {
                Console.WriteLine("File appears to be invalid. Only Switch files are supported at the moment...");
                return;
            }

            var lookups = input.ReadArray<UserLookup>((uint)header.Count);

            for (var i = 0; i < lookups.Length; i++)
            {
                var lookup = lookups[i];

                string name;
                using (input.TemporarySeek(header.StringTableOffset + lookup.NameOffset, SeekOrigin.Begin))
                {
                    name = inputReader.ReadShiftJISZ();
                }

                /* Figure out safe file name to use if needed. */
                var safeName = Regex.Replace(name, "[" + new string(Path.GetInvalidFileNameChars()) + "]+", "_", RegexOptions.Compiled);
                if (safeName != name)
                {
                    safeName += $"-{lookup.Offset:x8}";
                }

                var outputRawFi = od.GetFile($"{safeName}.{ELinkUserExtension}");
                var outputJsonFi = od.GetFile($"{safeName}.json");
                using (input.TemporarySeek(lookup.Offset, SeekOrigin.Begin))
                {
                    var start = input.Position;
                    var user = new User(name, input);

                    /* Infer the user length by either the next user or the start of the string table. */
                    long len;
                    if (i + 1 < lookups.Length)
                    {
                        len = lookups[i + 1].Offset - lookups[i].Offset;
                    }
                    else
                    {
                        len = header.StringTableOffset - lookups[i].Offset;
                    }

                    /* Write out raw file. */
                    using (input.TemporarySeek(start, SeekOrigin.Begin))
                    {
                        var raw = new byte[len];
                        input.Read(raw);
                        using var os = outputRawFi.Create();
                        os.Write(raw);
                    }

                    /* Write out json. */
                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        /* Make the japanese characters easier to read. */
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                    };
                    var json = JsonSerializer.Serialize(user, options);
                    File.WriteAllText(outputJsonFi.FullName, json);
                }
            }
        }

        private static void DoPackage(string path)
        {
            var di = new DirectoryInfo(path);

            if (!di.Exists)
            {
                Console.WriteLine($"\"{di.FullName}\" is not a valid directory path.");
                return;
            }

            var fi = di.Parent.GetFile($"{di.Name}.elk");

            var users = di.GetFiles()
                .Where(x => x.Extension == ".json")
                .Select(x => JsonSerializer.Deserialize<User>(File.ReadAllText(x.FullName))!)
                .ToList();

            var userNames = users.Select(x => x.Name).ToList();

            using var os = fi.Create();

            Header header = new()
            {
                Magic = Header.ExpectedMagic,
                Version = 9, /* ? */
                Count = users.Count
            };

            /* Skip over the header/lookups for now. */
            os.Position = Unsafe.SizeOf<Header>() + (Unsafe.SizeOf<UserLookup>() * users.Count);

            /* Write users, noting their indices. */
            var userIndices = new uint[users.Count];
            for (var i = 0; i < users.Count; i++)
            {
                var user = users[i];
                /* Note index. */
                userIndices[i] = (uint)os.Position;
                /* Write user data. */
                user.WriteTo(os);
            }

            /* Build user name string table. */
            var userNameStringTable = new StringTable();
            userNames.ForEach(userNameStringTable.TryAdd);
            var userNameStringTableBin = userNameStringTable.Build();

            /* Write user name string table. */
            header.StringTableOffset = (uint)os.Position;
            os.Write(userNameStringTableBin.Data);

            /* Back to the beginning. */
            os.Position = 0;

            /* Write the header. */
            os.Write(Utils.AsSpan(ref header));

            /* Write lookups. */
            for (var i = 0; i < users.Count; i++)
            {
                var lookup = new UserLookup()
                {
                    Offset = userIndices[i],
                    NameOffset = userNameStringTableBin.FindIndex(userNames[i])
                };

                os.Write(Utils.AsSpan(ref lookup));
            }
        }
    }
}