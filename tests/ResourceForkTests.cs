using System.Diagnostics;
using System.Text;
using ResourceForkReader.Records;

namespace ResourceForkReader.Tests;

public class ResourceForkTests
{
    [Theory]
    [InlineData("Microsoft Excel.res")]
    [InlineData("ResEdit.res")]
    [InlineData("Read Me.res")]
    [InlineData("Desktop.res")]
    [InlineData("OneEmptyFolder_Locked.res")]
    [InlineData("OneEmptyFolder_Unlocked.res")]
    [InlineData("Testing/Desktop_NoComment.res")]
    [InlineData("Testing/Desktop_Comment.res")]
    [InlineData("Testing/Desktop_MultiComments.res")]
    [InlineData("System1.0/DeskTop.res")]
    [InlineData("System1.0/Disk Copy.res")]
    [InlineData("System1.0/Finder.res")]
    [InlineData("System1.0/Font Mover.res")]
    [InlineData("System1.0/Fonts.res")]
    [InlineData("System1.0/Imagewriter.res")]
    [InlineData("System1.0/Scrapbook File.res")]
    [InlineData("System1.0/System.res")]
    [InlineData("System1.0/SysVersion.res")]
    [InlineData("System1.1/DeskTop.res")]
    [InlineData("System1.1/Disk Copy.res")]
    [InlineData("System1.1/Finder.res")]
    [InlineData("System1.1/Font Mover.res")]
    [InlineData("System1.1/Fonts.res")]
    [InlineData("System1.1/Imagewriter.res")]
    [InlineData("System1.1/Scrapbook File.res")]
    [InlineData("System1.1/System.res")]
    [InlineData("System2.0/Finder 1.1/Desktop.res")]
    [InlineData("System2.0/Finder 1.1/Disk Copy.res")]
    [InlineData("System2.0/Finder 1.1/Finder.res")]
    [InlineData("System2.0/Finder 1.1/Font Mover.res")]
    [InlineData("System2.0/Finder 1.1/Fonts.res")]
    [InlineData("System2.0/Finder 1.1/Imagewriter.res")]
    [InlineData("System2.0/Finder 1.1/SysVersion.res")]
    [InlineData("System2.0/Finder 1.1/System.res")]
    [InlineData("System2.0/128K Disk Copy.res")]
    [InlineData("System2.0/Desktop.res")]
    [InlineData("System2.0/Finder.res")]
    [InlineData("System2.0/Font_DA Mover.res")]
    [InlineData("System2.0/Fonts.res")]
    [InlineData("System2.0/ImageWriter.res")]
    [InlineData("System2.0/Scrapbook File.res")]
    [InlineData("System2.0/System.res")]
    [InlineData("System2.1/Desktop.res")]
    [InlineData("System2.1/Finder.res")]
    [InlineData("System2.1/HD 20 Test.res")]
    [InlineData("System2.1/HD Diag.res")]
    [InlineData("System2.1/Hard Disk 20.res")]
    [InlineData("System2.1/ImageWriter.res")]
    [InlineData("System2.1/Scrapbook File.res")]
    [InlineData("System2.1/System.res")]
    [InlineData("System3.0/Desktop.res")]
    [InlineData("System3.0/System Folder/AppleTalk ImageWriter.res")]
    [InlineData("System3.0/System Folder/Finder.res")]
    [InlineData("System3.0/System Folder/Font_DA Mover.res")]
    [InlineData("System3.0/System Folder/ImageWriter.res")]
    [InlineData("System3.0/System Folder/Laser Prep.res")]
    [InlineData("System3.0/System Folder/LaserWriter.res")]
    [InlineData("System3.0/System Folder/Scrapbook File.res")]
    [InlineData("System3.0/System Folder/System.res")]
    [InlineData("System3.0/Utilities Folder/Desk Accessories.res")]
    [InlineData("System3.0/Utilities Folder/Fonts.res")]
    [InlineData("System3.0/Utilities Folder/Installer.res")]
    [InlineData("System3.0/Utilities Folder/Installer Scripts/Mac Plus Update.res")]
    [InlineData("System3.1.1/Desktop.res")]
    [InlineData("System3.1.1/File.res")]
    [InlineData("System3.1.1/Mousing Around.res")]
    [InlineData("System3.1.1/readme.res")]
    [InlineData("System3.1.1/System Folder/Calc Scene.res")]
    [InlineData("System3.1.1/System Folder/Caps scene.res")]
    [InlineData("System3.1.1/System Folder/Control Scene.res")]
    [InlineData("System3.1.1/System Folder/Desktop 1.res")]
    [InlineData("System3.1.1/System Folder/Finder.res")]
    [InlineData("System3.1.1/System Folder/ImageWriter.res")]
    [InlineData("System3.1.1/System Folder/OVWT.res")]
    [InlineData("System3.1.1/System Folder/Scrap Scene.res")]
    [InlineData("System3.1.1/System Folder/Scrapbook File.res")]
    [InlineData("System3.1.1/System Folder/Sounds.res")]
    [InlineData("System3.1.1/System Folder/System.res")]
    [InlineData("System3.1.1/System Folder/Tour Guide.res")]
    [InlineData("System3.1.1/System Folder/Tour List.res")]
    [InlineData("System3.2/Desktop.res")]
    [InlineData("System3.2/Font_DA Mover.res")]
    [InlineData("System3.2/HD 20 Test.res")]
    [InlineData("System3.2/System Folder/Finder.res")]
    [InlineData("System3.2/System Folder/Hard Disk 20.res")]
    [InlineData("System3.2/System Folder/ImageWriter.res")]
    [InlineData("System3.2/System Folder/Scrapbook File.res")]
    [InlineData("System3.2/System Folder/System.res")]
    [InlineData("System3.4/Desktop.res")]
    [InlineData("System3.4/Installer.res")]
    [InlineData("System3.4/Installer Scripts/AppleShare Script.res")]
    [InlineData("System3.4/Installer Scripts/Mac System 512Ke Script.res")]
    [InlineData("System3.4/System Folder/AppleShare.res")]
    [InlineData("System3.4/System Folder/Finder.res")]
    [InlineData("System3.4/System Folder/System.res")]
    [InlineData("System5/Desktop.res")]
    [InlineData("System5/System Tools 1/TeachText.res")]
    [InlineData("System5/System Tools 1/System Folder/Color.res")]
    [InlineData("System5/System Tools 1/System Folder/DA?Handler.res")]
    [InlineData("System5/System Tools 1/System Folder/Easy Access.res")]
    [InlineData("System5/System Tools 1/System Folder/Finder.res")]
    [InlineData("System5/System Tools 1/System Folder/General.res")]
    [InlineData("System5/System Tools 1/System Folder/Key Layout.res")]
    [InlineData("System5/System Tools 1/System Folder/Keyboard.res")]
    [InlineData("System5/System Tools 1/System Folder/Monitors.res")]
    [InlineData("System5/System Tools 1/System Folder/Mouse.res")]
    [InlineData("System5/System Tools 1/System Folder/MultiFinder.res")]
    [InlineData("System5/System Tools 1/System Folder/Scrapbook File.res")]
    [InlineData("System5/System Tools 1/System Folder/Sound.res")]
    [InlineData("System5/System Tools 1/System Folder/Startup Device.res")]
    [InlineData("System5/System Tools 1/System Folder/System.res")]
    [InlineData("System5/System Tools 1/Update Folder/Read Me.res")]
    [InlineData("System5/System Tools 1/Utilities Folder/Apple HD SC Setup.res")]
    [InlineData("System5/System Tools 1/Utilities Folder/Installer.res")]
    [InlineData("System5/System Tools 1/Utilities Folder/Installer Scripts/Macintosh II Script.res")]
    [InlineData("System5/System Tools 1/Utilities Folder/Installer Scripts/Macintosh Plus Script.res")]
    [InlineData("System5/System Tools 1/Utilities Folder/Installer Scripts/Macintosh SE Script.res")]
    [InlineData("System5/System Tools 2/Installer.res")]
    [InlineData("System5/System Tools 2/TeachText.res")]
    [InlineData("System5/System Tools 2/System Folder/AppleTalk ImageWriter.res")]
    [InlineData("System5/System Tools 2/System Folder/Background Printing.res")]
    [InlineData("System5/System Tools 2/System Folder/Backgrounder.res")]
    [InlineData("System5/System Tools 2/System Folder/Color.res")]
    [InlineData("System5/System Tools 2/System Folder/DA?Handler.res")]
    [InlineData("System5/System Tools 2/System Folder/Easy Access.res")]
    [InlineData("System5/System Tools 2/System Folder/Finder.res")]
    [InlineData("System5/System Tools 2/System Folder/General.res")]
    [InlineData("System5/System Tools 2/System Folder/ImageWriter.res")]
    [InlineData("System5/System Tools 2/System Folder/Key Layout.res")]
    [InlineData("System5/System Tools 2/System Folder/Keyboard.res")]
    [InlineData("System5/System Tools 2/System Folder/Laser Prep.res")]
    [InlineData("System5/System Tools 2/System Folder/LaserWriter.res")]
    [InlineData("System5/System Tools 2/System Folder/Mouse.res")]
    [InlineData("System5/System Tools 2/System Folder/MultiFinder.res")]
    [InlineData("System5/System Tools 2/System Folder/PrintMonitor.res")]
    [InlineData("System5/System Tools 2/System Folder/Scrapbook File.res")]
    [InlineData("System5/System Tools 2/System Folder/Sound.res")]
    [InlineData("System5/System Tools 2/System Folder/Startup Device.res")]
    [InlineData("System5/System Tools 2/System Folder/System.res")]
    [InlineData("System5/System Tools 2/Update Folder/Read Me.res")]
    [InlineData("System5/Utilities 1/Apple HD SC Setup.res")]
    [InlineData("System5/Utilities 1/Disk First Aid.res")]
    [InlineData("System5/Utilities 1/HDBackup.res")]
    [InlineData("System5/Utilities 1/TeachText.res")]
    [InlineData("System5/Utilities 1/System Folder/Color.res")]
    [InlineData("System5/Utilities 1/System Folder/DA?Handler.res")]
    [InlineData("System5/Utilities 1/System Folder/Easy Access.res")]
    [InlineData("System5/Utilities 1/System Folder/Finder.res")]
    [InlineData("System5/Utilities 1/System Folder/General.res")]
    [InlineData("System5/Utilities 1/System Folder/Key Layout.res")]
    [InlineData("System5/Utilities 1/System Folder/Keyboard.res")]
    [InlineData("System5/Utilities 1/System Folder/Monitors.res")]
    [InlineData("System5/Utilities 1/System Folder/Mouse.res")]
    [InlineData("System5/Utilities 1/System Folder/MultiFinder.res")]
    [InlineData("System5/Utilities 1/System Folder/Scrapbook File.res")]
    [InlineData("System5/Utilities 1/System Folder/Sound.res")]
    [InlineData("System5/Utilities 1/System Folder/Startup Device.res")]
    [InlineData("System5/Utilities 1/System Folder/System.res")]
    [InlineData("System5/Utilities 1/Update Folder/Read Me.res")]
    [InlineData("System5/Utilities 2/Apple File Exchange Folder/Apple File Exchange.res")]
    [InlineData("System5/Utilities 2/Apple File Exchange Folder/DCA-RFT_MacWrite.res")]
    [InlineData("System5/Utilities 2/Font_DA Mover Folder/Desk Accessories.res")]
    [InlineData("System5/Utilities 2/Font_DA Mover Folder/Font_DA Mover.res")]
    [InlineData("System5/Utilities 2/Font_DA Mover Folder/Fonts.res")]
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
    [InlineData("System7/About System 7.5.5 Update.res")]
    [InlineData("System7/About System 7.5.res")]
    [InlineData("System7/Desktop.res")]
    [InlineData("System7/Read Me.res")]
    [InlineData("System7/SimpleText.res")]
    [InlineData("System7/Apple Extras/About the MacOS/About the Control Panels folder.res")]
    [InlineData("System7/Apple Extras/About the MacOS/About the Extensions folder.res")]
    [InlineData("System7/Apple Extras/About the MacOS/About the System Folder.res")]
    [InlineData("System7/Apple Extras/AppleCD Audio Player/AppleCD Audio Player Guide.res")]
    [InlineData("System7/Apple Extras/AppleCD Audio Player/AppleCD Audio Player.res")]
    [InlineData("System7/Apple Extras/AppleScript?/Script Editor.res")]
    [InlineData("System7/Apple Extras/AppleScript?/Using AppleScript part 1.res")]
    [InlineData("System7/Apple Extras/AppleScript?/Using AppleScript part 2.res")]
    [InlineData("System7/Apple Extras/AppleScript?/Automated Tasks/About Automated Tasks.res")]
    [InlineData("System7/Apple Extras/AppleScript?/Automated Tasks/Add Alias to Apple Menu.res")]
    [InlineData("System7/Apple Extras/AppleScript?/Automated Tasks/Find Original from Alias.res")]
    [InlineData("System7/Apple Extras/AppleScript?/Automated Tasks/Share a Folder (no Guest).res")]
    [InlineData("System7/Apple Extras/AppleScript?/Automated Tasks/Share a Folder.res")]
    [InlineData("System7/Apple Extras/AppleScript?/Automated Tasks/Start File Sharing.res")]
    [InlineData("System7/Apple Extras/AppleScript?/Automated Tasks/Stop File Sharing.res")]
    [InlineData("System7/Apple Extras/AppleScript?/Automated Tasks/Turn Sound Off.res")]
    [InlineData("System7/Apple Extras/AppleScript?/Automated Tasks/Turn Sound On.res")]
    [InlineData("System7/Apple Extras/AppleScript?/More Automated Tasks/About More Automated Tasks.res")]
    [InlineData("System7/Apple Extras/AppleScript?/More Automated Tasks/Alert When Folder Changes.res")]
    [InlineData("System7/Apple Extras/AppleScript?/More Automated Tasks/Change Monitor to 256.res")]
    [InlineData("System7/Apple Extras/AppleScript?/More Automated Tasks/Change Monitor to B&W.res")]
    [InlineData("System7/Apple Extras/AppleScript?/More Automated Tasks/Hide_Show Folder Sizes.res")]
    [InlineData("System7/Apple Extras/AppleScript?/More Automated Tasks/Synchronize Folders.res")]
    [InlineData("System7/System Folder/Finder.res")]
    [InlineData("System7/System Folder/MacTCP DNR.res")]
    [InlineData("System7/System Folder/Scrapbook File.res")]
    [InlineData("System7/System Folder/System.res")]
    [InlineData("System7/System Folder/Apple Menu Items/? Shut Down.res")]
    [InlineData("System7/System Folder/Apple Menu Items/Alarm Clock.res")]
    [InlineData("System7/System Folder/Apple Menu Items/AppleCD Audio Player.res")]
    [InlineData("System7/System Folder/Apple Menu Items/Automated Tasks.res")]
    [InlineData("System7/System Folder/Apple Menu Items/Calculator.res")]
    [InlineData("System7/System Folder/Apple Menu Items/Chooser.res")]
    [InlineData("System7/System Folder/Apple Menu Items/Control Panels.res")]
    [InlineData("System7/System Folder/Apple Menu Items/Find File.res")]
    [InlineData("System7/System Folder/Apple Menu Items/Jigsaw Puzzle.res")]
    [InlineData("System7/System Folder/Apple Menu Items/Key Caps.res")]
    [InlineData("System7/System Folder/Apple Menu Items/Note Pad.res")]
    [InlineData("System7/System Folder/Apple Menu Items/Puzzle.res")]
    [InlineData("System7/System Folder/Apple Menu Items/Scrapbook.res")]
    [InlineData("System7/System Folder/Apple Menu Items/Stickies.res")]
    [InlineData("System7/System Folder/Apple Menu Items/Recent Applications/Desktop Patterns.res")]
    [InlineData("System7/System Folder/Apple Menu Items/Recent Applications/ExportFl.res")]
    [InlineData("System7/System Folder/Apple Menu Items/Recent Applications/ImportFl.res")]
    [InlineData("System7/System Folder/Apple Menu Items/Recent Applications/Installer.res")]
    [InlineData("System7/System Folder/Apple Menu Items/Recent Applications/Org Plus .res")]
    [InlineData("System7/System Folder/Apple Menu Items/Recent Applications/SimpleText.res")]
    [InlineData("System7/System Folder/Apple Menu Items/Recent Applications/StuffIt Expander? 401 Installer.res")]
    [InlineData("System7/System Folder/Apple Menu Items/Recent Applications/StuffIt Expander?.res")]
    [InlineData("System7/System Folder/Apple Menu Items/Recent Documents/About System 7.5.5 Update.res")]
    [InlineData("System7/System Folder/Apple Menu Items/Recent Documents/FontDefault.opx.res")]
    [InlineData("System7/System Folder/Apple Menu Items/Recent Documents/FontStyle_Shadow.opx.res")]
    [InlineData("System7/System Folder/Apple Menu Items/Recent Documents/Read Me.res")]
    [InlineData("System7/System Folder/Apple Menu Items/Recent Documents/System 7.5.5 Update Install.res")]
    [InlineData("System7/System Folder/Control Panels/Apple Menu Options.res")]
    [InlineData("System7/System Folder/Control Panels/Date & Time.res")]
    [InlineData("System7/System Folder/Control Panels/Desktop Patterns.res")]
    [InlineData("System7/System Folder/Control Panels/Easy Access.res")]
    [InlineData("System7/System Folder/Control Panels/Extensions Manager.res")]
    [InlineData("System7/System Folder/Control Panels/File Sharing Monitor.res")]
    [InlineData("System7/System Folder/Control Panels/General Controls.res")]
    [InlineData("System7/System Folder/Control Panels/Keyboard.res")]
    [InlineData("System7/System Folder/Control Panels/Labels.res")]
    [InlineData("System7/System Folder/Control Panels/Launcher.res")]
    [InlineData("System7/System Folder/Control Panels/MacTCP.res")]
    [InlineData("System7/System Folder/Control Panels/Macintosh Easy Open.res")]
    [InlineData("System7/System Folder/Control Panels/Map.res")]
    [InlineData("System7/System Folder/Control Panels/Memory.res")]
    [InlineData("System7/System Folder/Control Panels/Mouse.res")]
    [InlineData("System7/System Folder/Control Panels/Network.res")]
    [InlineData("System7/System Folder/Control Panels/Numbers.res")]
    [InlineData("System7/System Folder/Control Panels/PC Exchange.res")]
    [InlineData("System7/System Folder/Control Panels/Sharing Setup.res")]
    [InlineData("System7/System Folder/Control Panels/Sound.res")]
    [InlineData("System7/System Folder/Control Panels/Startup Disk.res")]
    [InlineData("System7/System Folder/Control Panels/Text.res")]
    [InlineData("System7/System Folder/Control Panels/Users & Groups.res")]
    [InlineData("System7/System Folder/Control Panels/Views.res")]
    [InlineData("System7/System Folder/Control Panels/WindowShade.res")]
    [InlineData("System7/System Folder/Extensions/ EM Extension.res")]
    [InlineData("System7/System Folder/Extensions/About Apple Guide.res")]
    [InlineData("System7/System Folder/Extensions/Apple CD-ROM.res")]
    [InlineData("System7/System Folder/Extensions/Apple Guide.res")]
    [InlineData("System7/System Folder/Extensions/AppleScript?.res")]
    [InlineData("System7/System Folder/Extensions/AppleShare.res")]
    [InlineData("System7/System Folder/Extensions/AppleTalk ImageWriter.res")]
    [InlineData("System7/System Folder/Extensions/Audio CD Access.res")]
    [InlineData("System7/System Folder/Extensions/Caps Lock.res")]
    [InlineData("System7/System Folder/Extensions/Clipping Extension.res")]
    [InlineData("System7/System Folder/Extensions/Color Picker.res")]
    [InlineData("System7/System Folder/Extensions/DAL.res")]
    [InlineData("System7/System Folder/Extensions/File Sharing Extension.res")]
    [InlineData("System7/System Folder/Extensions/Find File Extension.res")]
    [InlineData("System7/System Folder/Extensions/Finder Help.res")]
    [InlineData("System7/System Folder/Extensions/Finder Scripting Extension.res")]
    [InlineData("System7/System Folder/Extensions/Foreign File Access.res")]
    [InlineData("System7/System Folder/Extensions/High Sierra File Access.res")]
    [InlineData("System7/System Folder/Extensions/ISO 9660 File Access.res")]
    [InlineData("System7/System Folder/Extensions/ImageWriter.res")]
    [InlineData("System7/System Folder/Extensions/LQ AppleTalk ImageWriter.res")]
    [InlineData("System7/System Folder/Extensions/LQ ImageWriter.res")]
    [InlineData("System7/System Folder/Extensions/LaserWriter 8.res")]
    [InlineData("System7/System Folder/Extensions/LaserWriter.res")]
    [InlineData("System7/System Folder/Extensions/Macintosh Guide.res")]
    [InlineData("System7/System Folder/Extensions/Network Extension.res")]
    [InlineData("System7/System Folder/Extensions/Networking Guide Additions.res")]
    [InlineData("System7/System Folder/Extensions/Personal LW LS.res")]
    [InlineData("System7/System Folder/Extensions/Personal LaserWriter SC.res")]
    [InlineData("System7/System Folder/Extensions/PrintMonitor.res")]
    [InlineData("System7/System Folder/Extensions/Printer Share.res")]
    [InlineData("System7/System Folder/Extensions/Shortcuts.res")]
    [InlineData("System7/System Folder/Extensions/SimpleText Guide.res")]
    [InlineData("System7/System Folder/Extensions/Sound_Monitors Guide Additions.res")]
    [InlineData("System7/System Folder/Extensions/StyleWriter 1200.res")]
    [InlineData("System7/System Folder/Extensions/Printer Descriptions/LaserWriter 16_600 PS Fax.res")]
    [InlineData("System7/System Folder/Extensions/Printer Descriptions/LaserWriter 16_600 PS-J.res")]
    [InlineData("System7/System Folder/Extensions/Printer Descriptions/LaserWriter 16_600 PS.res")]
    [InlineData("System7/System Folder/Extensions/Printer Descriptions/LaserWriter 4_600 PS.res")]
    [InlineData("System7/System Folder/Extensions/Printer Descriptions/LaserWriter Color 12_600 PS.res")]
    [InlineData("System7/System Folder/Extensions/Printer Descriptions/LaserWriter II NT.res")]
    [InlineData("System7/System Folder/Extensions/Printer Descriptions/LaserWriter II NTX v50.5.res")]
    [InlineData("System7/System Folder/Extensions/Printer Descriptions/LaserWriter II NTX v51.8.res")]
    [InlineData("System7/System Folder/Extensions/Printer Descriptions/LaserWriter II NTX-J v50.5.res")]
    [InlineData("System7/System Folder/Extensions/Printer Descriptions/LaserWriter II NTX.res")]
    [InlineData("System7/System Folder/Extensions/Printer Descriptions/LaserWriter IIf v2010.113.res")]
    [InlineData("System7/System Folder/Extensions/Printer Descriptions/LaserWriter IIf v2010.130.res")]
    [InlineData("System7/System Folder/Extensions/Printer Descriptions/LaserWriter IIg v2010.113.res")]
    [InlineData("System7/System Folder/Extensions/Printer Descriptions/LaserWriter IIg v2010.130.res")]
    [InlineData("System7/System Folder/Extensions/Printer Descriptions/LaserWriter Personal 320.res")]
    [InlineData("System7/System Folder/Extensions/Printer Descriptions/LaserWriter Personal NT.res")]
    [InlineData("System7/System Folder/Extensions/Printer Descriptions/LaserWriter Personal NTR.res")]
    [InlineData("System7/System Folder/Extensions/Printer Descriptions/LaserWriter Plus v38.0.res")]
    [InlineData("System7/System Folder/Extensions/Printer Descriptions/LaserWriter Plus v42.2.res")]
    [InlineData("System7/System Folder/Extensions/Printer Descriptions/LaserWriter Pro 400 v2011.110.res")]
    [InlineData("System7/System Folder/Extensions/Printer Descriptions/LaserWriter Pro 405 v2011.110.res")]
    [InlineData("System7/System Folder/Extensions/Printer Descriptions/LaserWriter Pro 600 v2010.130.res")]
    [InlineData("System7/System Folder/Extensions/Printer Descriptions/LaserWriter Pro 630 v2010.130.res")]
    [InlineData("System7/System Folder/Extensions/Printer Descriptions/LaserWriter Pro 810.res")]
    [InlineData("System7/System Folder/Extensions/Printer Descriptions/LaserWriter Pro 810f.res")]
    [InlineData("System7/System Folder/Extensions/Printer Descriptions/LaserWriter Select 360.res")]
    [InlineData("System7/System Folder/Extensions/Printer Descriptions/LaserWriter Select 360f.res")]
    [InlineData("System7/System Folder/Extensions/Printer Descriptions/LaserWriter Select 610.res")]
    [InlineData("System7/System Folder/Extensions/Printer Descriptions/LaserWriter.res")]
    [InlineData("System7/System Folder/Extensions/Scripting Additions/AGStart.res")]
    [InlineData("System7/System Folder/Extensions/Scripting Additions/Beep.res")]
    [InlineData("System7/System Folder/Extensions/Scripting Additions/Choose Application.res")]
    [InlineData("System7/System Folder/Extensions/Scripting Additions/Choose File.res")]
    [InlineData("System7/System Folder/Extensions/Scripting Additions/Current Date.res")]
    [InlineData("System7/System Folder/Extensions/Scripting Additions/Display Dialog.res")]
    [InlineData("System7/System Folder/Extensions/Scripting Additions/File Commands.res")]
    [InlineData("System7/System Folder/Extensions/Scripting Additions/Load Script.res")]
    [InlineData("System7/System Folder/Extensions/Scripting Additions/MonitorDepth.res")]
    [InlineData("System7/System Folder/Extensions/Scripting Additions/New File.res")]
    [InlineData("System7/System Folder/Extensions/Scripting Additions/Numerics.res")]
    [InlineData("System7/System Folder/Extensions/Scripting Additions/Read_Write Commands.res")]
    [InlineData("System7/System Folder/Extensions/Scripting Additions/Run Script.res")]
    [InlineData("System7/System Folder/Extensions/Scripting Additions/Scripting Components.res")]
    [InlineData("System7/System Folder/Extensions/Scripting Additions/Set Volume.res")]
    [InlineData("System7/System Folder/Extensions/Scripting Additions/Store Script.res")]
    [InlineData("System7/System Folder/Extensions/Scripting Additions/String Commands.res")]
    [InlineData("System7/System Folder/Extensions/Scripting Additions/Time to GMT.res")]
    [InlineData("System7/System Folder/Extensions/Scripting Additions/Dialects/English Dialect.res")]
    [InlineData("System7/System Folder/Fonts/Chicago.res")]
    [InlineData("System7/System Folder/Fonts/Courier.res")]
    [InlineData("System7/System Folder/Fonts/Geneva.res")]
    [InlineData("System7/System Folder/Fonts/Helvetica.res")]
    [InlineData("System7/System Folder/Fonts/Monaco.res")]
    [InlineData("System7/System Folder/Fonts/New York.res")]
    [InlineData("System7/System Folder/Fonts/Palatino.res")]
    [InlineData("System7/System Folder/Fonts/Symbol.res")]
    [InlineData("System7/System Folder/Fonts/Times.res")]
    [InlineData("System7/System Folder/Launcher Items/Script Editor.res")]
    [InlineData("System7/System Folder/Launcher Items/SimpleText.res")]
    [InlineData("System7/System Folder/Preferences/Apple Menu Options Prefs.res")]
    [InlineData("System7/System Folder/Preferences/AppleTalk Preferences.res")]
    [InlineData("System7/System Folder/Preferences/DAL Preferences.res")]
    [InlineData("System7/System Folder/Preferences/Date & Time Preferences.res")]
    [InlineData("System7/System Folder/Preferences/Desktop Pattern Prefs.res")]
    [InlineData("System7/System Folder/Preferences/Finder Preferences.res")]
    [InlineData("System7/System Folder/Preferences/General Controls Prefs.res")]
    [InlineData("System7/System Folder/Preferences/Macintosh Easy Open Preferences.res")]
    [InlineData("System7/System Folder/Preferences/PC Exchange Preferences.res")]
    [InlineData("System7/System Folder/Preferences/StuffIt Expander Preferences.res")]
    [InlineData("System7/System Folder/Preferences/WindowShade Preferences.res")]
    public void Ctor_Stream(string fileName)
    {
        // Skip tests with filenames containing characters invalid.
        if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
        {
            return; // Skip this test
        }

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
        // Create Output directory if it doesn't exist (needed for Windows)
        Directory.CreateDirectory("Output");

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
                case ResourceForkType.AINI:
                case ResourceForkType.cmtb:
                case ResourceForkType.Code1:
                case ResourceForkType.Code2:
                case ResourceForkType.Code3:
                case ResourceForkType.Code4:
                case ResourceForkType.Code5:
                case ResourceForkType.ControlDefinitionFunction:
                case ResourceForkType.ControlDeviceFunction:
                case ResourceForkType.Driver1:
                case ResourceForkType.Driver2:
                case ResourceForkType.ListDefinitionFunction:
                case ResourceForkType.Boot1:
                case ResourceForkType.Boot2:
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
                case ResourceForkType.ScriptCode:
                case ResourceForkType.ColorPickerCode:
                case ResourceForkType.GesaltManagerDefinition:
                case ResourceForkType.shal:
                case ResourceForkType.vdig:
                case ResourceForkType.dimg:
                case ResourceForkType.citt:
                case ResourceForkType.epch:
                case ResourceForkType.gcko:
                case ResourceForkType.ndlc:
                case ResourceForkType.ndrv:
                case ResourceForkType.nift:
                case ResourceForkType.nitt:
                case ResourceForkType.nlib:
                case ResourceForkType.nsnd:
                case ResourceForkType.nsrd:
                case ResourceForkType.SystemDecompressor:
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
                        if (strListResource.Attributes.HasFlag(ResourceAttributes.Compressed))
                        {
                            Debug.WriteLine($"  String List {strListResource} is compressed; skipping.");
                            continue;
                        }

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
                    Debug.WriteLine("vers Resources:");
                    foreach (var versResource in type.Value)
                    {
                        var versData = fork.GetResourceData(versResource);
                        var versionRecord = new VersionRecord(versData);
                        Debug.WriteLine($"  Version {versResource}: {versionRecord.Major}.{versionRecord.Minor}");
                    }
                    break;

                case ResourceForkType.VersionString:
                    Debug.WriteLine("VERS Resources:");
                    foreach (var versStringResource in type.Value)
                    {
                        var versStringData = fork.GetResourceData(versStringResource);
                        var versionStringRecord = new VersionStringRecord(versStringData);
                        Debug.WriteLine($"  Version String {versStringResource}: Major={versionStringRecord.Major}, Minor={versionStringRecord.Minor}");
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
                        if (bndlResource.Attributes.HasFlag(ResourceAttributes.Compressed))
                        {
                            Debug.WriteLine($"  Bundle {bndlResource} is compressed; skipping.");
                            continue;
                        }

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
                        if (iconResource.Attributes.HasFlag(ResourceAttributes.Compressed))
                        {
                            Debug.WriteLine($"  Icon {iconResource} is compressed; skipping.");
                            continue;
                        }

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
                        Debug.WriteLine($"  Picture {pictResource}: OpcodeData Length={pictureRecord.OpcodeData.Length}");
                    }

                    break;

                case ResourceForkType.IconList:
                    Debug.WriteLine("IconList ('ICN#') Resources:");
                    foreach (var iconListResource in type.Value)
                    {
                        if (iconListResource.Attributes.HasFlag(ResourceAttributes.Compressed))
                        {
                            Debug.WriteLine($"  Icon List {iconListResource} is compressed; skipping.");
                            continue;
                        }

                        var iconListData = fork.GetResourceData(iconListResource);
                        var iconListRecord = new IconListRecord(iconListData);
                        Debug.WriteLine($"  Icon List {iconListResource}: IconCount={iconListRecord.Icons.Count}");
                        foreach (var icon in iconListRecord.Icons)
                        {
                            Debug.WriteLine($"    Icon: Data Length={icon.Length}");
                        }
                    }

                    break;

                case ResourceForkType.Cursor:
                    Debug.WriteLine("CURS Resources:");
                    foreach (var cursResource in type.Value)
                    {
                        if (cursResource.Attributes.HasFlag(ResourceAttributes.Compressed))
                        {
                            Debug.WriteLine($"  Cursor {cursResource} is compressed; skipping.");
                            continue;
                        }

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
                        if (ditlResource.Attributes.HasFlag(ResourceAttributes.Compressed))
                        {
                            Debug.WriteLine($"  Item List {ditlResource} is compressed; skipping.");
                            continue;
                        }

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
                        if (menuResource.Attributes.HasFlag(ResourceAttributes.Compressed))
                        {
                            Debug.WriteLine($"  Menu {menuResource} is compressed; skipping.");
                            continue;
                        }

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
                        if (patListResource.Attributes.HasFlag(ResourceAttributes.Compressed))
                        {
                            Debug.WriteLine($"  Pattern List {patListResource} is compressed; skipping.");
                            continue;
                        }

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

                case ResourceForkType.LargeIcon4Bit:
                    Debug.WriteLine("icl4 Resources:");
                    foreach (var icl4Resource in type.Value)
                    {
                        if (icl4Resource.Attributes.HasFlag(ResourceAttributes.Compressed))
                        {
                            Debug.WriteLine($"  4-bit Large Icon {icl4Resource} is compressed; skipping.");
                            continue;
                        }

                        var icl4Data = fork.GetResourceData(icl4Resource);
                        var icon4BitRecord = new LargeIcon4BitRecord(icl4Data);
                        Debug.WriteLine($"  4-bit Icon {icl4Resource}: IconData Length={icon4BitRecord.IconData.Length}");
                    }

                    break;

                case ResourceForkType.LargeIcon8Bit:
                    Debug.WriteLine("icl8 Resources:");
                    foreach (var icl8Resource in type.Value)
                    {
                        if (icl8Resource.Attributes.HasFlag(ResourceAttributes.Compressed))
                        {
                            Debug.WriteLine($"  8-bit Large Icon {icl8Resource} is compressed; skipping.");
                            continue;
                        }

                        var icl8Data = fork.GetResourceData(icl8Resource);
                        var icon8BitRecord = new LargeIcon8BitRecord(icl8Data);
                        Debug.WriteLine($"  8-bit Icon {icl8Resource}: IconData Length={icon8BitRecord.IconData.Length}");
                    }

                    break;

                case ResourceForkType.SmallIcon4Bit:
                    Debug.WriteLine("ics4 Resources:");
                    foreach (var ics4Resource in type.Value)
                    {
                        if (ics4Resource.Attributes.HasFlag(ResourceAttributes.Compressed))
                        {
                            Debug.WriteLine($"  Small 4-bit Icon {ics4Resource} is compressed; skipping.");
                            continue;
                        }

                        var ics4Data = fork.GetResourceData(ics4Resource);
                        var smallIcon4BitRecord = new SmallIcon4BitRecord(ics4Data);
                        Debug.WriteLine($"  Small 4-bit Icon {ics4Resource}: IconData Length={smallIcon4BitRecord.IconData.Length}");
                    }

                    break;

                case ResourceForkType.SmallIcon8Bit:
                    Debug.WriteLine("ics8 Resources:");
                    foreach (var ics8Resource in type.Value)
                    {
                        if (ics8Resource.Attributes.HasFlag(ResourceAttributes.Compressed))
                        {
                            Debug.WriteLine($"  Small 8-bit Icon {ics8Resource} is compressed; skipping.");
                            continue;
                        }

                        var ics8Data = fork.GetResourceData(ics8Resource);
                        var smallIcon8BitRecord = new SmallIcon8BitRecord(ics8Data);
                        Debug.WriteLine($"  Small 8-bit Icon {ics8Resource}: IconData Length={smallIcon8BitRecord.IconData.Length}");
                    }

                    break;
                
                case ResourceForkType.SmallIconList:
                    Debug.WriteLine("icl# Resources:");
                    foreach (var iclResource in type.Value)
                    {
                        if (iclResource.Attributes.HasFlag(ResourceAttributes.Compressed))
                        {
                            Debug.WriteLine($"  Small Icon List {iclResource} is compressed; skipping.");
                            continue;
                        }

                        var iclData = fork.GetResourceData(iclResource);
                        var smallIconListRecord = new SmallIconListRecord(iclData);
                        Debug.WriteLine($"  Small Icon List {iclResource}: IconCount={smallIconListRecord.Icons.Count}");
                        foreach (var icon in smallIconListRecord.Icons)
                        {
                            Debug.WriteLine($"    Icon: Data Length={icon.Length}");
                        }
                    }

                    break;

                case ResourceForkType.ColorIcon:
                    Debug.WriteLine("cicn Resources:");
                    foreach (var cicnResource in type.Value)
                    {
                        if (cicnResource.Attributes.HasFlag(ResourceAttributes.Compressed))
                        {
                            Debug.WriteLine($"  Color Icon {cicnResource} is compressed; skipping.");
                            continue;
                        }

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

                case ResourceForkType.CompressedSound:
                    Debug.WriteLine("CSND Resources:");
                    foreach (var csndResource in type.Value)
                    {
                        var csndData = fork.GetResourceData(csndResource);
                        var compressedSoundRecord = new CompressedSoundRecord(csndData);
                        Debug.WriteLine($"  Compressed Sound {csndResource}: SampleType={compressedSoundRecord.SampleType}, DecompressedSize={compressedSoundRecord.DecompressedSize}, DecompressedData Length={compressedSoundRecord.DecompressedData.Length}");
                    }

                    break;

                case ResourceForkType.Palette:
                    Debug.WriteLine("pltt Resources:");
                    foreach (var plttResource in type.Value)
                    {
                        if (plttResource.Attributes.HasFlag(ResourceAttributes.Compressed))
                        {
                            Debug.WriteLine($"  Palette {plttResource} is compressed; skipping.");
                            continue;
                        }

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
                        if (ppatResource.Attributes.HasFlag(ResourceAttributes.Compressed))
                        {
                            Debug.WriteLine($"  Pixel Pattern {ppatResource} is compressed; skipping.");
                            continue;
                        }

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

                case ResourceForkType.CitiesList1:
                    Debug.WriteLine("CTY# Resources:");
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
                case ResourceForkType.SystemVersion3:
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

                case ResourceForkType.CacheTable1:
                case ResourceForkType.CacheTable2:
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
                case ResourceForkType.PreferencesFile:
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

                case ResourceForkType.Style:
                    Debug.WriteLine("styl Resources:");
                    foreach (var stylResource in type.Value)
                    {
                        if (stylResource.Attributes.HasFlag(ResourceAttributes.Compressed))
                        {
                            Debug.WriteLine($"  Style Record {stylResource} is compressed; skipping.");
                            continue;
                        }

                        var stylData = fork.GetResourceData(stylResource);
                        var styleRecord = new StyleRecord(stylData);
                        Debug.WriteLine($"  Style Record {stylResource}: NumberOfRuns={styleRecord.NumberOfRuns} ");
                        foreach (var run in styleRecord.StyleRuns)
                        {
                            Debug.WriteLine($"    Style Run: StartIndex={run.Offset}, FontID={run.FontResourceID}, Size={run.PointSize}, StyleFlags={run.Flags}, Color={run.Color}");
                        }
                    }

                    break;

                case ResourceForkType.ScriptingSize:
                    Debug.WriteLine("scsz Resources:");
                    foreach (var scszResource in type.Value)
                    {
                        var scszData = fork.GetResourceData(scszResource);
                        var scriptingSizeRecord = new ScriptingSizeRecord(scszData);
                        Debug.WriteLine($"  Scripting Size {scszResource}: Flags={scriptingSizeRecord.Flags} MinimumStackSize={scriptingSizeRecord.MinimumStackSize} PreferredStackSize={scriptingSizeRecord.PreferredStackSize} MaximumStackSize={scriptingSizeRecord.MaximumStackSize} MinimumHeapSize={scriptingSizeRecord.MinimumHeapSize} PreferredHeapSize={scriptingSizeRecord.PreferredHeapSize} MaximumHeapSize={scriptingSizeRecord.MaximumHeapSize}");
                    }

                    break;

                case ResourceForkType.ItemColorTable:
                    Debug.WriteLine("ictb Resources:");
                    foreach (var ictbResource in type.Value)
                    {
                        var ictbData = fork.GetResourceData(ictbResource);
                        var itemColorTableRecord = new ItemColorTableRecord(ictbData);
                        Debug.WriteLine($"  Item Color Table {ictbResource}: Entries Count={itemColorTableRecord.Entries.Count}");
                        for (int i = 0; i < itemColorTableRecord.Entries.Count; i++)
                        {
                            var entry = itemColorTableRecord.Entries[i];
                            Debug.WriteLine($"    Entry {i}: Data={entry.Data}, Offset={entry.Offset}");
                        }
                    }

                    break;

                case ResourceForkType.Psap:
                    Debug.WriteLine("psap Resources:");
                    foreach (var psapResource in type.Value)
                    {
                        var psapData = fork.GetResourceData(psapResource);
                        var psapRecord = new PsapRecord(psapData);
                        Debug.WriteLine($"  Psap Record {psapResource}: Value={psapRecord.Value}");
                    }

                    break;

                case ResourceForkType.ScriptingAdditionSize:
                    Debug.WriteLine("osiz Resources:");
                    foreach (var osizResource in type.Value)
                    {
                        var osizData = fork.GetResourceData(osizResource);
                        var scriptingAdditionSizeRecord = new ScriptingAdditionSizeRecord(osizData);
                        Debug.WriteLine($"  Scripting Addition Size {osizResource}: Flags={scriptingAdditionSizeRecord.Flags}");
                    }

                    break;

                case ResourceForkType.Alias:
                    Debug.WriteLine("alis Resources:");
                    foreach (var alisResource in type.Value)
                    {
                        var alisData = fork.GetResourceData(alisResource);
                        var aliasRecord = new AliasRecord(alisData);
                        Debug.WriteLine($"  Alias Record {alisResource}: Type=\"{aliasRecord.Type}\", DataLength={aliasRecord.DataLength} bytes, PrivateData Length={aliasRecord.PrivateData.Length} bytes");
                    }

                    break;


                case ResourceForkType.FinderIconMap:
                    Debug.WriteLine("fmap Resources:");
                    foreach (var fmapResource in type.Value)
                    {
                        var fmapData = fork.GetResourceData(fmapResource);
                        var finderIconMapRecord = new FinderIconMapRecord(fmapData);
                        Debug.WriteLine($"  Finder Icon Map {fmapResource}: Entries Count={finderIconMapRecord.Entries.Count}");
                        foreach (var entry in finderIconMapRecord.Entries)
                        {
                            Debug.WriteLine($"    Finder Icon Map Entry: Type={entry.Type}, StandardFileIconResourceID={entry.StandardFileIconResourceID}, FinderIconResourceID={entry.FinderIconResourceID}");
                        }
                    }

                    break;

                case ResourceForkType.FolderList:
                    Debug.WriteLine("fld# Resources:");
                    foreach (var fldResource in type.Value)
                    {
                        var fldData = fork.GetResourceData(fldResource);
                        var folderListRecord = new FolderListRecord(fldData);
                        Debug.WriteLine($"  Folder List {fldResource}: Folders Count={folderListRecord.Folders.Count}");
                        foreach (var folder in folderListRecord.Folders)
                        {
                            Debug.WriteLine($"    Folder: Type={folder.Type}, Name=\"{folder.Name}\"");
                        }
                    }

                    break;

                case ResourceForkType.Component:
                    Debug.WriteLine("thng Resources:");
                    foreach (var thngResource in type.Value)
                    {
                        var thngData = fork.GetResourceData(thngResource);
                        var componentRecord = new ComponentRecord(thngData);
                        Debug.WriteLine($"  Component {thngResource}: NameResourceType={componentRecord.NameResource.Type}, NameResourceID={componentRecord.NameResource.ResourceID}, CodeResourceType={componentRecord.CodeResource.Type}, CodeResourceID={componentRecord.CodeResource.ResourceID}");
                    }

                    break;

                case ResourceForkType.ComponentList:
                    Debug.WriteLine("thn# Resources:");
                    foreach (var thnnResource in type.Value)
                    {
                        var thnnData = fork.GetResourceData(thnnResource);
                        var componentListRecord = new ComponentListRecord(thnnData);
                        Debug.WriteLine($"  Component List {thnnResource}: Components Count={componentListRecord.Components.Count}");
                        foreach (var component in componentListRecord.Components)
                        {
                            Debug.WriteLine($"    Component List Entry: Type={component.Type}, ResourceID={component.ResourceID}");
                        }
                    }

                    break;

                case ResourceForkType.HelpRectangles:
                    Debug.WriteLine("hrct Resources:");
                    foreach (var hrctResource in type.Value)
                    {
                        var hrctData = fork.GetResourceData(hrctResource);
                        var helpRectanglesRecord = new HelpRectanglesRecord(hrctData);
                        Debug.WriteLine($"  Help Rectangles {hrctResource}: Version={helpRectanglesRecord.Version}, Options={helpRectanglesRecord.Options}, BalloonDefinitionFunctionResourceID={helpRectanglesRecord.BalloonDefinitionFunctionResourceID}, VariationCode={helpRectanglesRecord.VariationCode}, HotRectangleComponentCount={helpRectanglesRecord.HotRectangleComponentCount}");
                    }

                    break;

                case ResourceForkType.ISO9660VolumeDescriptor:
                    Debug.WriteLine("NRVD Resources:");
                    foreach (var nrvdResource in type.Value)
                    {
                        var nrvdData = fork.GetResourceData(nrvdResource);
                        var iso9660VolumeDescriptorRecord = new ISO9660VolumeDescriptorRecord(nrvdData);
                        Debug.WriteLine($"  ISO9660 Volume Descriptor {nrvdResource}: VolumeFlags={iso9660VolumeDescriptorRecord.VolumeFlags}, EscapeSequences={iso9660VolumeDescriptorRecord.EscapeSequences}");
                    }

                    break;

                case ResourceForkType.Bitmap1:
                case ResourceForkType.Bitmap2:
                    Debug.WriteLine("BMAP Resources:");
                    foreach (var bmapResource in type.Value)
                    {
                        var bmapData = fork.GetResourceData(bmapResource);
                        var bitmapRecord = new BitmapRecord(bmapData);
                        Debug.WriteLine($"  Bitmap {bmapResource}: Data Length={bitmapRecord.Data.Length} bytes");
                    }

                    break;

                case ResourceForkType.Open:
                    Debug.WriteLine("open Resources:");
                    foreach (var openResource in type.Value)
                    {
                        var openData = fork.GetResourceData(openResource);
                        var openRecord = new OpenRecord(openData);
                        Debug.WriteLine($"  Open Record {openResource}: ApplicationSignature=\"{openRecord.ApplicationSignature}\"");
                        for (int i = 0; i < openRecord.FileTypes.Count; i++)
                        {
                            Debug.WriteLine($"    File Type {i}: \"{openRecord.FileTypes[i]}\"");
                        }
                    }

                    break;

                case ResourceForkType.Kind:
                    Debug.WriteLine("kind Resources:");
                    foreach (var kindResource in type.Value)
                    {
                        if (kindResource.Attributes.HasFlag(ResourceAttributes.Compressed))
                        {
                            Debug.WriteLine($"  Kind Record {kindResource} is compressed; skipping.");
                            continue;
                        }

                        var kindData = fork.GetResourceData(kindResource);
                        var kindRecord = new KindRecord(kindData);
                        Debug.WriteLine($"  Kind Record {kindResource}: ApplicationSignature=\"{kindRecord.ApplicationSignature}\"");
                    }

                    break;

                case ResourceForkType.SoundLookupTable:
                    Debug.WriteLine("slut Resources:");
                    foreach (var slutResource in type.Value)
                    {
                        var slutData = fork.GetResourceData(slutResource);
                        var soundLookupTableRecord = new SoundLookupTableRecord(slutData);
                        Debug.WriteLine($"  Sound Lookup Table {slutResource}: Entries Count={soundLookupTableRecord.Entries.Count}");
                        foreach (var entry in soundLookupTableRecord.Entries)
                        {
                            Debug.WriteLine($"    Sound Lookup Table Entry: Type={entry.Type}, ResourceID={entry.ResourceID}");
                        }
                    }

                    break;

                case ResourceForkType.ErrorString:
                    Debug.WriteLine("errs Resources:");
                    foreach (var errsResource in type.Value)
                    {
                        var errsData = fork.GetResourceData(errsResource);
                        var errorStringsRecord = new ErrorStringsRecord(errsData);
                        Debug.WriteLine($"  Error Strings {errsResource}: Strings Count={errorStringsRecord.ErrorStrings.Count}");
                        for (int i = 0; i < errorStringsRecord.ErrorStrings.Count; i++)
                        {
                            Debug.WriteLine($"    String {i}: \"{errorStringsRecord.ErrorStrings[i]}\"");
                        }
                    }

                    break;
    
                case ResourceForkType.MiniIconList:
                    Debug.WriteLine("icm# Resources:");
                    foreach (var icmResource in type.Value)
                    {
                        var icmData = fork.GetResourceData(icmResource);
                        var miniIconListRecord = new MiniIconListRecord(icmData);
                        Debug.WriteLine($"  Mini Icon List {icmResource}: Icons Count={miniIconListRecord.Icons.Count}");
                        for (int i = 0; i < miniIconListRecord.Icons.Count; i++)
                        {
                            var icon = miniIconListRecord.Icons[i];
                            Debug.WriteLine($"    Icon {i}: Data Length={icon.Length} bytes");
                        }
                    }

                    break;

                case ResourceForkType.MiniIcon4Bit:
                    Debug.WriteLine("icm4 Resources:");
                    foreach (var icm4Resource in type.Value)
                    {
                        var icm4Data = fork.GetResourceData(icm4Resource);
                        var miniIcon4BitRecord = new MiniIcon4BitRecord(icm4Data);
                        Debug.WriteLine($"  Mini Icon 4-Bit {icm4Resource}: Data Length={miniIcon4BitRecord.IconData.Length} bytes");
                    }

                    break;

                case ResourceForkType.MiniIcon8Bit: 
                    Debug.WriteLine("icm8 Resources:");
                    foreach (var icm8Resource in type.Value)
                    {
                        var icm8Data = fork.GetResourceData(icm8Resource);
                        var miniIcon8BitRecord = new MiniIcon8BitRecord(icm8Data);
                        Debug.WriteLine($"  Mini Icon 8-Bit {icm8Resource}: Data Length={miniIcon8BitRecord.IconData.Length} bytes");
                    }

                    break;

                case ResourceForkType.KeyboardColorIconList:
                    Debug.WriteLine("kcs# Resources:");
                    foreach (var kcsResource in type.Value)
                    {
                        var kcsData = fork.GetResourceData(kcsResource);
                        var keyboardColorIconListRecord = new KeyboardColorIconListRecord(kcsData);
                        Debug.WriteLine($"  Keyboard Color Icon List {kcsResource}: Icons Count={keyboardColorIconListRecord.Icons.Count}");
                        for (int i = 0; i < keyboardColorIconListRecord.Icons.Count; i++)
                        {
                            var icon = keyboardColorIconListRecord.Icons[i];
                            Debug.WriteLine($"    Icon {i}: Data Length={icon.Length} bytes");
                        }
                    }

                    break;

                case ResourceForkType.KeyboardColorIcon4Bit:
                    Debug.WriteLine("kcs4 Resources:");
                    foreach (var kcs4Resource in type.Value)
                    {
                        var kcs4Data = fork.GetResourceData(kcs4Resource);
                        var keyboardColorIcon4BitRecord = new KeyboardColorIcon4BitRecord(kcs4Data);
                        Debug.WriteLine($"  Keyboard Color Icon 4-Bit {kcs4Resource}: Data Length={keyboardColorIcon4BitRecord.IconData.Length} bytes");
                    }

                    break;

                case ResourceForkType.KeyboardColorIcon8Bit:
                    Debug.WriteLine("kcs8 Resources:");
                    foreach (var kcs8Resource in type.Value)
                    {
                        var kcs8Data = fork.GetResourceData(kcs8Resource);
                        var keyboardColorIcon8BitRecord = new KeyboardColorIcon8BitRecord(kcs8Data);
                        Debug.WriteLine($"  Keyboard Color Icon 8-Bit {kcs8Resource}: Data Length={keyboardColorIcon8BitRecord.IconData.Length} bytes");
                    }

                    break;

                case ResourceForkType.TextStyle:
                    Debug.WriteLine("TxSt Resources:");
                    foreach (var txstResource in type.Value)
                    {
                        var txstData = fork.GetResourceData(txstResource);
                        var textStyleRecord = new TextStyleRecord(txstData);
                        Debug.WriteLine($"  Text Style Record {txstResource}: FontStyle={textStyleRecord.FontStyle}, FontSize={textStyleRecord.FontSize}, FontName=\"{textStyleRecord.FontName}\"");
                    }

                    break;

                case ResourceForkType.TitleList2:
                    Debug.WriteLine("Tlst Resources:");
                    foreach (var tlstResource in type.Value)
                    {
                        var tlstData = fork.GetResourceData(tlstResource);
                        var titleListRecord = new TitleListRecord(tlstData);
                        Debug.WriteLine($"  Title List Record {tlstResource}: Types Count={titleListRecord.Types.Count}");
                        for (int i = 0; i < titleListRecord.Types.Count; i++)
                        {
                            Debug.WriteLine($"    Type {i}: \"{titleListRecord.Types[i]}\"");
                        }
                    }

                    break;

                case ResourceForkType.DatabaseResultHandlers:
                    Debug.WriteLine("rtt# Resources:");
                    foreach (var rttResource in type.Value)
                    {
                        var rttData = fork.GetResourceData(rttResource);
                        var databaseResultHandlersRecord = new DatabaseResultHandlersRecord(rttData);
                        Debug.WriteLine($"  Database Result Handlers Record {rttResource}: NumberOfHandlers={databaseResultHandlersRecord.NumberOfHandlers}");
                        for (int i = 0; i < databaseResultHandlersRecord.Handlers.Count; i++)
                        {
                            var handler = databaseResultHandlersRecord.Handlers[i];
                            Debug.WriteLine($"    Handler {i}: ProcedureResourceID={handler.ProcedureResourceID}, NumberOfTypes={handler.NumberOfTypes}");
                            for (int j = 0; j < handler.Types.Count; j++)
                            {
                                Debug.WriteLine($"      Type {j}: \"{handler.Types[j]}\"");
                            }
                        }
                    }

                    break;

                case ResourceForkType.KeyboardMappings:
                    Debug.WriteLine("itlk Resources:");
                    foreach (var itlkResource in type.Value)
                    {
                        var itlkData = fork.GetResourceData(itlkResource);
                        var keyboardMappingsRecord = new KeyboardMappingsRecord(itlkData);
                        Debug.WriteLine($"  Keyboard Mapping Record {itlkResource}: Mappings Count={keyboardMappingsRecord.Mappings.Count}");
                        for (int i = 0; i < keyboardMappingsRecord.Mappings.Count; i++)
                        {
                            var mapping = keyboardMappingsRecord.Mappings[i];
                            Debug.WriteLine($"    Key Mapping {i}: KeyboardType={mapping.KeyboardType}, OldModifiers={mapping.OldModifiers}, OldKeyCode={mapping.OldKeyCode}, MaskModifiers={mapping.MaskModifiers}, MaskKeyCode={mapping.MaskKeyCode}, NewModifiers={mapping.NewModifiers}, NewKeyCode={mapping.NewKeyCode}");
                        }
                    }

                    break;

                case ResourceForkType.FontList:
                    Debug.WriteLine("resf Resources:");
                    foreach (var resfResource in type.Value)
                    {
                        var resfData = fork.GetResourceData(resfResource);
                        var fontListRecord = new FontListRecord(resfData);
                        Debug.WriteLine($"  Font List Record {resfResource}: FontFamilies Count={fontListRecord.FontFamilies.Count}");
                        for (int i = 0; i < fontListRecord.FontFamilies.Count; i++)
                        {
                            Debug.WriteLine($"    Font Family {i}: {fontListRecord.FontFamilies[i].Name}, Fonts Count={fontListRecord.FontFamilies[i].Fonts.Count}");
                            for (int j = 0; j < fontListRecord.FontFamilies[i].Fonts.Count; j++)
                            {
                                var font = fontListRecord.FontFamilies[i].Fonts[j];
                                Debug.WriteLine($"      Font {j}: PointSize=\"{font.PointSize}\", StyleFlags={font.StyleFlags}");
                            }
                        }
                    }

                    break;

                case ResourceForkType.MacintoshModels:
                    Debug.WriteLine("audt Resources:");
                    foreach (var audtResource in type.Value)
                    {
                        var audtData = fork.GetResourceData(audtResource);
                        var macintoshModelsRecord = new MacintoshModels(audtData);
                        Debug.WriteLine($"  Macintosh Models Record {audtResource}: Models Count={macintoshModelsRecord.Models.Count}");
                        for (int i = 0; i < macintoshModelsRecord.Models.Count; i++)
                        {
                            var model = macintoshModelsRecord.Models[i];
                            Debug.WriteLine($"    Model {i}: ModelType={model.ModelType}, InstallationStatus={model.InstallationStatus}");
                        }
                    }

                    break;

                case ResourceForkType.OutlineFont:
                    Debug.WriteLine("sfnt Resources:");
                    foreach (var sfntResource in type.Value)
                    {
                        var sfntData = fork.GetResourceData(sfntResource);
                        var outlineFontRecord = new OutlineFontRecord(sfntData);
                        Debug.WriteLine($"  Outline Font Record {sfntResource}: FontDirectory TableCount={outlineFontRecord.FontDirectory.NumberOfTables}");
                        for (int i = 0; i < outlineFontRecord.FontDirectory.Tables.Count; i++)
                        {
                            var table = outlineFontRecord.FontDirectory.Tables[i];
                            Debug.WriteLine($"    Font Table {i}: TagName=\"{table.TagName}\", Checksum={table.Checksum}, Offset={table.Offset}, Length={table.Length}");
                        }
                    }

                    break;

                case ResourceForkType.ApplicationList:
                    Debug.WriteLine("APPL Resources:");
                    foreach (var applResource in type.Value)
                    {
                        var applData = fork.GetResourceData(applResource);
                        var applicationListRecord = new ApplicationListRecord(applData);
                        Debug.WriteLine($"  Application List Record {applResource}: Applications Count={applicationListRecord.Entries.Count}");
                        for (int i = 0; i < applicationListRecord.Entries.Count; i++)
                        {
                            var entry = applicationListRecord.Entries[i];
                            Debug.WriteLine($"    Application Entry {i}: Creator=\"{entry.Creator}\", DirectoryID={entry.DirectoryID}, Application=\"{entry.Application}\"");
                        }
                    }

                    break;

                case ResourceForkType.TypeList:
                    Debug.WriteLine("TYP# Resources:");
                    foreach (var typResource in type.Value)
                    {
                        var typData = fork.GetResourceData(typResource);
                        var typeListRecord = new TypeList(typData);
                        Debug.WriteLine($"  Type List Record {typResource}: NumberOfTypes={typeListRecord.NumberOfTypes}");
                        for (int i = 0; i < typeListRecord.Types.Count; i++)
                        {
                            Debug.WriteLine($"    Type {i}: \"{typeListRecord.Types[i]}\"");
                        }
                    }

                    break;

                case ResourceForkType.StringWordCount:
                    Debug.WriteLine("WSTR Resources:");
                    foreach (var wstrResource in type.Value)
                    {
                        var wstrData = fork.GetResourceData(wstrResource);
                        var stringWordCountRecord = new StringWordCountRecord(wstrData);
                        Debug.WriteLine($"  String Word Count Record {wstrResource}: Value={stringWordCountRecord.Value}");
                    }

                    break;

                case ResourceForkType.MacroMakerInformation1:
                case ResourceForkType.MacroMakerInformation2:
                case ResourceForkType.MacroMakerInformation3:
                case ResourceForkType.LaserPrepROM:
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
                case ResourceForkType.OpenScriptingArchitectureExtension: // "System 7/System Folder/Extensions..."
                //case ResourceForkType.MacProgrammersWorkshopShellResource: // 
                case "dhMP": // "System 7/System Folder/Extensions/Networking Guide Additions.res"
                case "NOTI": // "System 7/System Folder/Apple Extras/Applie Script?/More Automated Tasks/About More Automated Tasks.res"
                case "extm": // "System7/Apple Extras/AppleCD Audio Player/AppleCD Audio Player Guide.res"
                case "pwRB": // "System7/Apple Extras/AppleCD Audio Player/AppleCD Audio Player Guide.res"
                case ResourceForkType.WindowPosition: // "System7/Apple Extras/AppleScript?/Automated Tasks/Add Alias to Apple Menu.res"
                case "pcPR": // "System7/Apple Extras/AppleCD Audio Player/AppleCD Audio Player Guide.res"
                case "pcRB": // "System7/Apple Extras/AppleCD Audio Player/AppleCD Audio Player Guide.res"
                case "acTB": // "System7/Apple Extras/AppleCD Audio Player/AppleCD Audio Player Guide.res"
                case "acSU": // "System7/Apple Extras/AppleCD Audio Player/AppleCD Audio Player Guide.res"
                case "acVI": // "System7/Apple Extras/AppleCD Audio Player/AppleCD Audio Player Guide.res"
                case "spsh": // "System7/Apple Extras/AppleScript?/Automated Tasks/Add Alias to Apple Menu.res"
                case ResourceForkType.Script: // "System7/Apple Extras/AppleScript?/More Automated Tasks/Synchronize Folders.res"
                case ResourceForkType.Droplet: // "System7/Apple Extras/AppleScript?/Automated Tasks/Add Alias to Apple Menu.res"
                case ResourceForkType.Applet: // "System7/Apple Extras/AppleScript?/More Automated Tasks/Change Monitor to B&W.res"
                case ResourceForkType.MacProgrammersWorkshopShellResource: // "System7/System Folder/Extensions/Printer Descriptions/LaserWriter Pro 810f.res"
                case "nbrs": // "System7/System Folder/Control Panels/Numbers.res"
                case ResourceForkType.DatabaseExtension: // "System7/System Folder/Extensions/DAL.res"
                case "extE": // "System7/System Folder/Extensions/ EM Extension.res"
                case "aete": // "System7/System Folder/Extensions/Scripting Additions/Read_Write Commands.res"
                case "shdo": // "System7/System Folder/Apple Menu Items/? Shut Down.res"
                case "actb": // "System7/System Folder/Apple Menu Items/Find File.res"
                case "lpch": // "System7/System Folder/Control Panels/PC Exchange.res"
                case "CL1N": // "System7/System Folder/Extensions/DAL.res"
                case "cfmt": // "System7/System Folder/Control Panels/Numbers.res"
                case "CUST": // "System7/System Folder/Control Panels/PC Exchange.res"
                case "DICL": // "System7/System Folder/Extensions/StyleWriter 1200.res"
                case "hjcr": // "System7/System Folder/Extensions/Finder Scripting Extension.res"
                case "ffEx": // "System7/System Folder/Extensions/Find File Extension.res"
                case "dnrp": // "System7/System Folder/MacTCP DNR.res"
                case "ufox": // "System7/System Folder/Extensions/Foreign File Access.res"
                case "calc": // "System7/System Folder/Apple Menu Items/Calculator.res"
                case "ugcf": // "System7/System Folder/Control Panels/Users & Groups.res"
                case "KINI": // "System7/System Folder/Control Panels/Mouse.res"
                case "C&Jp": // "System7/System Folder/Control Panels/Apple Menu Options.res"
                case "aclk": // "System7/System Folder/Apple Menu Items/Alarm Clock.res"
                case "DALF": // "System7/System Folder/Extensions/DAL.res"
                case "MDVR": // "System7/System Folder/Control Panels/PC Exchange.res"
                case "rspc": // "System7/System Folder/Extensions/StyleWriter 1200.res"
                case "dnsl": // "System7/System Folder/MacTCP DNR.res"
                case "STSZ": // "System7/System Folder/Extensions/Foreign File Access.res"
                case "CDJR": // "System7/System Folder/Control Panels/Apple Menu Options.res"
                case "dast": // "System7/System Folder/Apple Menu Items/Alarm Clock.res"
                case "aucd": // "System7/Apple Extras/AppleCD Audio Player/AppleCD Audio Player.res"
                case "flbs": // "System7/System Folder/Control Panels/Labels.res"
                case "DATA": // "System7/System Folder/Apple Menu Items/Note Pad.res"
                case "tecd": // "System7/System Folder/Control Panels/Text.res"
                case "STRI": // "System7/System Folder/Extensions/LaserWriter 8.res"
                case "pkft": // "System7/System Folder/Apple Menu Items/Key Caps.res"
                case "shrt": // "System7/System Folder/Preferences/Desktop Pattern Prefs.res"
                case "gmra": // "System7/System Folder/Control Panels/Macintosh Easy Open.res"
                case "fval": // "System7/System Folder/Preferences/Finder Preferences.res"
                case "dald": // "System7/System Folder/Extensions/DAL.res"
                case "fSCR": // "System7/System Folder/Control Panels/PC Exchange.res"
                case "GAMA": // "System7/System Folder/Extensions/StyleWriter 1200.res"
                case "fsiz": // "System7/System Folder/Extensions/Finder Scripting Extension.res"
                case "CODX": // "System7/System Folder/Extensions/Foreign File Access.res"
                case "wtab": // "System7/System Folder/Extensions/Color Picker.res"
                case "CLRS": // "System7/Apple Extras/AppleCD Audio Player/AppleCD Audio Player.res"
                case "WSPR": // "System7/System Folder/Control Panels/WindowShade.res"
                case "FTIN": // "System7/System Folder/Extensions/ISO 9660 File Access.res"
                case "sPAT": // "System7/System Folder/Extensions/LaserWriter 8.res"
                case "kcwn": // "System7/System Folder/Apple Menu Items/Key Caps.res"
                case "WPOS": // "System7/System Folder/Preferences/Desktop Pattern Prefs.res"
                case "chzr": // "System7/System Folder/Apple Menu Items/Chooser.res"
                case "ZERO": // "System7/System Folder/Control Panels/Desktop Patterns.res"
                case "iemt": // "System7/System Folder/Control Panels/Macintosh Easy Open.res"
                case "sgci": // "System7/System Folder/Preferences/Finder Preferences.res"
                case "fsmn": // "System7/System Folder/Control Panels/File Sharing Monitor.res"
                case "cryp": // "System7/System Folder/Extensions/File Sharing Extension.res"
                case "dMap": // "System7/System Folder/Preferences/PC Exchange Preferences.res"
                case "Smrt": // "System7/System Folder/Preferences/General Controls Prefs.res"
                case "puzz": // "System7/System Folder/Apple Menu Items/Puzzle.res"
                case "ffpt": // "System7/System Folder/Control Panels/General Controls.res"
                case "adev": // "System7/System Folder/Control Panels/Network.res"
                case "Dbdl": // "System7/System Folder/Extensions/Scripting Additions/Dialects/English Dialect.res"
                case "BUTT": // "System7/System Folder/Extensions/Apple CD-ROM.res"
                case "vm  ": // "System7/System Folder/System.res"
                case "caps": // "System7/System Folder/Extensions/Caps Lock.res"
                case "QLfy": // "System7/System Folder/Extensions/Sound_Monitors Guide Additions.res"
                case "TSEP": // "System7/System Folder/Extensions/Audio CD Access.res"
                case "sndP": // "System7/System Folder/Control Panels/Sound.res"
                case "fvew": // "System7/System Folder/Extensions/Network Extension.res"
                case "8INI": // "System7/System Folder/Control Panels/Extensions Manager.res"
                case "LWLS": // "System7/System Folder/Extensions/Personal LW LS.res"
                case "CDAA": // "System7/System Folder/Preferences/Apple Menu Options Prefs.res"
                case "cxlt": // "System7/System Folder/Preferences/Macintosh Easy Open Preferences.res"
                case "neta": // "System7/System Folder/Preferences/AppleTalk Preferences.res"
                case "easy": // "System7/System Folder/Control Panels/Easy Access.res"
                case "mash": // "System7/System Folder/Control Panels/Launcher.res"
                case "date": // "System7/System Folder/Control Panels/Date & Time.res"
                case "ascd": // "System7/System Folder/Extensions/AppleShare.res"
                case "mnrm": // "System7/System Folder/Control Panels/Memory.res"
                case "sdsk": // "System7/System Folder/Control Panels/Startup Disk.res"
                case "init": // "System7/System Folder/Extensions/AppleScript?.res"
                case "dflg": // "System7/System Folder/Extensions/DAL.res"
                case "SCIC": // "System7/System Folder/Control Panels/PC Exchange.res"
                case "cfrg": // "System7/System Folder/Extensions/StyleWriter 1200.res"
                case "fsfx": // "System7/System Folder/Extensions/Finder Scripting Extension.res"
                case "ASPT": // "System7/System Folder/Extensions/Foreign File Access.res"
                case "DREL": // "System7/System Folder/Apple Menu Items/Note Pad.res"
                case "WS2!": // "System7/System Folder/Control Panels/WindowShade.res"
                case "NCOD": // "System7/System Folder/Extensions/ISO 9660 File Access.res"
                case "cInf": // "System7/System Folder/Extensions/LaserWriter 8.res"
                case "keyc": // "System7/System Folder/Apple Menu Items/Key Caps.res"
                case "xsig": // "System7/System Folder/Control Panels/Macintosh Easy Open.res"
                case "RDP ": // "System7/System Folder/Extensions/File Sharing Extension.res"
                case "pdat": // "System7/System Folder/Apple Menu Items/Puzzle.res"
                case "misc": // "System7/System Folder/Control Panels/General Controls.res"
                case "atdv": // "System7/System Folder/Control Panels/Network.res"
                case "LCTL": // "System7/System Folder/Extensions/Scripting Additions/Dialects/English Dialect.res"
                case "VBLF": // "System7/System Folder/Extensions/Apple CD-ROM.res"
                case "LOG ": // "System7/System Folder/Extensions/StyleWriter 1200.res"
                case "erm#": // "System7/System Folder/Extensions/Finder Scripting Extension.res"
                case "npad": // "System7/System Folder/Apple Menu Items/Note Pad.res"
                case "PRFS": // "System7/System Folder/Extensions/LaserWriter 8.res"
                case "kpdr": // "System7/System Folder/Apple Menu Items/Key Caps.res"
                case "BUFF": // "System7/System Folder/Extensions/File Sharing Extension.res"
                case "wsta": // "System7/System Folder/Apple Menu Items/Puzzle.res"
                case "ASer": // "System7/System Folder/Extensions/Scripting Additions/Dialects/English Dialect.res"
                case "rgb ": // "System7/System Folder/System.res"
                case "QLca": // "System7/System Folder/Extensions/Sound_Monitors Guide Additions.res"
                case "jigz": // "System7/System Folder/Apple Menu Items/Jigsaw Puzzle.res"
                case "DLGX": // "System7/System Folder/Apple Menu Items/Find File.res"
                case "NPLY": // "System7/System Folder/Extensions/StyleWriter 1200.res"
                case "cpmg": // "System7/System Folder/Extensions/Color Picker.res"
                case "wrct": // "System7/System Folder/Apple Menu Items/Note Pad.res"
                case "PS  ": // "System7/System Folder/Extensions/LaserWriter 8.res"
                case "kbit": // "System7/System Folder/Apple Menu Items/Key Caps.res"
                case "hhgg": // "System7/System Folder/Extensions/File Sharing Extension.res"
                case "ASms": // "System7/System Folder/Extensions/Scripting Additions/Dialects/English Dialect.res"
                case "lstr": // "System7/System Folder/System.res"
                case "QLcn": // "System7/System Folder/Extensions/Sound_Monitors Guide Additions.res"
                case "sflg": // "System7/System Folder/Extensions/Network Extension.res"
                case "empr": // "System7/System Folder/Control Panels/Extensions Manager.res"
                case "FMap": // "System7/System Folder/Preferences/Apple Menu Options Prefs.res"
                case "mirt": // "System7/System Folder/Preferences/Macintosh Easy Open Preferences.res"
                case "ea_p": // "System7/System Folder/Control Panels/Easy Access.res"
                case "fnin": // "System7/System Folder/Control Panels/Launcher.res"
                case "pts#": // "System7/System Folder/Extensions/AppleShare.res"
                case "mmry": // "System7/System Folder/Control Panels/Memory.res"
                case "fAni": // "System7/System Folder/Finder.res"
                case "TMP#": // "System7/Apple Extras/AppleScript?/Script Editor.res"
                case "scbk": // "System7/System Folder/Apple Menu Items/Scrapbook.res"
                case "clip": // "System7/System Folder/Extensions/Clipping Extension.res"
                case "PROC": // "System7/System Folder/Extensions/AppleScript?.res"
                case "ipln": // "System7/System Folder/Control Panels/MacTCP.res"
                case "pbc2": // "System7/System Folder/Extensions/Printer Share.res"
                case "Prfs": // "System7/System Folder/Extensions/Apple Guide.res"
                case "pcod": // "System7/System Folder/Control Panels/PC Exchange.res"
                case "ntvl": // "System7/System Folder/Extensions/StyleWriter 1200.res"
                case "npdt": // "System7/System Folder/Apple Menu Items/Note Pad.res"
                case "Desc": // "System7/System Folder/Extensions/LaserWriter 8.res"
                case "meo ": // "System7/System Folder/Control Panels/Macintosh Easy Open.res"
                case "TERM": // "System7/System Folder/Extensions/Scripting Additions/Dialects/English Dialect.res"
                case "fdmn": // "System7/System Folder/Extensions/Network Extension.res"
                case "ESet": // "System7/System Folder/Control Panels/Extensions Manager.res"
                case "prat": // "System7/System Folder/Preferences/Macintosh Easy Open Preferences.res"
                case "MINH": // "System7/System Folder/Extensions/AppleShare.res"
                case "tbl ": // "System7/System Folder/Control Panels/Memory.res"
                case "Stl#": // "System7/Apple Extras/AppleScript?/Script Editor.res"
                case "sbkt": // "System7/System Folder/Apple Menu Items/Scrapbook.res"
                case "Colr": // "System7/System Folder/Apple Menu Items/Stickies.res"
                case "iplp": // "System7/System Folder/Control Panels/MacTCP.res"
                case "#ckt": // "System7/System Folder/Extensions/Printer Share.res"
                case "crtr": // "System7/System Folder/Extensions/Apple Guide.res"
                case "nstp": // "System7/System Folder/Control Panels/Sharing Setup.res"
                case "FMAT": // "System7/System Folder/Apple Menu Items/Find File.res"
                case "ptbl": // "System7/System Folder/Control Panels/PC Exchange.res"
                case "PSEL": // "System7/System Folder/Extensions/StyleWriter 1200.res"
                case "dFlt": // "System7/System Folder/Extensions/LaserWriter 8.res"
                case "ASdl": // "System7/System Folder/Extensions/Scripting Additions/Dialects/English Dialect.res"
                case "user": // "System7/System Folder/Extensions/Network Extension.res"
                case "FTyp": // "System7/System Folder/Control Panels/Extensions Manager.res" 
                case "DFLG": // "System7/System Folder/Extensions/AppleShare.res"
                case "gest": // "System7/System Folder/Finder.res"
                case "Keyf": // "System7/Apple Extras/AppleScript?/Script Editor.res"
                case "FEIF": // "System7/System Folder/Apple Menu Items/Stickies.res"
                case "ctrp": // "System7/System Folder/Control Panels/MacTCP.res"
                case "WLDa": // "System7/System Folder/Extensions/Apple Guide.res"
                case "pack": // "System7/System Folder/Control Panels/PC Exchange.res"
                case "CHOS": // "System7/System Folder/Extensions/StyleWriter 1200.res"
                case "symE": // "System7/System Folder/Extensions/LaserWriter 8.res"
                case "YACC": // "System7/System Folder/Extensions/Scripting Additions/Dialects/English Dialect.res"
                case "lmgr": // "System7/System Folder/System.res"
                case "iNIT": // "System7/System Folder/Control Panels/Extensions Manager.res"
                case "DATB": // "System7/System Folder/Extensions/AppleShare.res"
                case "hcsl": // "System7/System Folder/Finder.res"
                case "SIZe": // "System7/Apple Extras/AppleScript?/Script Editor.res"
                case "Pref": // "System7/System Folder/Apple Menu Items/Stickies.res"
                case "arps": // "System7/System Folder/Control Panels/MacTCP.res"
                case "WLDp": // "System7/System Folder/Extensions/Apple Guide.res"
                case "STRX": // "System7/System Folder/Apple Menu Items/Find File.res"
                case "Cach": // "System7/System Folder/Control Panels/PC Exchange.res"
                case "dblo": // "System7/System Folder/Extensions/StyleWriter 1200.res"
                case "pTyp": // "System7/System Folder/Extensions/LaserWriter 8.res"
                case "YCTL": // "System7/System Folder/Extensions/Scripting Additions/Dialects/English Dialect.res"
                case "pixs": // "System7/System Folder/System.res"
                case "opts": // "System7/System Folder/Control Panels/Extensions Manager.res"
                case "DATC": // "System7/System Folder/Extensions/AppleShare.res"
                case "sirz": // "System7/System Folder/Finder.res"
                case "HFdR": // "System7/Apple Extras/AppleScript?/Script Editor.res"
                case "BDat": // "System7/System Folder/Apple Menu Items/Stickies.res"
                case "ztcp": // "System7/System Folder/Control Panels/MacTCP.res"
                case "RSTY": // "System7/System Folder/Extensions/Apple Guide.res"
                case "IgCn": // "System7/System Folder/Apple Menu Items/Find File.res"
                case "FICO": // "System7/System Folder/Control Panels/PC Exchange.res"
                case "strm": // "System7/System Folder/Extensions/StyleWriter 1200.res"
                case "fSub": // "System7/System Folder/Extensions/LaserWriter 8.res"
                case "ASuc": // "System7/System Folder/Extensions/Scripting Additions/Dialects/English Dialect.res"
                case "fmnu": // "System7/System Folder/Finder.res"
                case "PtSz": // "System7/System Folder/Apple Menu Items/Stickies.res"
                case "dsrt": // "System7/System Folder/Extensions/Apple Guide.res"
                case "fndf": // "System7/System Folder/Apple Menu Items/Find File.res"
                case "dosa": // "System7/System Folder/Control Panels/PC Exchange.res"
                case "purg": // "System7/System Folder/Extensions/StyleWriter 1200.res"
                case "fSav": // "System7/System Folder/Extensions/LaserWriter 8.res"
                case "ASpc": // "System7/System Folder/Extensions/Scripting Additions/Dialects/English Dialect.res"
                case "lefT": // "System7/System Folder/System.res"
                case "XREF": // "System7/System Folder/Apple Menu Items/Stickies.res"
                case "acSD": // "System7/System Folder/Extensions/Apple Guide.res"
                case "DOSK": // "System7/System Folder/Control Panels/PC Exchange.res"
                case "ENGC": // "System7/System Folder/Extensions/StyleWriter 1200.res"
                case "FDer": // "System7/System Folder/Extensions/LaserWriter 8.res"
                case "AStl": // "System7/System Folder/Extensions/Scripting Additions/Dialects/English Dialect.res"
                case "pmap": // "System7/System Folder/System.res"
                case "notz": // "System7/System Folder/Apple Menu Items/Stickies.res"
                case "paEV": // "System7/System Folder/Extensions/Apple Guide.res"
                case "Defl": // "System7/System Folder/Control Panels/PC Exchange.res"
                case "HTSC": // "System7/System Folder/Extensions/StyleWriter 1200.res"
                case "encM": // "System7/System Folder/Extensions/LaserWriter 8.res"
                case "Deft": // "System7/System Folder/Control Panels/PC Exchange.res"
                case "MEMB": // "System7/System Folder/Extensions/StyleWriter 1200.res"
                case "aFax": // "System7/System Folder/Extensions/LaserWriter 8.res"
                case "AStn": // "System7/System Folder/Extensions/Scripting Additions/Dialects/English Dialect.res"
                case "ppUA": // "System7/System Folder/Extensions/Apple Guide.res"
                case "PFSL": // "System7/System Folder/Extensions/StyleWriter 1200.res"
                case "sgmr": // "System7/System Folder/Extensions/StyleWriter 1200.res"
                case "pcCI": // "System7/System Folder/Extensions/Apple Guide.res"
                case "BADA": // "System7/System Folder/Extensions/LaserWriter 8.res"
                case "adio": // "System7/System Folder/System.res"
                case "pcTX": // "System7/System Folder/Extensions/Apple Guide.res"
                case "MCMD": // "System7/SimpleText.res"
                case "pcTA": // "System7/System Folder/Extensions/Apple Guide.res"
                case "pxCA": // "System7/System Folder/Extensions/Apple Guide.res"
                case "pqMS": // "System7/System Folder/Extensions/Apple Guide.res"
                case "pqCN": // "System7/System Folder/Extensions/Apple Guide.res"
                case "acPT": // "System7/System Folder/Extensions/Apple Guide.res"
                case "acCC": // "System7/System Folder/Extensions/Apple Guide.res"
                case "Stem": // "System7/System Folder/Extensions/Apple Guide.res"
                case "reno": // "System7/System Folder/Extensions/Apple Guide.res"
                case "idnt": // "System7/System Folder/Extensions/Apple Guide.res"
                case "cmfl": // "System7/System Folder/Extensions/Apple Guide.res"
                case "RGN ": // "System7/System Folder/Extensions/Apple Guide.res"
                case "chlk": // "System7/System Folder/Extensions/Apple Guide.res"
                case "FLEX": // "System7/System Folder/Extensions/Scripting Additions/Dialects/English Dialect.res"
                case "aeut": // "System7/System Folder/Extensions/Scripting Additions/Dialects/English Dialect.res"
                case "tstr": // "System7/System Folder/Extensions/Scripting Additions/Dialects/English Dialect.res"
                case "per2": // "System7/System Folder/Extensions/Scripting Additions/Dialects/English Dialect.res"
                case "ASe1": // "System7/System Folder/Extensions/Scripting Additions/Dialects/English Dialect.res"
                case "ASe2": // "System7/System Folder/Extensions/Scripting Additions/Dialects/English Dialect.res"
                case "ASnf": // "System7/System Folder/Extensions/Scripting Additions/Dialects/English Dialect.res"
                case "ASnp": // "System7/System Folder/Extensions/Scripting Additions/Dialects/English Dialect.res"
                case "UNIT": // "System7/System Folder/Extensions/LaserWriter 8.res"
                case "bfdf": // "System7/System Folder/System.res"
                case "lMac": // "System7/System Folder/Extensions/Apple Guide.res"
                case "vgrd": // "System7/System Folder/Extensions/LaserWriter 8.res"
                case "dskp": // "System7/System Folder/Control Panels/Desktop Patterns.res"
                case "bfim": // "System7/System Folder/System.res"
                case "bNDL": // "System7/Apple Extras/AppleScript?/Script Editor.res"
                case "ascr": // "System7/System Folder/Extensions/AppleScript?.res"
                case "strt": // "System7/System Folder/Extensions/Apple Guide.res"
                case ResourceForkType.CitiesList2: // "System7/System Folder/Control Panels/Map.res"
                case "fwst": // "System7/System Folder/Control Panels/Map.res"
                case "bmgr": // "System7/System Folder/System.res"
                case "bpch": // "System7/System Folder/System.res"
                case "cmu!": // "System7/System Folder/System.res"
                case "ecf2": // "System7/System Folder/System.res"
                case "ecfg": // "System7/System Folder/System.res"
                case "enet": // "System7/System Folder/System.res"
                case "fovr": // "System7/System Folder/System.res"
                case "gbly": // "System7/System Folder/System.res"
                case "gnld": // "System7/System Folder/System.res"
                case "gnth": // "System7/System Folder/System.res"
                case "gnxt": // "System7/System Folder/System.res"
                case "gpch": // "System7/System Folder/System.res"
                case "gtbl": // "System7/System Folder/System.res"
                case "gusd": // "System7/System Folder/System.res"
                case "lodr": // "System7/System Folder/System.res"
                case "ntrb": // "System7/System Folder/System.res"
                case "pslt": // "System7/System Folder/System.res"
                case "sfvr": // "System7/System Folder/System.res"
                case "sift": // "System7/System Folder/System.res"
                case "timd": // "System7/System Folder/System.res"
                case "vdpm": // "System7/System Folder/System.res"
                case "wart": // "System7/System Folder/System.res"
                case "picb": // "System7/System Folder/System.res"
                case "SERD": // "System7/System Folder/System.res"
                case "accl": // "System7/System Folder/System.res"
                case "atlk": // "System7/System Folder/System.res"
                case "ctb ": // "System7/System Folder/System.res"
                case "dbag": // "System7/System Folder/System.res"
                case "dspf": // "System7/System Folder/System.res"
                case "flst": // "System7/System Folder/System.res"
                case "i2c ": // "System7/System Folder/System.res"
                case "iopc": // "System7/System Folder/System.res"
                case "ltlk": // "System7/System Folder/System.res"
                case "otdr": // "System7/System Folder/System.res"
                case "otlm": // "System7/System Folder/System.res"
                case "ppcc": // "System7/System Folder/System.res"
                case "ppci": // "System7/System Folder/System.res"
                case "prob": // "System7/System Folder/System.res"
                case "dbex": // "System7/System Folder/System.res"
                case "indl": // "System7/System Folder/System.res"
                case "inpm": // "System7/System Folder/System.res"
                case "itlm": // "System7/System Folder/System.res"
                case "acfg": // "System7/System Folder/System.res"
                case "cpud": // "System7/System Folder/System.res"
                case "pg&e": // "System7/System Folder/System.res"
                case "DDid": // "System7/System Folder/Extensions/AppleScript?.res"
                case "aspf": // "System7/System Folder/Extensions/AppleScript?.res"
                case "ashi": // "System7/System Folder/Extensions/AppleScript?.res"
                case "asha": // "System7/System Folder/Extensions/AppleScript?.res"
                case "ascn": // "System7/System Folder/Extensions/AppleScript?.res"
                case "asc2": // "System7/System Folder/Extensions/AppleScript?.res"
                case "asc3": // "System7/System Folder/Extensions/AppleScript?.res"
                case "asc4": // "System7/System Folder/Extensions/AppleScript?.res"
                case "asc5": // "System7/System Folder/Extensions/AppleScript?.res"
                case "asc6": // "System7/System Folder/Extensions/AppleScript?.res"
                case "ccod": // "System7/System Folder/Extensions/AppleScript?.res"
                case "DEI ": // "System7/System Folder/Extensions/AppleScript?.res"
                case "fREF": // "System7/Apple Extras/AppleScript?/Script Editor.res"
                case "bnDL": // "System7/Apple Extras/AppleScript?/Script Editor.res"
                case "REXP": // "System7/Apple Extras/AppleScript?/Script Editor.res"
                case "SCSz": // "System7/Apple Extras/AppleScript?/Script Editor.res"
                case "ToyS": // "System7/Apple Extras/AppleScript?/Script Editor.res"
                case "CMNU": // "System7/Apple Extras/AppleScript?/Script Editor.res"
                case "RLIS": // "System7/Apple Extras/AppleScript?/Script Editor.res"
                case "aedt": // "System7/Apple Extras/AppleScript?/Script Editor.res"
                case "Lang": // "System7/Apple Extras/AppleScript?/Script Editor.res"
                case "WITL": // "System7/Apple Extras/AppleScript?/Script Editor.res"
                case "hVew": // "System7/Apple Extras/AppleScript?/Script Editor.res"
                case "time": // "System7/System Folder/Control Panels/Date & Time.res"
                case "ZSYS": // "System3.2/HD 20 Test.res"
                case "HDTS": // "System3.2/HD 20 Test.res"
                case ResourceForkType.ConstantsList: // "System3.2/System Folder/System.res"
                case ResourceForkType.TitleList1: // "System3.2/System Folder/System.res"
                case "uins": // "System5/Desktop.res"
                case "PSHX": // "System3.0/System Folder/LaserWriter.res"
                case "BITD": // "System3.1.1/System Folder/Control Scene.res"
                case "RONY": // "System3.1.1/Mousing Around.res"
                case "MMTE": // "System3.1.1/System Foldr/OVWT.res"
                case "CARY": // "System3.1.1/Desktop.res"
                case "JSHL": // "System3.1.1/Desktop.res"
                case "MMVW": // "System3.1.1/Desktop.res"
                    // Unknown.
                    break;

                default:
                    throw new NotImplementedException($"Resource type '{type.Key}' not implemented for detailed dumping.");
            }
        }
    }
}
