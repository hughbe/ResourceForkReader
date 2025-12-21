using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using ResourceForkReader.Records;

namespace ResourceForkReader.Tests;

public class ResourceForkTests
{
    [Theory]
    [InlineData("Microsoft Excel.res")]
    [InlineData("Read Me.res")]
    [InlineData("Desktop.res")]
    [InlineData("OneEmptyFolder_Locked.res")]
    [InlineData("OneEmptyFolder_Unlocked.res")]
    [InlineData("Testing/Desktop_NoComment.res")]
    [InlineData("Testing/Desktop_Comment.res")]
    [InlineData("Testing/Desktop_MultiComments.res")]
    public void Ctor_Stream(string fileName)
    {
        var filePath = Path.Combine("Samples", fileName);
        using var stream = File.OpenRead(filePath);

        var fork = new ResourceFork(stream);
        DumpFork(fork);

        Debug.WriteLine("");

        // Dump CODE the code.
        if (fork.Map.Types.TryGetValue("CODE", out var code))
        {
            Debug.WriteLine("CODE Resources:");
            foreach (var entry in code)
            {
                Debug.WriteLine($"  Resource ID: {entry.ID}, Data Offset: {entry.DataOffset}, Attributes: {entry.Attributes}");
            }
        }

        // Dump the 'STR ' resources.
        if (fork.Map.Types.TryGetValue("STR ", out var strResources))
        {
            Debug.WriteLine($"STR Resources: ({strResources.Count} string{(strResources.Count == 1 ? "" : "s")})");
            var strings = new List<string>(strResources.Count);
            foreach (var strResource in strResources)
            {
                var record = new StringRecord(fork.GetResourceData(strResource));
                strings.Add(record.Value);
                Debug.WriteLine($"  String 0x{strResource.ID:X4}: \"{record.Value}\"");
            }

            File.WriteAllLines(Path.Combine("Output", "Strings.txt"), strings);
        }

        // Dump the 'STR#' resources.
        if (fork.Map.Types.TryGetValue("STR#", out var strListResources))
        {
            Debug.WriteLine("STR# Resources:");
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
        }

        // Dump the 'VERS' resource.
        if (fork.Map.Types.TryGetValue("VERS", out var versResources))
        {
            Debug.WriteLine("VERS Resources:");
            foreach (var versResource in versResources)
            {
                var versData = fork.GetResourceData(versResource);
                var versionRecord = new VersionRecord(versData);
                Debug.WriteLine($"  Version 0x{versResource.ID:X4}: {versionRecord.Major}.{versionRecord.Minor}");
            }
        }

        // Dump the 'FREF' resources.
        if (fork.Map.Types.TryGetValue("FREF", out var frefResources))
        {
            Debug.WriteLine("FREF Resources:");
            foreach (var frefResource in frefResources)
            {
                var fileReferenceData = fork.GetResourceData(frefResource);
                var fileReferenceRecord = new FileReferenceRecord(fileReferenceData);
                Debug.WriteLine($"  File Reference 0x{frefResource.ID:X4}: Type={fileReferenceRecord.Type}, LocalIconID={fileReferenceRecord.LocalIconID}, Name={fileReferenceRecord.Name}");
            }
        }

        // Dump the 'FCMT' resources.
        if (fork.Map.Types.TryGetValue("FCMT", out var fcmtResources))
        {
            Debug.WriteLine("FCMT Resources:");
            foreach (var fcmtResource in fcmtResources)
            {
                var fileCommentData = fork.GetResourceData(fcmtResource);
                var fileCommentRecord = new FileCommentRecord(fileCommentData);
                Debug.WriteLine($"  File Comment 0x{fcmtResource.ID:X4}: \"{fileCommentRecord.Comment}\"");
            }
        }

        // Dump the 'BNDL' resources.
        if (fork.Map.Types.TryGetValue("BNDL", out var bndlResources))
        {
            Debug.WriteLine("BNDL Resources:");
            foreach (var bndlResource in bndlResources)
            {
                var bundleData = fork.GetResourceData(bndlResource);
                var bundleRecord = new BundleRecord(bundleData);
                Debug.WriteLine($"  Bundle 0x{bndlResource.ID:X4}: Owner={bundleRecord.Owner}, OwnerID={bundleRecord.OwnerID}, NumberOfTypes={bundleRecord.NumberOfTypes}");
            }
        }

        // Dump the 'FOBJ' resources.
        if (fork.Map.Types.TryGetValue("FOBJ", out var fobjResources))
        {
            Debug.WriteLine("FOBJ Resources:");
            foreach (var fobjResource in fobjResources)
            {
                var fileObjectData = fork.GetResourceData(fobjResource);
                var fileObjectRecord = new FileObjectRecord(fileObjectData);
                Debug.WriteLine($"  File Object 0x{fobjResource.ID:X4}");
                Debug.WriteLine($"    Type: {fileObjectRecord.Type}");
                Debug.WriteLine($"    ParentLocationX: {fileObjectRecord.ParentLocationX}");
                Debug.WriteLine($"    ParentLocationY: {fileObjectRecord.ParentLocationY}");
                Debug.WriteLine($"    Reserved1: 0x{fileObjectRecord.Reserved1:X4}");
                Debug.WriteLine($"    Reserved2: 0x{fileObjectRecord.Reserved2:X4}");
                Debug.WriteLine($"    Reserved3: 0x{fileObjectRecord.Reserved3:X4}");
                Debug.WriteLine($"    Parent: {fileObjectRecord.Parent}");
                Debug.WriteLine($"    Reserved4: 0x{fileObjectRecord.Reserved4:X4}");
                Debug.WriteLine($"    Reserved5: 0x{fileObjectRecord.Reserved5:X4}");
                Debug.WriteLine($"    Reserved6: 0x{fileObjectRecord.Reserved6:X4}");
                Debug.WriteLine($"    Reserved7: 0x{fileObjectRecord.Reserved7:X4}");
                Debug.WriteLine($"    Reserved8: 0x{fileObjectRecord.Reserved8:X4}");
                Debug.WriteLine($"    Reserved9: 0x{fileObjectRecord.Reserved9:X4}");
                Debug.WriteLine($"    CreationDate: {fileObjectRecord.CreationDate}");
                Debug.WriteLine($"    LastModificationDate: {fileObjectRecord.LastModificationDate}");
                Debug.WriteLine($"    Reserved10: 0x{fileObjectRecord.Reserved10:X4}");
                Debug.WriteLine($"    FinderFlags: {fileObjectRecord.Flags}");
            }
        }
    }

    [Fact]
    public void Ctor_NullStream_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>("stream", () => new ResourceFork(null!));
    }

    [Fact]
    public void Ctor_EmptyStream_ThrowsArgumentException()
    {
        using var stream = new MemoryStream();
        Assert.Throws<ArgumentException>("stream", () => new ResourceFork(stream));
    }

    [Fact]
    public void GetResourceData_NullOutputStream_ThrowsArgumentNullException()
    {
        using var stream = File.OpenRead(Path.Combine("Samples", "Microsoft Excel.res"));
        var fork = new ResourceFork(stream);
        var entry = fork.Map.Types["CODE"][0];
        Assert.Throws<ArgumentNullException>("outputStream", () => fork.GetResourceData(entry, null!));
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
                var name = entry.GetName(fork);
                var nameString = string.IsNullOrEmpty(name) ? "" : $" (\"{name}\")";
                Debug.WriteLine($"    ID: {entry.ID}, Name Offset: 0x{entry.NameOffset:X4}{nameString}, Data Offset: 0x{entry.DataOffset:X4}, Attributes: {entry.Attributes}");
            }
        }
    }
}
