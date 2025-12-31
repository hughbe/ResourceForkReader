using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using ResourceForkReader.Records;

namespace ResourceForkReader.Tests;

public class ResourceForkTests
{
    [Theory]
    [InlineData("System1.1/DeskTop.res")]
    [InlineData("System1.1/Disk Copy.res")]
    [InlineData("System1.1/Finder.res")]
    [InlineData("System1.1/Font Mover.res")]
    [InlineData("System1.1/Fonts.res")]
    [InlineData("System1.1/Imagewriter.res")]
    [InlineData("System1.1/Scrapbook File.res")]
    [InlineData("System1.1/System.res")]
    [InlineData("Microsoft Excel.res")]
    [InlineData("ResEdit.res")]
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
    [InlineData("System4.1/Apple File Exchange.res")]
    [InlineData("System4.1/Apple HD SC Setup.res")]
    [InlineData("System4.1/AppleTalk ImageWriter.res")]
    [InlineData("System4.1/DCA-RFT_MacWrite.res")]
    [InlineData("System4.1/Desk Accessories.res")]
    [InlineData("System4.1/DeskTop.res")]
    [InlineData("System4.1/Disk First Aid.res")]
    [InlineData("System4.1/Easy Access.res")]
    [InlineData("System4.1/Finder.res")]
    [InlineData("System4.1/Font_DA Mover.res")]
    [InlineData("System4.1/Fonts.res")]
    [InlineData("System4.1/General.res")]
    [InlineData("System4.1/HDBackup.res")]
    [InlineData("System4.1/ImageWriter.res")]
    [InlineData("System4.1/Key Layout.res")]
    [InlineData("System4.1/Keyboard.res")]
    [InlineData("System4.1/Laser Prep.res")]
    [InlineData("System4.1/LaserWriter.res")]
    [InlineData("System4.1/Monitors.res")]
    [InlineData("System4.1/Mouse.res")]
    [InlineData("System4.1/Sound.res")]
    [InlineData("System4.1/Startup Device.res")]
    [InlineData("System4.1/System.res")]
    [InlineData("System4.1/TeachText.res")]
    [InlineData("System6/AppleTalk ImageWriter.res")]
    [InlineData("System6/Backgrounder.res")]
    [InlineData("System6/DA Handler.res")]
    [InlineData("System6/DeskTop.res")]
    [InlineData("System6/Easy Access.res")]
    [InlineData("System6/Finder.res")]
    [InlineData("System6/General.res")]
    [InlineData("System6/ImageWriter.res")]
    [InlineData("System6/Key Layout.res")]
    [InlineData("System6/Keyboard.res")]
    [InlineData("System6/LQ AppleTalk ImageWriter.res")]
    [InlineData("System6/LQ ImageWriter.res")]
    [InlineData("System6/Laser Prep.res")]
    [InlineData("System6/LaserWriter.res")]
    [InlineData("System6/Mouse.res")]
    [InlineData("System6/MultiFinder.res")]
    [InlineData("System6/Personal LaserWriter SC.res")]
    [InlineData("System6/PrintMonitor.res")]
    [InlineData("System6/Scrapbook File.res")]
    [InlineData("System6/Sound.res")]
    [InlineData("System6/StuffIt Expander Preferences.res")]
    [InlineData("System6/System.res")]
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
                case ResourceForkType.Code1:
                case ResourceForkType.Code2:
                case ResourceForkType.ControlDefinitionFunction:
                case ResourceForkType.ControlDeviceFunction:
                case ResourceForkType.Driver:
                case ResourceForkType.ListDefinitionFunction:
                case ResourceForkType.Boot:
                case ResourceForkType.SystemExtension:
                case ResourceForkType.Package:
                case ResourceForkType.ROMPatchCode1:
                case ResourceForkType.ROMPatchCode2:
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
                case ResourceForkType.KeyboardConfiguration:
                case ResourceForkType.AppleDesktopBusServiceRoutine:
                case ResourceForkType.PrinterDefinitionFunction:
                case ResourceForkType.NameBindingProtocolCode:
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
                        Debug.WriteLine($"  String {strResource}: \"{record.Value}\"");
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

                case ResourceForkType.Version1:
                case ResourceForkType.Version2:
                    Debug.WriteLine("VERS Resources:");
                    foreach (var versResource in type.Value)
                    {
                        var versData = fork.GetResourceData(versResource);
                        var versionRecord = new VersionRecord(versData);
                        Debug.WriteLine($"  Version {versResource}: {versionRecord.Major}.{versionRecord.Minor}");
                    }
                    break;

                case ResourceForkType.FileReference:
                    Debug.WriteLine("FREF Resources:");
                    foreach (var frefResource in type.Value)
                    {
                        var fileReferenceData = fork.GetResourceData(frefResource);
                        var fileReferenceRecord = new FileReferenceRecord(fileReferenceData);
                        Debug.WriteLine($"  File Reference {frefResource}: Type={fileReferenceRecord.Type}, LocalIconID={fileReferenceRecord.LocalIconID}, Name={fileReferenceRecord.Name}");
                    }
                    break;

                case ResourceForkType.FileComment:
                    Debug.WriteLine("FCMT Resources:");
                    foreach (var fcmtResource in type.Value)
                    {
                        var fileCommentData = fork.GetResourceData(fcmtResource);
                        var fileCommentRecord = new FileCommentRecord(fileCommentData);
                        Debug.WriteLine($"  File Comment {fcmtResource}: \"{fileCommentRecord.Comment}\"");
                    }
                    break;

                case ResourceForkType.Bundle:
                    Debug.WriteLine("BNDL Resources:");
                    foreach (var bndlResource in type.Value)
                    {
                        var bundleData = fork.GetResourceData(bndlResource);
                        var bundleRecord = new BundleRecord(bundleData);
                        Debug.WriteLine($"  Bundle {bndlResource}: Owner={bundleRecord.Owner}, OwnerID={bundleRecord.OwnerID}, NumberOfTypes={bundleRecord.NumberOfTypes}");
                    }
                    break;

                case ResourceForkType.FileObject:
                    Debug.WriteLine("FOBJ Resources:");
                    foreach (var fobjResource in type.Value)
                    {
                        var fileObjectData = fork.GetResourceData(fobjResource);
                        var fileObjectRecord = new FileObjectRecord(fileObjectData);
                        Debug.WriteLine($"  File Object {fobjResource}");
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
                        Debug.WriteLine($"  Icon {iconResource}: IconData Length={iconRecord.IconData.Length}");
                    }

                    break;

