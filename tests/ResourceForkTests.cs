using System.ComponentModel;
using System.Diagnostics;
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
    [InlineData("System Startup/Access Privileges.res")]
    [InlineData("System Startup/Apple HD SC Setup.res")]
    [InlineData("System Startup/AppleShare.res")]
    [InlineData("System Startup/Backgrounder.res")]
    [InlineData("System Startup/Brightness.res")]
    [InlineData("System Startup/Clipboard File.res")]
    [InlineData("System Startup/Color.res")]
    [InlineData("System Startup/DA Handler.res")]
    [InlineData("System Startup/DeskTop.res")]
    [InlineData("System Startup/Disk First Aid.res")]
    [InlineData("System Startup/Easy Access.res")]
    [InlineData("System Startup/Finder.res")]
    [InlineData("System Startup/General.res")]
    [InlineData("System Startup/Installer Script.res")]
    [InlineData("System Startup/Installer.res")]
    [InlineData("System Startup/Key Layout.res")]
    [InlineData("System Startup/Keyboard.res")]
    [InlineData("System Startup/Map.res")]
    [InlineData("System Startup/Monitors.res")]
    [InlineData("System Startup/Mouse.res")]
    [InlineData("System Startup/Multifinder.res")]
    [InlineData("System Startup/Portable.res")]
    [InlineData("System Startup/Read Me.res")]
    [InlineData("System Startup/Scrapbook File.res")]
    [InlineData("System Startup/Sound.res")]
    [InlineData("System Startup/Startup Device.res")]
    [InlineData("System Startup/System.res")]
    [InlineData("System Startup/TeachText.res")]
    [InlineData("System Additions/32-Bit QuickDraw.res")]
    [InlineData("System Additions/Apple File Exchange.res")]
    [InlineData("System Additions/AppleTalk ImageWriter.res")]
    [InlineData("System Additions/CloseView.res")]
    [InlineData("System Additions/DCA-RFT_MacWrite.res")]
    [InlineData("System Additions/Desk Accessories.res")]
    [InlineData("System Additions/DeskTop.res")]
    [InlineData("System Additions/Font_DA Mover.res")]
    [InlineData("System Additions/Fonts.res")]
    [InlineData("System Additions/ImageWriter.res")]
    [InlineData("System Additions/LQ AppleTalk ImageWriter.res")]
    [InlineData("System Additions/LQ ImageWriter.res")]
    [InlineData("System Additions/Laser Prep.res")]
    [InlineData("System Additions/LaserWriter.res")]
    [InlineData("System Additions/MacroMaker Help.res")]
    [InlineData("System Additions/MacroMaker.res")]
    [InlineData("System Additions/Macros.res")]
    [InlineData("System Additions/Personal LaserWriter SC.res")]
    [InlineData("System Additions/PrintMonitor.res")]
    [InlineData("System Additions/Read Me for Apple Color.res")]
    [InlineData("System Additions/Responder.res")]
    [InlineData("System Additions/TeachText.res")]
    public void Ctor_Stream(string fileName)
    {
        var filePath = Path.Combine("Samples", fileName);
        using var stream = File.OpenRead(filePath);

        var fork = new ResourceFork(stream);
        DumpFork(fork);
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

        foreach (var type in fork.Map.Types)
        {
            switch (type.Key)
            {
                case ResourceForkType.Code:
                case "Code":
                case ResourceForkType.ControlDefinitionFunction:
                case ResourceForkType.ControlDeviceFunction:
                case ResourceForkType.Driver:
                case ResourceForkType.ListDefinitionFunction:
                case ResourceForkType.Boot:
                case ResourceForkType.SystemExtension:
                case ResourceForkType.Package:
                case ResourceForkType.ROMPatchCode:
                case "ptch":
                case ResourceForkType.MenuDefinitionFunction:
                case ResourceForkType.MenuBarDefinitionFunction:
                case ResourceForkType.ROMOverride:
                case ResourceForkType.RAMCacheControlCode:
                case ResourceForkType.FunctionKeyCode:
                case ResourceForkType.StartupAlertsCode:
                case ResourceForkType.FormattingCode:
                case ResourceForkType.ProcCode:
                case ResourceForkType.Synthesizer:
                case ResourceForkType.MonitorCode:
                case ResourceForkType.WindowDefinitionFunction:
                    Debug.WriteLine("CODE Resources:");
                    foreach (var entry in type.Value)
                    {
                        Debug.WriteLine($"  Resource ID: {entry.ID}, Data Offset: {entry.DataOffset}, Attributes: {entry.Attributes}");
                    }
                    break;

                case ResourceForkType.String:
                    Debug.WriteLine($"STR Resources: ({type.Value.Count} string{(type.Value.Count == 1 ? "" : "s")})");
                    var strings = new List<string>(type.Value.Count);
                    foreach (var strResource in type.Value)
                    {
                        var record = new StringRecord(fork.GetResourceData(strResource));
                        strings.Add(record.Value);
                        Debug.WriteLine($"  String 0x{strResource.ID:X4}: \"{record.Value}\"");
                    }
                    File.WriteAllLines(Path.Combine("Output", "Strings.txt"), strings);
                    break;

                case ResourceForkType.StringList:
                    Debug.WriteLine("STR# Resources:");
                    foreach (var strListResource in type.Value)
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
                    break;

                case ResourceForkType.Version:
                case "vers": // Seen lowercase.
                    Debug.WriteLine("VERS Resources:");
                    foreach (var versResource in type.Value)
                    {
                        var versData = fork.GetResourceData(versResource);
                        var versionRecord = new VersionRecord(versData);
                        Debug.WriteLine($"  Version 0x{versResource.ID:X4}: {versionRecord.Major}.{versionRecord.Minor}");
                    }
                    break;

                case ResourceForkType.FileReference:
                    Debug.WriteLine("FREF Resources:");
                    foreach (var frefResource in type.Value)
                    {
                        var fileReferenceData = fork.GetResourceData(frefResource);
                        var fileReferenceRecord = new FileReferenceRecord(fileReferenceData);
                        Debug.WriteLine($"  File Reference 0x{frefResource.ID:X4}: Type={fileReferenceRecord.Type}, LocalIconID={fileReferenceRecord.LocalIconID}, Name={fileReferenceRecord.Name}");
                    }
                    break;

                case ResourceForkType.FileComment:
                    Debug.WriteLine("FCMT Resources:");
                    foreach (var fcmtResource in type.Value)
                    {
                        var fileCommentData = fork.GetResourceData(fcmtResource);
                        var fileCommentRecord = new FileCommentRecord(fileCommentData);
                        Debug.WriteLine($"  File Comment 0x{fcmtResource.ID:X4}: \"{fileCommentRecord.Comment}\"");
                    }
                    break;

                case ResourceForkType.Bundle:
                    Debug.WriteLine("BNDL Resources:");
                    foreach (var bndlResource in type.Value)
                    {
                        var bundleData = fork.GetResourceData(bndlResource);
                        var bundleRecord = new BundleRecord(bundleData);
                        Debug.WriteLine($"  Bundle 0x{bndlResource.ID:X4}: Owner={bundleRecord.Owner}, OwnerID={bundleRecord.OwnerID}, NumberOfTypes={bundleRecord.NumberOfTypes}");
                    }
                    break;

                case ResourceForkType.FileObject:
                    Debug.WriteLine("FOBJ Resources:");
                    foreach (var fobjResource in type.Value)
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
                    break;

                case ResourceForkType.Icon:
                    Debug.WriteLine("ICON Resources:");
                    foreach (var iconResource in type.Value)
                    {
                        var iconData = fork.GetResourceData(iconResource);
                        var iconRecord = new IconRecord(iconData);
                        Debug.WriteLine($"  Icon 0x{iconResource.ID:X4}: IconData Length={iconRecord.IconData.Length}");
                    }

                    break;

                case ResourceForkType.SystemIcon:
                    Debug.WriteLine("SICN Resources:");
                    foreach (var sicnResource in type.Value)
                    {
                        var sicnData = fork.GetResourceData(sicnResource);
                        var systemIconRecord = new SystemIconRecord(sicnData);
                        Debug.WriteLine($"  System Icon 0x{sicnResource.ID:X4}: IconData Length={systemIconRecord.IconData.Length}");
                    }

                    break;

                case ResourceForkType.Picture:
                    Debug.WriteLine("PICT Resources:");
                    foreach (var pictResource in type.Value)
                    {
                        var pictData = fork.GetResourceData(pictResource);
                        var pictureRecord = new PictureRecord(pictData);
                        Debug.WriteLine($"  Picture 0x{pictResource.ID:X4}: PictureData Length={pictureRecord.PictureData.Length}");
                    }

                    break;

                case ResourceForkType.IconList:
                    Debug.WriteLine("IconList ('ICN#') Resources:");
                    foreach (var iconListResource in type.Value)
                    {
                        var iconListData = fork.GetResourceData(iconListResource);
                        var iconListRecord = new IconListRecord(iconListData);
                        Debug.WriteLine($"  IconList 0x{iconListResource.ID:X4}: IconData Length={iconListRecord.IconData.Length}, MaskData Length={iconListRecord.MaskData.Length}");
                    }

                    break;

                case ResourceForkType.Cursor:
                    Debug.WriteLine("CURS Resources:");
                    foreach (var cursResource in type.Value)
                    {
                        var cursorData = fork.GetResourceData(cursResource);
                        var cursorRecord = new CursorRecord(cursorData);
                        Debug.WriteLine($"  Cursor 0x{cursResource.ID:X4}: HotspotX={cursorRecord.HotspotX}, HotspotY={cursorRecord.HotspotY}, ImageData Length={cursorRecord.ImageData.Length}, MaskData Length={cursorRecord.MaskData.Length}");
                    }

                    break;

                case ResourceForkType.AnimatedCursor:
                case "ACUR":
                    Debug.WriteLine("ACUR Resources:");
                    foreach (var acurResource in type.Value)
                    {
                        var animatedCursorData = fork.GetResourceData(acurResource);
                        var animatedCursorRecord = new AnimatedCursorRecord(animatedCursorData);
                        Debug.WriteLine($"  Animated Cursor 0x{acurResource.ID:X4}: NumberOfFrames={animatedCursorRecord.NumberOfFrames}");
                    }

                    break;
                
                case ResourceForkType.AlertBox:
                    Debug.WriteLine("ALRT Resources:");
                    foreach (var alrtResource in type.Value)
                    {
                        var alertBoxData = fork.GetResourceData(alrtResource);
                        var alertBoxRecord = new AlertBoxRecord(alertBoxData);
                        Debug.WriteLine($"  Alert Box 0x{alrtResource.ID:X4}: Bounds=({alertBoxRecord.Bounds.Top}, {alertBoxRecord.Bounds.Left}, {alertBoxRecord.Bounds.Bottom}, {alertBoxRecord.Bounds.Right}), ItemListResourceID={alertBoxRecord.ItemListResourceID}, AlertInfo=({alertBoxRecord.FirstStageAlertInfo}, {alertBoxRecord.SecondStageAlertInfo}, {alertBoxRecord.ThirdStageAlertInfo}, {alertBoxRecord.FourthStageAlertInfo}), AlertBoxPosition={alertBoxRecord.AlertBoxPosition}");
                    }

                    break;

                case ResourceForkType.ItemList:
                    Debug.WriteLine("DITL Resources:");
                    foreach (var ditlResource in type.Value)
                    {
                        var itemListData = fork.GetResourceData(ditlResource);
                        var itemListRecord = new ItemListRecord(itemListData);
                        Debug.WriteLine($"  Item List 0x{ditlResource.ID:X4}: ItemCount={itemListRecord.Items.Count}");
                        foreach (var item in itemListRecord.Items)
                        {
                            Debug.WriteLine($"    Item: Type={item.Type}, Enabled={item.Enabled}");
                        }
                    }

                    break;

                case ResourceForkType.Menu:
                    Debug.WriteLine("MENU Resources:");
                    foreach (var menuResource in type.Value)
                    {
                        var menuData = fork.GetResourceData(menuResource);
                        var menuRecord = new MenuRecord(menuData);
                        Debug.WriteLine($"  Menu 0x{menuResource.ID:X4}: Title=\"{menuRecord.Title}\", ItemCount={menuRecord.Items.Count}");
                        foreach (var item in menuRecord.Items)
                        {
                            Debug.WriteLine($"    Menu Item: Text=\"{item.Text}\"");
                        }
                    }

                    break;

                case ResourceForkType.MenuBar:
                    Debug.WriteLine("MBAR Resources:");
                    foreach (var mbarResource in type.Value)
                    {
                        var menuBarData = fork.GetResourceData(mbarResource);
                        var menuBarRecord = new MenuBarRecord(menuBarData);
                        Debug.WriteLine($"  Menu Bar 0x{mbarResource.ID:X4}: NumberOfMenus={menuBarRecord.NumberOfMenus}");
                        foreach (var resourceID in menuBarRecord.ResourceIDs)
                        {
                            Debug.WriteLine($"    Menu Resource ID: {resourceID}");
                        }
                    }

                    break;

                case ResourceForkType.DialogBox:
                    Debug.WriteLine("DLOG Resources:");
                    foreach (var dlogResource in type.Value)
                    {
                        var dialogBoxData = fork.GetResourceData(dlogResource);
                        var dialogBoxRecord = new DialogBoxRecord(dialogBoxData);
                        Debug.WriteLine($"  Dialog Box 0x{dlogResource.ID:X4}: Bounds=({dialogBoxRecord.Bounds.Top}, {dialogBoxRecord.Bounds.Left}, {dialogBoxRecord.Bounds.Bottom}, {dialogBoxRecord.Bounds.Right}), WindowDefinitionID={dialogBoxRecord.WindowDefinitionID}, Visible={dialogBoxRecord.Visible}, CloseBox={dialogBoxRecord.CloseBox}, ReferenceConstant={dialogBoxRecord.ReferenceConstant}, ItemListResourceID={dialogBoxRecord.ItemListResourceID}, WindowTitle=\"{dialogBoxRecord.WindowTitle}\", Position={dialogBoxRecord.Position}");
                    }

                    break;

                case ResourceForkType.Size:
                    Debug.WriteLine("SIZE Resources:");
                    foreach (var sizeResource in type.Value)
                    {
                        var sizeData = fork.GetResourceData(sizeResource);
                        var sizeRecord = new SizeRecord(sizeData);
                        Debug.WriteLine($"  Size 0x{sizeResource.ID:X4}: PreferredMemorySize={sizeRecord.PreferredMemorySize}, MinimumMemorySize={sizeRecord.MinimumMemorySize}");
                    }

                    break;

                case ResourceForkType.Control:
                    Debug.WriteLine("CTRL Resources:");
                    foreach (var ctrlResource in type.Value)
                    {
                        var controlData = fork.GetResourceData(ctrlResource);
                        var controlRecord = new ControlRecord(controlData);
                        Debug.WriteLine($"  Control 0x{ctrlResource.ID:X4}: Bounds=({controlRecord.Bounds.Top}, {controlRecord.Bounds.Left}, {controlRecord.Bounds.Bottom}, {controlRecord.Bounds.Right}), InitialSetting={controlRecord.InitialSetting}, Visible={controlRecord.Visible}, Fill={controlRecord.Fill}, MaximumSetting={controlRecord.MaximumSetting}, MinimumSetting={controlRecord.MinimumSetting}, DefinitionID={controlRecord.DefinitionID}, ReferenceValue={controlRecord.ReferenceValue}, Title=\"{controlRecord.Title}\"");
                    }

                    break;

                case ResourceForkType.RectanglePositionsList:
                    Debug.WriteLine("nrct Resources:");
                    foreach (var nrctResource in type.Value)
                    {
                        var nrctData = fork.GetResourceData(nrctResource);
                        var nrctRecord = new RectanglePositionsListRecord(nrctData);
                        Debug.WriteLine($"  Rectangle Positions List 0x{nrctResource.ID:X4}: RectangleCount={nrctRecord.Rectangles.Count}");
                        foreach (var rect in nrctRecord.Rectangles)
                        {
                            Debug.WriteLine($"    RECT: Top={rect.Top}, Left={rect.Left}, Bottom={rect.Bottom}, Right={rect.Right}");
                        }
                    }

                    break;

                case ResourceForkType.Machine:
                    Debug.WriteLine("mach Resources:");
                    foreach (var machResource in type.Value)
                    {
                        var machData = fork.GetResourceData(machResource);
                        var machRecord = new MachineRecord(machData);
                        Debug.WriteLine($"  Machine 0x{machResource.ID:X4}: SoftMask=0x{machRecord.SoftMask:X4}, HardMask=0x{machRecord.HardMask:X4}");
                    }
                    
                    break;

                case ResourceForkType.Window:
                    Debug.WriteLine("WIND Resources:");
                    foreach (var windResource in type.Value)
                    {
                        var windowData = fork.GetResourceData(windResource);
                        var windowRecord = new WindowRecord(windowData);
                        Debug.WriteLine($"  Window 0x{windResource.ID:X4}: Bounds=({windowRecord.Bounds.Top}, {windowRecord.Bounds.Left}, {windowRecord.Bounds.Bottom}, {windowRecord.Bounds.Right}), Visible={windowRecord.Visible}, CloseBox={windowRecord.CloseBox}, ReferenceConstant={windowRecord.ReferenceConstant}, Title=\"{windowRecord.Title}\", Position={windowRecord.Position}");
                    }

                    break;

                case ResourceForkType.ROMOverrideList:
                    Debug.WriteLine("rov# Resources:");
                    foreach (var rovResource in type.Value)
                    {
                        var rovData = fork.GetResourceData(rovResource);
                        var romOverrideListRecord = new ROMOverrideList(rovData);
                        Debug.WriteLine($"  ROM Override List 0x{rovResource.ID:X4}: VersionNumber={romOverrideListRecord.VersionNumber}, NumberOfResources={romOverrideListRecord.NumberOfResources}");
                        foreach (var entry in romOverrideListRecord.Entries)
                        {
                            Debug.WriteLine($"    ResourceType={entry.ResourceType}, ResourceID={entry.ResourceID}");
                        }
                    }

                    break;

                case ResourceForkType.ROMFonts:
                    Debug.WriteLine("FRSV Resources:");
                    foreach (var frsvResource in type.Value)
                    {
                        var frsvData = fork.GetResourceData(frsvResource);
                        var romFontsRecord = new ROMFontsRecord(frsvData);
                        Debug.WriteLine($"  ROM Fonts 0x{frsvResource.ID:X4}: FontCount={romFontsRecord.NumberOfFonts}");
                        foreach (var fontID in romFontsRecord.FontResourceIDs)
                        {
                            Debug.WriteLine($"    Font ID: {fontID}");
                        }
                    }

                    break;

                case ResourceForkType.MouseTracking:
                    Debug.WriteLine("mcky Resources:");
                    foreach (var mckyResource in type.Value)
                    {
                        var mckyData = fork.GetResourceData(mckyResource);
                        var mouseTrackingRecord = new MouseTrackingRecord(mckyData);
                        Debug.WriteLine($"  Mouse Tracking 0x{mckyResource.ID:X4}: Threshold1={mouseTrackingRecord.Threshold1}, Threshold2={mouseTrackingRecord.Threshold2}, Threshold3={mouseTrackingRecord.Threshold3}, Threshold4={mouseTrackingRecord.Threshold4}, Threshold5={mouseTrackingRecord.Threshold5}, Threshold6={mouseTrackingRecord.Threshold6}, Threshold7={mouseTrackingRecord.Threshold7}, Threshold8={mouseTrackingRecord.Threshold8}");
                    }

                    break;

                case ResourceForkType.VideoCard:
                    Debug.WriteLine("card Resources:");
                    foreach (var cardResource in type.Value)
                    {
                        var cardData = fork.GetResourceData(cardResource);
                        var videoCardRecord = new VideoCardRecord(cardData);
                        Debug.WriteLine($"  Video Card 0x{cardResource.ID:X4}: Name=\"{videoCardRecord.Name}\"");
                    }

                    break;

                case ResourceForkType.Rectangle:
                    Debug.WriteLine("RECT Resources:");
                    foreach (var rectResource in type.Value)
                    {
                        var rectData = fork.GetResourceData(rectResource);
                        var rectangleRecord = new RectangleRecord(rectData);
                        Debug.WriteLine($"  Rectangle 0x{rectResource.ID:X4}: Top={rectangleRecord.Rectangle.Top}, Left={rectangleRecord.Rectangle.Left}, Bottom={rectangleRecord.Rectangle.Bottom}, Right={rectangleRecord.Rectangle.Right}");
                    }

                    break;

                case ResourceForkType.Layout:
                    Debug.WriteLine("LAYO Resources:");
                    foreach (var layoutResource in type.Value)
                    {
                        var layoutData = fork.GetResourceData(layoutResource);
                        var layoutRecord = new LayoutRecord(layoutData);
                        Debug.WriteLine($"  Layout 0x{layoutResource.ID:X4}: FontResourceID={layoutRecord.FontResourceID}");
                    }

                    break;

                case ResourceForkType.HelpWindow:
                    Debug.WriteLine("hwin Resources:");
                    foreach (var hwinResource in type.Value)
                    {
                        var hwinData = fork.GetResourceData(hwinResource);
                        var helpWindowRecord = new HelpWindowRecord(hwinData);
                        Debug.WriteLine($"  Help Window 0x{hwinResource.ID:X4}: Version={helpWindowRecord.Version}, Options={helpWindowRecord.Options}, Window Count={helpWindowRecord.WindowComponentCount}");
                        foreach (var component in helpWindowRecord.WindowComponents)
                        {
                            Debug.WriteLine($"    Help Window Component: Type={component.ResourceType}, ID={component.ResourceID}, TitleLengthOrWindowKind={component.LengthOfComparisonStringOrWindowKind}, Title=\"{component.WindowTitleString}\")");
                        }
                    }

                    break;

                case ResourceForkType.Pattern:
                    Debug.WriteLine("PAT Resources:");
                    foreach (var patResource in type.Value)
                    {
                        var patData = fork.GetResourceData(patResource);
                        var patternRecord = new PatternRecord(patData);
                        Debug.WriteLine($"  Pattern 0x{patResource.ID:X4}: PatternData Length={patternRecord.PatternData.Length}");
                    }

                    break;

                case ResourceForkType.PatternList:
                    Debug.WriteLine("PAT# Resources:");
                    foreach (var patListResource in type.Value)
                    {
                        var patListData = fork.GetResourceData(patListResource);
                        var patternListRecord = new PatternListRecord(patListData);
                        Debug.WriteLine($"  Pattern List 0x{patListResource.ID:X4}: PatternCount={patternListRecord.Patterns.Count}");
                        foreach (var pattern in patternListRecord.Patterns)
                        {
                            Debug.WriteLine($"    Pattern: {pattern.PatternData.Length} bytes");
                        }
                    }

                    break;

                case ResourceForkType.FontFamily:
                    Debug.WriteLine("FOND Resources:");
                    foreach (var fondResource in type.Value)
                    {
                        var fondData = fork.GetResourceData(fondResource);
                        var fontFamilyRecord = new FontFamilyRecord(fondData);
                        Debug.WriteLine($"  Font Family 0x{fondResource.ID:X4}: ");
                    }

                    break;
                
                case ResourceForkType.FontInformation:
                    Debug.WriteLine("finf Resources:");
                    foreach (var finfResource in type.Value)
                    {
                        var finfData = fork.GetResourceData(finfResource);
                        var fontInformationRecord = new FontInformationRecord(finfData);
                        Debug.WriteLine($"  Font Information Entries Count: {fontInformationRecord.Entries.Count}");
                        foreach (var fontInfoEntry in fontInformationRecord.Entries)
                        {
                            Debug.WriteLine($"    Font Information Entry: FontID={fontInfoEntry.FontID}, FontStyle={fontInfoEntry.FontStyle}, FontSize={fontInfoEntry.FontSize}");
                        }
                    }

                    break;

                case ResourceForkType.Font:
                    Debug.WriteLine("FONT Resources:");
                    foreach (var fontResource in type.Value)
                    {
                        var fontData = fork.GetResourceData(fontResource);
                        var fontRecord = new FontRecord(fontData);
                        Debug.WriteLine($"  Font 0x{fontResource.ID:X4}: FontData Length={fontRecord.FontData.Length}");
                    }

                    break;

                case ResourceForkType.FontNew:
                    Debug.WriteLine("NFNT Resources:");
                    foreach (var nfntResource in type.Value)
                    {
                        var nfntData = fork.GetResourceData(nfntResource);
                        var newFontRecord = new FontNewRecord(nfntData);
                        Debug.WriteLine($"  New Font 0x{nfntResource.ID:X4}: FontData Length={newFontRecord.FontData.Length}");
                    }

                    break;
                
                case ResourceForkType.Text:
                    Debug.WriteLine("TEXT Resources:");
                    foreach (var textResource in type.Value)
                    {
                        var textData = fork.GetResourceData(textResource);
                        var textRecord = new TextRecord(textData);
                        Debug.WriteLine($"  Text 0x{textResource.ID:X4}: TextData Length={textRecord.Text.Length}");
                    }

                    break;

                case ResourceForkType.LargeIcon4bit:
                    Debug.WriteLine("icl4 Resources:");
                    foreach (var icl4Resource in type.Value)
                    {
                        var icl4Data = fork.GetResourceData(icl4Resource);
                        var icon4bitRecord = new LargeIcon4bitRecord(icl4Data);
                        Debug.WriteLine($"  4-bit Icon 0x{icl4Resource.ID:X4}: IconData Length={icon4bitRecord.IconData.Length}");
                    }

                    break;

                case ResourceForkType.LargeIcon8bit:
                    Debug.WriteLine("icl8 Resources:");
                    foreach (var icl8Resource in type.Value)
                    {
                        var icl8Data = fork.GetResourceData(icl8Resource);
                        var icon8bitRecord = new LargeIcon8bitRecord(icl8Data);
                        Debug.WriteLine($"  8-bit Icon 0x{icl8Resource.ID:X4}: IconData Length={icon8bitRecord.IconData.Length}");
                    }

                    break;

                case ResourceForkType.SmallIcon4bit:
                    Debug.WriteLine("ics4 Resources:");
                    foreach (var ics4Resource in type.Value)
                    {
                        var ics4Data = fork.GetResourceData(ics4Resource);
                        var smallIcon4bitRecord = new SmallIcon4bitRecord(ics4Data);
                        Debug.WriteLine($"  Small 4-bit Icon 0x{ics4Resource.ID:X4}: IconData Length={smallIcon4bitRecord.IconData.Length}");
                    }

                    break;

                case ResourceForkType.SmallIcon8bit:
                    Debug.WriteLine("ics8 Resources:");
                    foreach (var ics8Resource in type.Value)
                    {
                        var ics8Data = fork.GetResourceData(ics8Resource);
                        var smallIcon8bitRecord = new SmallIcon8bitRecord(ics8Data);
                        Debug.WriteLine($"  Small 8-bit Icon 0x{ics8Resource.ID:X4}: IconData Length={smallIcon8bitRecord.IconData.Length}");
                    }

                    break;
                
                case ResourceForkType.SmallIconList:
                    Debug.WriteLine("icl# Resources:");
                    foreach (var iclResource in type.Value)
                    {
                        var iclData = fork.GetResourceData(iclResource);
                        var smallIconListRecord = new SmallIconListRecord(iclData);
                        Debug.WriteLine($"  Small Icon List 0x{iclResource.ID:X4}: IconData Length={smallIconListRecord.IconData.Length}, MaskData Length={smallIconListRecord.MaskData.Length}");
                    }

                    break;

                case ResourceForkType.ColorIcon:
                    Debug.WriteLine("cicn Resources:");
                    foreach (var cicnResource in type.Value)
                    {
                        var cicnData = fork.GetResourceData(cicnResource);
                        var colorIconRecord = new ColorIconRecord(cicnData);
                        Debug.WriteLine($"  Color Icon 0x{cicnResource.ID:X4}: IconData Length={colorIconRecord.IconData.Length}");
                    }

                    break;

                case ResourceForkType.Sound:
                    Debug.WriteLine("snd Resources:");
                    foreach (var sndResource in type.Value)
                    {
                        var sndData = fork.GetResourceData(sndResource);
                        var soundRecord = new SoundRecord(sndData);
                        Debug.WriteLine($"  Sound 0x{sndResource.ID:X4}: SoundData Length={soundRecord.SoundData.Length}");
                    }

                    break;

                case ResourceForkType.Palette:
                    Debug.WriteLine("pltt Resources:");
                    foreach (var plttResource in type.Value)
                    {
                        var plttData = fork.GetResourceData(plttResource);
                        var paletteRecord = new PaletteRecord(plttData);
                        Debug.WriteLine($"  Palette 0x{plttResource.ID:X4}: ColorCount={paletteRecord.Entries.Count}");
                    }

                    break;

                case ResourceForkType.ColorLookupTable:
                case "CLUT":
                case ResourceForkType.DialogColorLookupTable:
                case ResourceForkType.WindowColorTable:
                case ResourceForkType.ControlColorLookupTable:
                    Debug.WriteLine("CLUT Resources:");
                    foreach (var clutResource in type.Value)
                    {
                        var clutData = fork.GetResourceData(clutResource);
                        var clutRecord = new ColorLookupTableRecord(clutData);
                        Debug.WriteLine($"  Color Lookup Table 0x{clutResource.ID:X4}: ColorCount={clutRecord.Entries.Count}");
                    }

                    break;

                case ResourceForkType.InternationalResource:
                case ResourceForkType.PhysicalKeyboardDescription:
                case ResourceForkType.SoftwareKeyboardLayout:
                case ResourceForkType.KeyboardLayout:
                case ResourceForkType.MacroMakerInformation1:
                case ResourceForkType.MacroMakerInformation2:
                case ResourceForkType.MacroMakerInformation3:
                case ResourceForkType.PrintRecord:
                case "GNRL":
                case ResourceForkType.CodeToDrivePrinters:
                case ResourceForkType.PostScript:
                case ResourceForkType.PrinterAccessProtocolAddress:
                case ResourceForkType.Preferences:
                case ResourceForkType.CacheTable:
                case ResourceForkType.ADBDriver:
                case ResourceForkType.PixelPattern:
                case ResourceForkType.PixelPatternList:
                case ResourceForkType.InternationalConfiguration:
                case ResourceForkType.InternationalScriptBundle:
                case ResourceForkType.InternationalFormattingInformation:
                case ResourceForkType.InternationalDateFormattingInformation:
                case ResourceForkType.InternationalStringComparisonPackageHooks:
                case ResourceForkType.InternationalTokenize:
                case ResourceForkType.KeyboardSwapping:
                case ResourceForkType.KeyboardMapping:
                case ResourceForkType.MakeInverseTableQueueSizes:
                    // Documented as existing but format unknown.
                    break;

                case ResourceForkType.CitiesList:
                case "infs":
                case "inra":
                case "inbb":
                case "ERRS":
                case "icmt":
                case "infa":
                case "inpk":
                case "indm":
                    // TODO, not documented but reverse engineered.
                    break;

                case "MACS":
                case "macs":
                case "TTXT":
                case "PCOD":
                case "RFIL":
                case "FRMT":
                case "XCPC":
                case "XCEL":
                case "RES#":
                case "SIZ#":
                case "wmap":
                case "MCFG":
                case "brit":
                case "sldr":
                case "mstr":
                case "mst#":
                case "MACA":
                case "MPNT":
                case "MDRW":
                case "TWIT":
                case "lmem":
                case "mous":
                case "scsi":
                case "SMAP":
                case "acss":
                case "harp":
                case "ttxt":
                case "cdsc":
                case "sysc":
                case "Scav":
                case "keyb":
                case "coll":
                case "cfbj":
                case "FSMT":
                case "ttab":
                case "map ":
                case "infn":
                case "DMOV":
                case "MWRT":
                case "ldex":
                case "ITYP":
                case "rtab":
                case "soun":
                case "colr":
                case "incd":
                case "afps":
                case "EXFS":
                case "sysz":
                case "hdlg":
                case "SCMP":
                case "CMAP":
                case "inft":
                case "LEAK":
                case "prmt":
                case "PSPT":
                case "BSDa":
                case "FNUM":
                case "FIDX":
                case "IWRT":
                case "fsvn":
                case "mdos":
                case "hmnu":
                case "LWSC":
                case "icod":
                case "mime":
                case "BWRT":
                case "GFID":
                case "BSD!":
                case "IWRX":
                case "BWRX":
                case "4NFS":
                case "LPFA":
                case "MCMC":
                case "key#":
                case "LWRW":
                case "CMPT":
                case "PDPD":
                case "CNVT":
                case "RLRL":
                case "MSMS":
                case "INST":
                case "MCPD":
                case "LPLS":
                case "MCMS":
                case "LPLD":
                case "PDMC":
                case "MWLS":
                case "PDMS":
                case "SLPI":
                case "MSMC":
                case "CTRD":
                case "MSPD":
                case "tcod":
                case "tprc":
                case "tprf":
                case "frmt":
                case "LIST":
                case "pgsz":
                case "hfdr":
                case "bjg ":
                case "MMAP":
                case "wedg":
                case "bst#":
                case "INT#":
                case "bvrs":
                    // Unknown.
                    break;

                default:
                    throw new NotImplementedException($"Resource type '{type.Key}' not implemented for detailed dumping.");
            }
        }
    }
}
