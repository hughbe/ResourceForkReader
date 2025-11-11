using System.Diagnostics;
using System.Text;
using ResourceForkReader.Records;

namespace ResourceForkReader.Tests;

public class ResourceForkTests
{
    [Fact]
    public void Ctor_Stream()
    {
        using var stream = File.OpenRead(Path.Combine("Samples", "Microsoft Excel.res"));
        var fork = new ResourceFork(stream);
        DumpFork(fork);

        // Dump CODE the code.
        var code = fork.Map.Types["CODE"];
        foreach (var entry in code)
        {
            Debug.WriteLine($"CODE Resource ID: {entry.ID}, Data Offset: {entry.DataOffset}, Attributes: {entry.Attributes}");
        }

        // Dump the 'STR ' resources.
        var strResources = fork.Map.Types["STR "];
        var strings = new List<string>(strResources.Count);
        foreach (var strResource in strResources)
        {
            var record = new StringRecord(fork.GetResourceData(strResource));
            strings.Add(record.Value);
        }

        File.WriteAllLines(Path.Combine("Output", "Strings.txt"), strings);
        
        // Dump the 'STR#' resources.
        var strListResources = fork.Map.Types["STR#"];
        foreach (var strListResource in strListResources)
        {
            var stringListData = fork.GetResourceData(strListResource);
            ushort stringCount = (ushort)((stringListData[0] << 8) | stringListData[1]);

            var list = new List<string>(stringCount);
            int offset = 2;
            for (int i = 0; i < stringCount; i++)
            {
                byte strLength = stringListData[offset];
                offset += 1;

                string str = Encoding.ASCII.GetString(stringListData, offset, strLength);
                offset += strLength;
                list.Add(str);
            }

            File.WriteAllLines(Path.Combine("Output", $"StringList_{strListResource.ID}.txt"), list);
        }

        // Dump the 'VERS' resource.
        var versResources = fork.Map.Types["VERS"];
        foreach (var versResource in versResources)
        {
            var versData = fork.GetResourceData(versResource);
            var versionRecord = new VersionRecord(versData);
            Debug.WriteLine($"  Version: {versionRecord.Major}.{versionRecord.Minor}");
        }
    }

    private static void DumpFork(ResourceFork fork)
    {
        Debug.WriteLine("Resource Fork Header:");
        Debug.WriteLine($"  Data Offset: {fork.Header.DataOffset}");
        Debug.WriteLine($"  Map Offset: {fork.Header.MapOffset}");
        Debug.WriteLine($"  Data Length: {fork.Header.DataLength}");
        Debug.WriteLine($"  Map Length: {fork.Header.MapLength}");
        Debug.WriteLine("");

        Debug.WriteLine("Resource Fork Map:");
        Debug.WriteLine($"  Resource Types Count: {fork.Map.ResourceTypeCount}");
        foreach (var type in fork.Map.Types)
        {
            Debug.WriteLine($"  Type: {type.Key}, Count: {type.Value.Count}");
            foreach (var entry in type.Value)
            {
                Debug.WriteLine($"    ID: {entry.ID}, Name Offset: {entry.NameOffset}, Data Offset: {entry.DataOffset}, Attributes: {entry.Attributes}");
            }
        }
    }
}