                case ResourceForkType.SystemIcon:
                    Debug.WriteLine("SICN Resources:");
                    foreach (var sicnResource in type.Value)
                    {
                        var sicnData = fork.GetResourceData(sicnResource);
                        var systemIconRecord = new SystemIconRecord(sicnData);
                        Debug.WriteLine($"  System Icon {sicnResource}: IconData Length={systemIconRecord.IconData.Length}");
                    }

                    break;

                case ResourceForkType.Picture:
                    Debug.WriteLine("PICT Resources:");
                    foreach (var pictResource in type.Value)
                    {
                        var pictData = fork.GetResourceData(pictResource);
                        var pictureRecord = new PictureRecord(pictData);
                        Debug.WriteLine($"  Picture {pictResource}: PictureData Length={pictureRecord.PictureData.Length}");
                    }

                    break;

                case ResourceForkType.IconList:
                    Debug.WriteLine("IconList ('ICN#') Resources:");
                    foreach (var iconListResource in type.Value)
                    {
                        var iconListData = fork.GetResourceData(iconListResource);
                        var iconListRecord = new IconListRecord(iconListData);
                        Debug.WriteLine($"  IconList {iconListResource}: IconData Length={iconListRecord.IconData.Length}, MaskData Length={iconListRecord.MaskData.Length}");
                    }

                    break;

                case ResourceForkType.Cursor:
                    Debug.WriteLine("CURS Resources:");
                    foreach (var cursResource in type.Value)
                    {
                        var cursorData = fork.GetResourceData(cursResource);
                        var cursorRecord = new CursorRecord(cursorData);
                        Debug.WriteLine($"  Cursor {cursResource}: HotspotX={cursorRecord.HotspotX}, HotspotY={cursorRecord.HotspotY}, ImageData Length={cursorRecord.ImageData.Length}, MaskData Length={cursorRecord.MaskData.Length}");
                    }

                    break;

                case ResourceForkType.ColorCursor:
                    Debug.WriteLine("crsr Resources:");
                    foreach (var crsrResource in type.Value)
                    {
                        var colorCursorData = fork.GetResourceData(crsrResource);
                        var colorCursorRecord = new ColorCursorRecord(colorCursorData);
                        Debug.WriteLine($"  Color Cursor {crsrResource}: Type={colorCursorRecord.Type}, PixelMapOffset={colorCursorRecord.PixelMapOffset}, PixelDataOffset={colorCursorRecord.PixelDataOffset}, ExpandedCursorData={colorCursorRecord.ExpandedCursorData}");
                    }

                    break;

                case ResourceForkType.AnimatedCursor1:
                case ResourceForkType.AnimatedCursor2:
                    Debug.WriteLine("ACUR Resources:");
                    foreach (var acurResource in type.Value)
                    {
                        var animatedCursorData = fork.GetResourceData(acurResource);
                        var animatedCursorRecord = new AnimatedCursorRecord(animatedCursorData);
                        Debug.WriteLine($"  Animated Cursor {acurResource}: NumberOfFrames={animatedCursorRecord.NumberOfFrames}");
                    }

                    break;
                
                case ResourceForkType.AlertBox:
                    Debug.WriteLine("ALRT Resources:");
                    foreach (var alrtResource in type.Value)
                    {
                        var alertBoxData = fork.GetResourceData(alrtResource);
                        var alertBoxRecord = new AlertBoxRecord(alertBoxData);
                        Debug.WriteLine($"  Alert Box {alrtResource}: Bounds=({alertBoxRecord.Bounds.Top}, {alertBoxRecord.Bounds.Left}, {alertBoxRecord.Bounds.Bottom}, {alertBoxRecord.Bounds.Right}), ItemListResourceID={alertBoxRecord.ItemListResourceID}, AlertInfo=({alertBoxRecord.FirstStageAlertInfo}, {alertBoxRecord.SecondStageAlertInfo}, {alertBoxRecord.ThirdStageAlertInfo}, {alertBoxRecord.FourthStageAlertInfo}), AlertBoxPosition={alertBoxRecord.AlertBoxPosition}");
                    }

                    break;

                case ResourceForkType.ItemList:
                    Debug.WriteLine("DITL Resources:");
                    foreach (var ditlResource in type.Value)
                    {
                        var itemListData = fork.GetResourceData(ditlResource);
                        var itemListRecord = new ItemListRecord(itemListData);
                        Debug.WriteLine($"  Item List {ditlResource}: ItemCount={itemListRecord.Items.Count}");
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
                        Debug.WriteLine($"  Menu {menuResource}: Title=\"{menuRecord.Title}\", ItemCount={menuRecord.Items.Count}");
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
                        Debug.WriteLine($"  Menu Bar {mbarResource}: NumberOfMenus={menuBarRecord.NumberOfMenus}");
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
                        Debug.WriteLine($"  Dialog Box {dlogResource}: Bounds=({dialogBoxRecord.Bounds.Top}, {dialogBoxRecord.Bounds.Left}, {dialogBoxRecord.Bounds.Bottom}, {dialogBoxRecord.Bounds.Right}), WindowDefinitionID={dialogBoxRecord.WindowDefinitionID}, Visible={dialogBoxRecord.Visible}, CloseBox={dialogBoxRecord.CloseBox}, ReferenceConstant={dialogBoxRecord.ReferenceConstant}, ItemListResourceID={dialogBoxRecord.ItemListResourceID}, WindowTitle=\"{dialogBoxRecord.WindowTitle}\", Position={dialogBoxRecord.Position}");
                    }

                    break;

                case ResourceForkType.Size:
                    Debug.WriteLine("SIZE Resources:");
                    foreach (var sizeResource in type.Value)
                    {
                        var sizeData = fork.GetResourceData(sizeResource);
                        var sizeRecord = new SizeRecord(sizeData);
                        Debug.WriteLine($"  Size {sizeResource}: PreferredMemorySize={sizeRecord.PreferredMemorySize}, MinimumMemorySize={sizeRecord.MinimumMemorySize}");
                    }

                    break;

                case ResourceForkType.Control:
                    Debug.WriteLine("CTRL Resources:");
                    foreach (var ctrlResource in type.Value)
                    {
                        var controlData = fork.GetResourceData(ctrlResource);
                        var controlRecord = new ControlRecord(controlData);
                        Debug.WriteLine($"  Control {ctrlResource}: Bounds=({controlRecord.Bounds.Top}, {controlRecord.Bounds.Left}, {controlRecord.Bounds.Bottom}, {controlRecord.Bounds.Right}), InitialSetting={controlRecord.InitialSetting}, Visible={controlRecord.Visible}, Fill={controlRecord.Fill}, MaximumSetting={controlRecord.MaximumSetting}, MinimumSetting={controlRecord.MinimumSetting}, DefinitionID={controlRecord.DefinitionID}, ReferenceValue={controlRecord.ReferenceValue}, Title=\"{controlRecord.Title}\"");
                    }

                    break;

                case ResourceForkType.RectanglePositionsList:
                    Debug.WriteLine("nrct Resources:");
                    foreach (var nrctResource in type.Value)
                    {
                        var nrctData = fork.GetResourceData(nrctResource);
                        var nrctRecord = new RectanglePositionsListRecord(nrctData);
                        Debug.WriteLine($"  Rectangle Positions List {nrctResource}: RectangleCount={nrctRecord.Rectangles.Count}");
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
                        Debug.WriteLine($"  Machine {machResource}: SoftMask=0x{machRecord.SoftMask:X4}, HardMask=0x{machRecord.HardMask:X4}");
                    }
                    
                    break;

                case ResourceForkType.Window:
                    Debug.WriteLine("WIND Resources:");
                    foreach (var windResource in type.Value)
                    {
                        var windowData = fork.GetResourceData(windResource);
                        var windowRecord = new WindowRecord(windowData);
                        Debug.WriteLine($"  Window {windResource}: Bounds=({windowRecord.Bounds.Top}, {windowRecord.Bounds.Left}, {windowRecord.Bounds.Bottom}, {windowRecord.Bounds.Right}), Visible={windowRecord.Visible}, CloseBox={windowRecord.CloseBox}, ReferenceConstant={windowRecord.ReferenceConstant}, Title=\"{windowRecord.Title}\", Position={windowRecord.Position}");
                    }

                    break;

                case ResourceForkType.ROMOverrideList:
                    Debug.WriteLine("rov# Resources:");
                    foreach (var rovResource in type.Value)
                    {
                        var rovData = fork.GetResourceData(rovResource);
                        var romOverrideListRecord = new ROMOverrideList(rovData);
                        Debug.WriteLine($"  ROM Override List {rovResource}: VersionNumber={romOverrideListRecord.VersionNumber}, NumberOfResources={romOverrideListRecord.NumberOfResources}");
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
                        Debug.WriteLine($"  ROM Fonts {frsvResource}: FontCount={romFontsRecord.NumberOfFonts}");
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
                        Debug.WriteLine($"  Mouse Tracking {mckyResource}: Threshold1={mouseTrackingRecord.Threshold1}, Threshold2={mouseTrackingRecord.Threshold2}, Threshold3={mouseTrackingRecord.Threshold3}, Threshold4={mouseTrackingRecord.Threshold4}, Threshold5={mouseTrackingRecord.Threshold5}, Threshold6={mouseTrackingRecord.Threshold6}, Threshold7={mouseTrackingRecord.Threshold7}, Threshold8={mouseTrackingRecord.Threshold8}");
                    }

                    break;

                case ResourceForkType.VideoCard:
                    Debug.WriteLine("card Resources:");
                    foreach (var cardResource in type.Value)
                    {
                        var cardData = fork.GetResourceData(cardResource);
                        var videoCardRecord = new VideoCardRecord(cardData);
                        Debug.WriteLine($"  Video Card {cardResource}: Name=\"{videoCardRecord.Name}\"");
                    }

                    break;

                case ResourceForkType.Rectangle:
                    Debug.WriteLine("RECT Resources:");
                    foreach (var rectResource in type.Value)
                    {
                        var rectData = fork.GetResourceData(rectResource);
                        var rectangleRecord = new RectangleRecord(rectData);
                        Debug.WriteLine($"  Rectangle {rectResource}: Top={rectangleRecord.Rectangle.Top}, Left={rectangleRecord.Rectangle.Left}, Bottom={rectangleRecord.Rectangle.Bottom}, Right={rectangleRecord.Rectangle.Right}");
                    }

                    break;

                case ResourceForkType.Layout:
                    Debug.WriteLine("LAYO Resources:");
                    foreach (var layoutResource in type.Value)
                    {
                        var layoutData = fork.GetResourceData(layoutResource);
                        var layoutRecord = new LayoutRecord(layoutData);
                        Debug.WriteLine($"  Layout {layoutResource}: FontResourceID={layoutRecord.FontResourceID}");
                    }

                    break;

                case ResourceForkType.HelpWindow:
                    Debug.WriteLine("hwin Resources:");
                    foreach (var hwinResource in type.Value)
                    {
                        var hwinData = fork.GetResourceData(hwinResource);
                        var helpWindowRecord = new HelpWindowRecord(hwinData);
                        Debug.WriteLine($"  Help Window {hwinResource}: Version={helpWindowRecord.Version}, Options={helpWindowRecord.Options}, Window Count={helpWindowRecord.WindowComponentCount}");
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
                        Debug.WriteLine($"  Pattern {patResource}: PatternData Length={patternRecord.PatternData.Length}");
                    }

                    break;

                case ResourceForkType.PatternList:
                    Debug.WriteLine("PAT# Resources:");
                    foreach (var patListResource in type.Value)
                    {
                        var patListData = fork.GetResourceData(patListResource);
                        var patternListRecord = new PatternListRecord(patListData);
                        Debug.WriteLine($"  Pattern List {patListResource}: PatternCount={patternListRecord.Patterns.Count}");
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
                        Debug.WriteLine($"  Font Family {fondResource}: ");
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
                case ResourceForkType.FontNew:
                    Debug.WriteLine("FONT/NFNT Resources:");
                    foreach (var nfntResource in type.Value)
                    {
                        var nfntData = fork.GetResourceData(nfntResource);
                        if (nfntData.Length == 0)
                        {
                            Debug.WriteLine("  Skipping empty FONT/NFNT resource.");
                            continue;   
                        }

                        var newFontRecord = new FontRecord(nfntData);
                        Debug.WriteLine($"  New Font {nfntResource}: BitImageTable Length={newFontRecord.BitImageTable.Length}");
                    }

                    break;
                
                case ResourceForkType.Text:
                    Debug.WriteLine("TEXT Resources:");
                    foreach (var textResource in type.Value)
                    {
                        var textData = fork.GetResourceData(textResource);
                        var textRecord = new TextRecord(textData);
                        Debug.WriteLine($"  Text {textResource}: TextData Length={textRecord.Text.Length}");
                    }

                    break;

                case ResourceForkType.LargeIcon4bit:
                    Debug.WriteLine("icl4 Resources:");
                    foreach (var icl4Resource in type.Value)
                    {
                        var icl4Data = fork.GetResourceData(icl4Resource);
                        var icon4bitRecord = new LargeIcon4bitRecord(icl4Data);
                        Debug.WriteLine($"  4-bit Icon {icl4Resource}: IconData Length={icon4bitRecord.IconData.Length}");
                    }

                    break;

                case ResourceForkType.LargeIcon8bit:
                    Debug.WriteLine("icl8 Resources:");
                    foreach (var icl8Resource in type.Value)
                    {
                        var icl8Data = fork.GetResourceData(icl8Resource);
                        var icon8bitRecord = new LargeIcon8bitRecord(icl8Data);
                        Debug.WriteLine($"  8-bit Icon {icl8Resource}: IconData Length={icon8bitRecord.IconData.Length}");
                    }

                    break;

                case ResourceForkType.SmallIcon4bit:
                    Debug.WriteLine("ics4 Resources:");
                    foreach (var ics4Resource in type.Value)
                    {
                        var ics4Data = fork.GetResourceData(ics4Resource);
                        var smallIcon4bitRecord = new SmallIcon4bitRecord(ics4Data);
                        Debug.WriteLine($"  Small 4-bit Icon {ics4Resource}: IconData Length={smallIcon4bitRecord.IconData.Length}");
                    }

                    break;

                case ResourceForkType.SmallIcon8bit:
                    Debug.WriteLine("ics8 Resources:");
                    foreach (var ics8Resource in type.Value)
                    {
                        var ics8Data = fork.GetResourceData(ics8Resource);
                        var smallIcon8bitRecord = new SmallIcon8bitRecord(ics8Data);
                        Debug.WriteLine($"  Small 8-bit Icon {ics8Resource}: IconData Length={smallIcon8bitRecord.IconData.Length}");
                    }

                    break;
                
                case ResourceForkType.SmallIconList:
                    Debug.WriteLine("icl# Resources:");
                    foreach (var iclResource in type.Value)
                    {
                        var iclData = fork.GetResourceData(iclResource);
                        var smallIconListRecord = new SmallIconListRecord(iclData);
                        Debug.WriteLine($"  Small Icon List {iclResource}: IconData Length={smallIconListRecord.IconData.Length}, MaskData Length={smallIconListRecord.MaskData.Length}");
                    }

                    break;

                case ResourceForkType.ColorIcon:
                    Debug.WriteLine("cicn Resources:");
                    foreach (var cicnResource in type.Value)
                    {
                        var cicnData = fork.GetResourceData(cicnResource);
                        var colorIconRecord = new ColorIconRecord(cicnData);
                        Debug.WriteLine($"  Color Icon {cicnResource}: MaskBitmap Length={colorIconRecord.MaskBitmap.DataSize}, IconBitmap Length={colorIconRecord.IconBitmap.DataSize}");
                    }

                    break;

                case ResourceForkType.Sound:
                    Debug.WriteLine("snd Resources:");
                    foreach (var sndResource in type.Value)
                    {
                        var sndData = fork.GetResourceData(sndResource);
                        if (sndResource.Attributes.HasFlag(ResourceAttributes.Compressed))
                        {
                            Debug.WriteLine($"  Sound {sndResource}: Compressed sound data of length {sndData.Length} bytes.");
                        }
                        else
                        {
                            var soundRecord = new SoundRecord(sndData);
                            Debug.WriteLine($"  Sound {sndResource}: Commands Count={soundRecord.Commands.Count}");
                        }
                    }

                    break;

                case ResourceForkType.Palette:
                    Debug.WriteLine("pltt Resources:");
                    foreach (var plttResource in type.Value)
                    {
                        var plttData = fork.GetResourceData(plttResource);
                        var paletteRecord = new PaletteRecord(plttData);
                        Debug.WriteLine($"  Palette {plttResource}: ColorCount={paletteRecord.Entries.Count}");
                    }

                    break;

                case ResourceForkType.ColorLookupTable1:
                case ResourceForkType.ColorLookupTable2:
                case ResourceForkType.DialogColorLookupTable:
                case ResourceForkType.WindowColorTable:
                case ResourceForkType.ControlColorLookupTable:
                    Debug.WriteLine("CLUT Resources:");
                    foreach (var clutResource in type.Value)
                    {
                        var clutData = fork.GetResourceData(clutResource);
                        var clutRecord = new ColorLookupTableRecord(clutData);
                        Debug.WriteLine($"  Color Lookup Table {clutResource}: ColorCount={clutRecord.ColorTable.Entries.Count}");
                    }

                    break;

                case ResourceForkType.MenuColorTable:
                    Debug.WriteLine("mctb Resources:");
                    foreach (var mctbResource in type.Value)
                    {
                        var mctbData = fork.GetResourceData(mctbResource);
                        var menuColorTableRecord = new MenuColorTableRecord(mctbData);
                        Debug.WriteLine($"  Menu Color Table {mctbResource}: ColorCount={menuColorTableRecord.Entries.Count}");
                    }

                    break;

                case ResourceForkType.PixelPattern:
                    Debug.WriteLine("ppat Resources:");
                    foreach (var ppatResource in type.Value)
                    {
                        var ppatData = fork.GetResourceData(ppatResource);
                        var pixelPatternRecord = new PixelPatternRecord(ppatData);
                        Debug.WriteLine($"  Pixel Pattern {ppatResource}: PixelDataOffset offset={pixelPatternRecord.Pattern.PixelDataOffset}");
                    }

                    break;

                case ResourceForkType.CommandKeys:
                    Debug.WriteLine("CMDK Resources:");
                    foreach (var cmdkResource in type.Value)
                    {
                        var cmdkData = fork.GetResourceData(cmdkResource);
                        var commandKeysRecord = new CommandKeysRecord(cmdkData);
                        Debug.WriteLine($"  Command Keys {cmdkResource}: Command Keys={commandKeysRecord.CommandKeys}");
                    }

                    break;

                case ResourceForkType.ResEditCreatorSignature:
                    Debug.WriteLine("RSED Resources:");
                    foreach (var rsedResource in type.Value)
                    {
                        var rsedData = fork.GetResourceData(rsedResource);
                        var resEditCreatorSignatureRecord = new ResEditCreatorSignatureRecord(rsedData);
                        Debug.WriteLine($"  ResEdit Command Signature {rsedResource}: Signature={resEditCreatorSignatureRecord.Signature}");
                    }

                    break;

                case ResourceForkType.ResEditPicker:
                    Debug.WriteLine("PICK Resources:");
                    foreach (var pickResource in type.Value)
                    {
                        var pickData = fork.GetResourceData(pickResource);
                        var resEditPickerRecord = new ResEditPickerRecord(pickData);
                        Debug.WriteLine($"  ResEdit Picker {pickResource}: Type={resEditPickerRecord.Type}");
                    }

                    break;

                case ResourceForkType.ResEditMap:
                    Debug.WriteLine("RMAP Resources:");
                    foreach (var rmapResource in type.Value)
                    {
                        var rmapData = fork.GetResourceData(rmapResource);
                        var resEditMapRecord = new ResEditMapRecord(rmapData);
                        Debug.WriteLine($"  ResEdit Map {rmapResource}: MapToType={resEditMapRecord.MapToType}");
                    }

                    break;

                case ResourceForkType.ResEditTemplate:
                    Debug.WriteLine("TMPL Resources:");
                    foreach (var tmplResource in type.Value)
                    {
                        var tmplData = fork.GetResourceData(tmplResource);
                        var resEditTemplateRecord = new ResEditTemplateRecord(tmplData);
                        Debug.WriteLine($"  ResEdit Template {tmplResource}: Entries={resEditTemplateRecord.Entries.Count}");
                    }

                    break;

                case ResourceForkType.ResEditDecompressor:
                    Debug.WriteLine("dcmp Resources:");
                    foreach (var dcmpResource in type.Value)
                    {
                        var dcmpData = fork.GetResourceData(dcmpResource);
                        var resEditDecompressorRecord = new ResEditDecompressorRecord(dcmpData);
                        Debug.WriteLine($"  ResEdit Decompressor {dcmpResource}: DecompressorData={resEditDecompressorRecord.DecompressorData.Length}");
                    }

                    break;

                case ResourceForkType.ResEditTool:
                    Debug.WriteLine("TOOL Resources:");
                    foreach (var toolResource in type.Value)
                    {
                        var toolData = fork.GetResourceData(toolResource);
                        var resEditToolRecord = new ResEditToolRecord(toolData);
                        Debug.WriteLine($"  ResEdit Tool {toolResource}: ToolsPerRow{resEditToolRecord.ToolsPerRow}, NumberOfRows={resEditToolRecord.NumberOfRows}");
                    }

                    break;

                case ResourceForkType.ResEditResource:
                    Debug.WriteLine("RSSC Resources:");
                    foreach (var rsscResource in type.Value)
                    {
                        if (rsscResource.Attributes.HasFlag(ResourceAttributes.Compressed))
                        {
                            Debug.WriteLine($"  ResEdit Resource {rsscResource}: Compressed resource data, skipping.");
                        }
                        else
                        {
                            var rsscData = fork.GetResourceData(rsscResource);
                            var resEditResourceRecord = new ResEditResourceRecord(rsscData);
                            Debug.WriteLine($"  ResEdit Resource {rsscResource}: ResourceType={resEditResourceRecord.ResourceType}");
                        }
                    }

                    break;

                case ResourceForkType.ResEditDateFormatting:
                    Debug.WriteLine("ITL1 Resources:");
                    foreach (var toolResource in type.Value)
                    {
                        var toolData = fork.GetResourceData(toolResource);
                        var dateFormattingRecord = new ResEditDateFormattingRecord(toolData);
                        Debug.WriteLine($"  ResEdit Date Formatting {toolResource}: UseShortDatesBeforeSystem={dateFormattingRecord.UseShortDatesBeforeSystem}");
                    }

                    break;

                case ResourceForkType.KeyboardName:
                    Debug.WriteLine("KBDN Resources:");
                    foreach (var kbdnResource in type.Value)
                    {
                        var kbdnData = fork.GetResourceData(kbdnResource);
                        var keyboardNameRecord = new KeyboardNameRecord(kbdnData);
                        Debug.WriteLine($"  Keyboard Name {kbdnResource}: Name=\"{keyboardNameRecord.Name}\"");
                    }

                    break;

                case ResourceForkType.InstallerFileSpec:
                    Debug.WriteLine("infs Resources:");
                    foreach (var infsResource in type.Value)
                    {
                        var infsData = fork.GetResourceData(infsResource);
                        var installerFileSpecRecord = new InstallerFileSpecRecord(infsData);
                        Debug.WriteLine($"  Installer File Spec {infsResource}: FileType={installerFileSpecRecord.FileType}, FileCreator={installerFileSpecRecord.FileCreator}, CreationDate={installerFileSpecRecord.CreationDate}, Flags={installerFileSpecRecord.Flags}, FileName={installerFileSpecRecord.FileName}");
                    }

                    break;

                case ResourceForkType.InstallerPackage:
                    Debug.WriteLine("inpk Resources:");
                    foreach (var inpkResource in type.Value)
                    {
                        var inpkData = fork.GetResourceData(inpkResource);
                        var installerPackageRecord = new InstallerPackageRecord(inpkData);
                        Debug.WriteLine($"  Installer Package {inpkResource}: PackageName=\"{installerPackageRecord.PackageName}\", PackageSize={installerPackageRecord.PackageSize} bytes");
                    }

                    break;

                case ResourceForkType.InstallerComment:
                    Debug.WriteLine("icmt Resources:");
                    foreach (var icmtResource in type.Value)
                    {
                        var icmtData = fork.GetResourceData(icmtResource);
                        var installerCommentRecord = new InstallerCommentRecord(icmtData);
                        Debug.WriteLine($"  Installer Comment {icmtResource}: Comment=\"{installerCommentRecord.Comment}\"");
                    }

                    break;

                case ResourceForkType.InstallerFileAtom:
                    Debug.WriteLine("infa Resources:");
                    foreach (var infaResource in type.Value)
                    {
                        var infaData = fork.GetResourceData(infaResource);
                        var installerFileAtomRecord = new InstallerFileAtomRecord(infaData);
                        Debug.WriteLine($"  Installer File Atom {infaResource}: SourceFileSpecResourceID={installerFileAtomRecord.SourceFileSpecResourceID}, Description=\"{installerFileAtomRecord.Description}\", FileSize={installerFileAtomRecord.FileSize} bytes");
                    }

                    break;

                case ResourceForkType.InstallerResourceAtom:
                    Debug.WriteLine("inra Resources:");
                    foreach (var inraResource in type.Value)
                    {
                        var inraData = fork.GetResourceData(inraResource);
                        var installerResourceAtomRecord = new InstallerResourceAtomRecord(inraData);
                        Debug.WriteLine($"  Installer Resource Atom {inraResource}: SourceFileSpecResourceID={installerResourceAtomRecord.SourceFileSpecResourceID}, TargetFileSpecResourceID={installerResourceAtomRecord.TargetFileSpecResourceID}, ResourceType={installerResourceAtomRecord.ResourceType}, SourceResourceID={installerResourceAtomRecord.SourceResourceID}, TargetResourceID={installerResourceAtomRecord.TargetResourceID}");
                    }

                    break;

                case ResourceForkType.InstallerBootBlock:
                    Debug.WriteLine("inbb Resources:");
                    foreach (var inbbResource in type.Value)
                    {
                        var inbbData = fork.GetResourceData(inbbResource);
                        var installerBootBlockRecord = new InstallerBootBlockRecord(inbbData);
                        Debug.WriteLine($"  Installer Boot Block {inbbResource}: FormatVersion={installerBootBlockRecord.FormatVersion}, Flags={installerBootBlockRecord.Flags}, ValueKey={installerBootBlockRecord.ValueKey}, ValueDataLength={installerBootBlockRecord.ValueData.Length}");
                    }

                    break;

                case ResourceForkType.InstallerCreationDate:
                    Debug.WriteLine("incr Resources:");
                    foreach (var incrResource in type.Value)
                    {
                        var incrData = fork.GetResourceData(incrResource);
                        var installerCreationDateRecord = new InstallerCreationDateRecord(incrData);
                        Debug.WriteLine($"  Installer Creation Date {incrResource}: CreationDate={installerCreationDateRecord.CreationDate}");
                    }

                    break;

                case ResourceForkType.InstallerDefaultMap:
                    Debug.WriteLine("indm Resources:");
                    foreach (var indmResource in type.Value)
                    {
                        var indmData = fork.GetResourceData(indmResource);
                        var installerDefaultMapRecord = new InstallerDefaultMapRecord(indmData);
                        Debug.WriteLine($"  Installer Default Map {indmResource}: Packages Count={installerDefaultMapRecord.NumberOfPackages}");
                    }

                    break;

                case ResourceForkType.CitiesList:
                    Debug.WriteLine("CTYN Resources:");
                    foreach (var ctynResource in type.Value)
                    {
                        var ctynData = fork.GetResourceData(ctynResource);
                        var citiesListRecord = new CitiesListRecord(ctynData);
                        Debug.WriteLine($"  Cities List {ctynResource}: CityCount={citiesListRecord.Cities.Count}");
                        foreach (var city in citiesListRecord.Cities)
                        {
                            Debug.WriteLine($"    City: Name=\"{city.Name}\", Numchars={city.Numchars}, Longitude={city.Longitude}, Latitude={city.Latitude}, GMTDifference={city.GMTDifference}, Reserved={city.Reserved}");
                        }
                    }

                    break;

                case ResourceForkType.General:
                    Debug.WriteLine("GNRL Resources:");
                    foreach (var gneralResource in type.Value)
                    {
                        var generalData = fork.GetResourceData(gneralResource);
                        if (gneralResource.ID == -4096)
                        {
                            var retryRecord = new NBPRetryRecord(generalData);
                            Debug.WriteLine($"  General {gneralResource}: NBPRetryRecord: RetryCount={retryRecord.RetryCount}, RetryInterval={retryRecord.RetryInterval} seconds");
                        }
                        else
                        {
                            Debug.WriteLine($"  General {gneralResource}: Unknown general resource of length {generalData.Length} bytes.");
                        }
                    }

                    break;

                case ResourceForkType.KeyboardSwap:
                    Debug.WriteLine("KSWP Resources:");
                    foreach (var kswpResource in type.Value)
                    {
                        var kswpData = fork.GetResourceData(kswpResource);
                        var keyboardSwapRecord = new KeyboardSwapRecord(kswpData);
                        Debug.WriteLine($"  Keyboard Swap {kswpResource}: Entries Count={keyboardSwapRecord.Entries.Count}");
                        foreach (var entry in keyboardSwapRecord.Entries)
                        {
                            Debug.WriteLine($"    Keyboard Swap Entry: ScriptOrNegativeCode={entry.ScriptOrNegativeCode}, VirtualKeyCode={entry.VirtualKeyCode}, ModifierState={entry.ModifierState}");
                        }
                    }

                    break;

                case ResourceForkType.KeyCaps:
                    Debug.WriteLine("KCAP Resources:");
                    foreach (var kcapResource in type.Value)
                    {
                        var kcapData = fork.GetResourceData(kcapResource);
                        var keyCapsRecord = new KeyCapsRecord(kcapData);
                        Debug.WriteLine($"  Key Caps {kcapResource}: NumberOfShapes={keyCapsRecord.NumberOfShapes}");
                        for (int i = 0; i < keyCapsRecord.Shapes.Count; i++)
                        {
                            var shape = keyCapsRecord.Shapes[i];
                            Debug.WriteLine($"    Shape {i}: NumberOfPointEntries={shape.NumberOfPointEntries}, NumberOfKeyEntries={shape.NumberOfKeyEntries}");
                            for (int j = 0; j < shape.PointEntries.Count; j++)
                            {
                                var pointEntry = shape.PointEntries[j];
                                Debug.WriteLine($"      Point Entry {j}: X={pointEntry.X}, Y={pointEntry.Y}");
                            }
                            for (int k = 0; k < shape.KeyEntries.Count; k++)
                            {
                                var keyEntry = shape.KeyEntries[k];
                                Debug.WriteLine($"      Key Entry {k}: VirtualKeyCode={keyEntry.VirtualKeyCode}, ModifierMask={keyEntry.ModifierMask}");
                            }
                        }
                    }

                    break;

                case ResourceForkType.KeyboardLayout:
                    Debug.WriteLine("KCHR Resources:");
                    foreach (var kchrResource in type.Value)
                    {
                        var kchrData = fork.GetResourceData(kchrResource);
                        var keyboardLayoutRecord = new KeyboardLayoutRecord(kchrData);
                        Debug.WriteLine($"  Software Keyboard Layout {kchrResource}: Version={keyboardLayoutRecord.Version}, NumberOfTables={keyboardLayoutRecord.NumberOfTables}, NumberOfDeadKeyRecords={keyboardLayoutRecord.NumberOfDeadKeyRecords}");
                        Debug.WriteLine($"    Table Selection Index: {BitConverter.ToString(keyboardLayoutRecord.TableSelectionIndex)}");
                        for (int i = 0; i < keyboardLayoutRecord.CharacterMappingTables.Count; i++)
                        {
                            var table = keyboardLayoutRecord.CharacterMappingTables[i];
                            Debug.WriteLine($"    Character Mapping Table {i}: Length={table.Length} bytes");
                        }
                    }

                    break;

                case ResourceForkType.KeyMap:
                    Debug.WriteLine("KMAP Resources:");
                    foreach (var kmapResource in type.Value)
                    {
                        var kmapData = fork.GetResourceData(kmapResource);
                        var keyMapRecord = new KeyMapRecord(kmapData);
                        Debug.WriteLine($"  Key Map {kmapResource}: ID={keyMapRecord.ResourceID}, Version={keyMapRecord.Version}, NumberOfExceptions={keyMapRecord.NumberOfExceptions}");
                    }

                    break;

                case ResourceForkType.SystemVersion1:
                case ResourceForkType.SystemVersion2:
                    Debug.WriteLine("System Version Resources:");
                    foreach (var macsResource in type.Value)
                    {
                        var macsData = fork.GetResourceData(macsResource);
                        var systemVersionRecord = new SystemVersionRecord(macsData);
                        Debug.WriteLine($"  System Version {macsResource}: Version=\"{systemVersionRecord.Version}\"");
                    }

                    break;

                case ResourceForkType.NumericFormatRecord:
                    Debug.WriteLine("itl0 Resources:");
                    foreach (var itl0Resource in type.Value)
                    {
                        var itl0Data = fork.GetResourceData(itl0Resource);
                        var numericFormatRecord = new NumericFormatRecord(itl0Data);
                        Debug.WriteLine($"  Numeric Format {itl0Resource}: DecimalSeparator='{(char)numericFormatRecord.DecimalSeparator}', DateSeparator='{(char)numericFormatRecord.DateSeparator}', TimeSeparator='{(char)numericFormatRecord.TimeSeparator}', TwentyFourHourMorningString=\"{numericFormatRecord.TwentyFourHourMorningString}\", TwentyFourHourEveningString=\"{numericFormatRecord.TwentyFourHourEveningString}\", UnitOfMeasure={numericFormatRecord.UnitOfMeasure}");
                    }

                    break;

                case ResourceForkType.LongDateFormat:
                    Debug.WriteLine("itl1 Resources:");
                    foreach (var itl1Resource in type.Value)
                    {
                        var itl1Data = fork.GetResourceData(itl1Resource);
                        var longDateFormatRecord = new LongDateFormatRecord(itl1Data);
                        Debug.WriteLine($"  Long Date Format {itl1Resource}: DayNamesCount={longDateFormatRecord.DayNames.Length}, MonthNamesCount={longDateFormatRecord.MonthNames.Length}");
                        for (int i = 0; i < longDateFormatRecord.DayNames.Length; i++)
                        {
                            Debug.WriteLine($"    Day {i}: \"{longDateFormatRecord.DayNames[i]}\"");
                        }
                        for (int i = 0; i < longDateFormatRecord.MonthNames.Length; i++)
                        {
                            Debug.WriteLine($"    Month {i}: \"{longDateFormatRecord.MonthNames[i]}\"");
                        }
                    }

                    break;

                case ResourceForkType.InternationalResource:
                    Debug.WriteLine("INTL Resources:");
                    foreach (var intlResource in type.Value)
                    {
                        var intlData = fork.GetResourceData(intlResource);
                        if (intlResource.ID == 0)
                        {
                            var numericFormatRecord = new NumericFormatRecord(intlData);
                            Debug.WriteLine($"  International Resource {intlResource}: DecimalSeparator='{(char)numericFormatRecord.DecimalSeparator}', DateSeparator='{(char)numericFormatRecord.DateSeparator}', TimeSeparator='{(char)numericFormatRecord.TimeSeparator}', TwentyFourHourMorningString=\"{numericFormatRecord.TwentyFourHourMorningString}\", TwentyFourHourEveningString=\"{numericFormatRecord.TwentyFourHourEveningString}\", UnitOfMeasure={numericFormatRecord.UnitOfMeasure}");
                        }
                        else if (intlResource.ID == 1)
                        {
                            var longDateFormatRecord = new LongDateFormatRecord(intlData);
                            Debug.WriteLine($"  International Resource {intlResource}: DayNamesCount={longDateFormatRecord.DayNames.Length}, MonthNamesCount={longDateFormatRecord.MonthNames.Length}");
                            for (int i = 0; i < longDateFormatRecord.DayNames.Length; i++)
                            {
                                Debug.WriteLine($"    Day {i}: \"{longDateFormatRecord.DayNames[i]}\"");
                            }
                            for (int i = 0; i < longDateFormatRecord.MonthNames.Length; i++)
                            {
                                Debug.WriteLine($"    Month {i}: \"{longDateFormatRecord.MonthNames[i]}\"");
                            }
                        }
                        else
                        {
                            Debug.WriteLine($"  International Resource {intlResource}: Unknown international resource of length {intlData.Length} bytes.");
                        }
                    }

                    break;

                case ResourceForkType.CacheTable:
                    Debug.WriteLine("CTAB Resources:");
                    foreach (var ctabResource in type.Value)
                    {
                        var ctabData = fork.GetResourceData(ctabResource);
                        var cacheTableRecord = new CacheTableRecord(ctabData);
                        Debug.WriteLine($"  Cache Table {ctabResource}: Entries Count={cacheTableRecord.Entries.Count}");
                        foreach (var entry in cacheTableRecord.Entries)
                        {
                            Debug.WriteLine($"    Cache Table Entry: Key={entry.Key}");
                        }
                    }

                    break;

                case ResourceForkType.PrinterAccessProtocolAddress:
                    Debug.WriteLine("PAPA Resources:");
                    foreach (var papaResource in type.Value)
                    {
                        var papaData = fork.GetResourceData(papaResource);
                        var papaRecord = new PrinterAccessProtocolAddressRecord(papaData);
                        Debug.WriteLine($"  Printer Access Protocol Address {papaResource}: Name=\"{papaRecord.Name}\", Type={papaRecord.Type}, Zone=\"{papaRecord.Zone}\", AddressBlock=\"{papaRecord.AddressBlock}\"");
                    }

                    break;

                case ResourceForkType.PixelPatternList:
                    Debug.WriteLine("ppt# Resources:");
                    foreach (var pptResource in type.Value)
                    {
                        var pptData = fork.GetResourceData(pptResource);
                        var pixelPatternListRecord = new PixelPatternListRecord(pptData);
                        Debug.WriteLine($"  Pixel Pattern List {pptResource}: Patterns Count={pixelPatternListRecord.Patterns.Count}");
                    }

                    break;

                case ResourceForkType.MakeInverseTableQueueSizes:
                    Debug.WriteLine("mitq Resources:");
                    foreach (var mitqResource in type.Value)
                    {
                        var mitqData = fork.GetResourceData(mitqResource);
                        var mitqRecord = new MakeInverseTableQueueSizesRecord(mitqData);
                        Debug.WriteLine($"  Make Inverse Table Queue Sizes {mitqResource}: QueueSize3Bit={mitqRecord.QueueSize3Bit}, QueueSize4Bit={mitqRecord.QueueSize4Bit}, QueueSize5Bit={mitqRecord.QueueSize5Bit}");
                    }

                    break;

                case ResourceForkType.PrinterRecord:
                    Debug.WriteLine("PREC Resources:");
                    foreach (var precResource in type.Value)
                    {
                        var precData = fork.GetResourceData(precResource);
                        var printRecord = new PrinterRecord(precData);
                        Debug.WriteLine($"  Printer Record {precResource}: PrivateData Length={printRecord.PrivateData.Length} bytes");
                    }

                    break;

                case ResourceForkType.PostScript:
                    Debug.WriteLine("POST Resources:");
                    foreach (var postResource in type.Value)
                    {
                        var postData = fork.GetResourceData(postResource);
                        var postScriptRecord = new PostScriptRecord(postData);
                        Debug.WriteLine($"  PostScript Record {postResource}: NumberOfCommands={postScriptRecord.NumberOfCommands}");
                        for (int i = 0; i < postScriptRecord.Commands.Count; i++)
                        {
                            Debug.WriteLine($"    Command {i}: \"{postScriptRecord.Commands[i]}\"");
                        }
                    }

                    break;

                case ResourceForkType.Preferences:
                    Debug.WriteLine("PREF Resources:");
                    foreach (var prefResource in type.Value)
                    {
                        var prefData = fork.GetResourceData(prefResource);
                        var preferencesRecord = new PreferencesRecord(prefData);
                        Debug.WriteLine($"  Preferences Record {prefResource}: Data=\"{preferencesRecord.Data}\"");
                    }

                    break;

                case ResourceForkType.Tokens:
                    Debug.WriteLine("itl4 Resources:");
                    foreach (var itl4Resource in type.Value)
                    {
                        var itl4Data = fork.GetResourceData(itl4Resource);
                        var tokensRecord = new TokensRecord(itl4Data);
                        Debug.WriteLine($"  Tokens Record {itl4Resource}: ResourceType={tokensRecord.ResourceType}, ResourceID={tokensRecord.ResourceID}, VersionNumber={tokensRecord.VersionNumber}, FormatCode={tokensRecord.FormatCode}");
                    }

                    break;

                case ResourceForkType.InternationalConfiguration:
                    Debug.WriteLine("itlc Resources:");
                    foreach (var itlcResource in type.Value)
                    {
                        var itlcData = fork.GetResourceData(itlcResource);
                        var internationalConfigurationRecord = new InternationalConfigurationRecord(itlcData);
                        Debug.WriteLine($"  International Configuration {itlcResource}: SystemScriptCode={internationalConfigurationRecord.SystemScriptCode}");
                    }

                    break;

                case ResourceForkType.InternationalBundle:
                    Debug.WriteLine("itlb Resources:");
                    foreach (var itlbResource in type.Value)
                    {
                        var itlbData = fork.GetResourceData(itlbResource);
                        var internationalBundleRecord = new InternationalBundleRecord(itlbData);
                        Debug.WriteLine($"  International Bundle {itlbResource}: NumericFormatResourceID={internationalBundleRecord.NumericFormatResourceID}, LongDateFormatResourceID={internationalBundleRecord.DateFormatResourceID}");
                    }

                    break;

                case ResourceForkType.StringManipulation:
                    Debug.WriteLine("itl2 Resources:");
                    foreach (var itl2Resource in type.Value)
                    {
                        var itl2Data = fork.GetResourceData(itl2Resource);
                        var stringManipulationRecord = new StringManipulationRecord(itl2Data);
                        Debug.WriteLine($"  String Manipulation {itl2Resource}: Data Length={itl2Data.Length} bytes");
                    }

                    break;

                case ResourceForkType.FinderVersion: // "System 1.1/Finder.res"
                    Debug.WriteLine("FNDR Resources:");
                    foreach (var fndrResource in type.Value)
                    {
                        var fndrData = fork.GetResourceData(fndrResource);
                        var finderVersionRecord = new FinderVersionRecord(fndrData);
                        Debug.WriteLine($"  Finder Version {fndrResource}: VersionString=\"{finderVersionRecord.VersionString}\"");
                    }

                    break;

                case ResourceForkType.CachedIconList:
                    Debug.WriteLine("clst Resources:");
                    foreach (var clstResource in type.Value)
                    {
                        var clstData = fork.GetResourceData(clstResource);
                        var cachedIconListRecord = new CachedIconListRecord(clstData);
                        Debug.WriteLine($"  Cached Icon List {clstResource}: NumberOfEntries={cachedIconListRecord.NumberOfEntries}");
                        for (int i = 0; i < cachedIconListRecord.Entries.Count; i++)
                        {
                            var entry = cachedIconListRecord.Entries[i];
                            Debug.WriteLine($"    Entry {i}: Unknown={entry.Unknown1}, Type={entry.Type}");
                        }
                    }

                    break;

                case ResourceForkType.InstallerScript:
                    Debug.WriteLine("insc Resources:");
                    foreach (var inscResource in type.Value)
                    {
                        var inscData = fork.GetResourceData(inscResource);
                        var installerConfigurationRecord = new InstallerScriptRecord(inscData);
                        Debug.WriteLine($"  Installer Configuration {inscResource}: Version={installerConfigurationRecord.Version}, Flags={installerConfigurationRecord.Flags}, Name=\"{installerConfigurationRecord.Name}\", HelpString=\"{installerConfigurationRecord.HelpString}\"");
                    }

                    break;

                case ResourceForkType.MacroMakerInformation1:
                case ResourceForkType.MacroMakerInformation2:
                case ResourceForkType.MacroMakerInformation3:
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
                case "PATC":
                case "ERRS":
                case "FMOV": // "System 1.1/Font Mover.res"
                case "QUIK": // "System 1.1/Disk Copy.res"
                case "FVIS": // "System 1.1/Finder.res"
                case "TAB#": // "System 1.1/Finder.res"
                case "sdev": // "System 4.1/Apple HD SC Setup.res"
                case "MrBK": // "System 4.1/HDBackup.res" and "System 4.1/DeskTop.res"
                case "LWRT": // "System 4.1/LaserWriter.res"
                case "FAST": // "System 4.1/Finder.res"
                case "ExpP": // "System 6/StuffIt Epxander Preferences.res"
                case "Page": // "System 6/StuffIt Epxander Preferences.res"
                    // Unknown.
                    break;

                default:
                    throw new NotImplementedException($"Resource type '{type.Key}' not implemented for detailed dumping.");
            }
        }
    }
}
