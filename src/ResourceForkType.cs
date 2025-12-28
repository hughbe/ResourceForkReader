namespace ResourceForkReader;

/// <summary>
/// Defines constants for common resource fork types.
/// </summary>
public static class ResourceForkType
{
    /// <summary>
    /// The 'ADBS' resource type for ADB drivers.
    /// </summary>
    public const string ADBDriver = "ADBS";
    
    /// <summary>
    /// The 'acur' resource type for animated cursors.
    /// </summary>
    public const string AnimatedCursor = "acur";

    /// <summary>
    /// The 'ALRT' resource type for alert boxes.
    /// </summary>
    public const string AlertBox = "ALRT";

    /// <summary>
    /// The 'boot' resource type for boot resources.
    /// </summary>
    public const string Boot = "boot";

    /// <summary>
    /// The 'BNDL' resource type for bundles.
    /// </summary>
    public const string Bundle = "BNDL";

    /// <summary>
    /// The 'CTAB' resource type for cache tables.
    /// </summary>
    public const string CacheTable = "CTAB";

    /// <summary>
    /// The 'CTY#' resource type for cities list.
    /// </summary>
    public const string CitiesList = "CTY#";

    /// <summary>
    /// The 'CODE' resource type for code resources.
    /// </summary>
    public const string Code = "CODE";

    /// <summary>
    /// The 'PDEF' resource type for code to drive printers.
    /// </summary>
    public const string CodeToDrivePrinters = "PDEF";

    /// <summary>
    /// The 'clut' resource type for color lookup tables.
    /// </summary>
    public const string ColorLookupTable = "clut";

    /// <summary>
    /// The 'cicn' resource type for color icons.
    /// </summary>
    public const string ColorIcon = "cicn";

    /// <summary>
    /// The 'CNTL' resource type for controls.
    /// </summary>
    public const string Control = "CNTL";

    /// <summary>
    /// The 'CDEF' resource type for control definition functions.
    /// </summary>
    public const string ControlDefinitionFunction = "CDEF";

    /// <summary>
    /// The 'cdev' resource type for control device functions.
    /// </summary>
    public const string ControlDeviceFunction = "cdev";

    /// <summary>
    /// The 'cctb' resource type for control color tables.
    /// </summary>
    public const string ControlColorLookupTable = "cctb";

    /// <summary>
    /// The 'CURS' resource type for cursors.
    /// </summary>
    public const string Cursor = "CURS";

    /// <summary>
    /// The 'DLOG' resource type for dialog boxes.
    /// </summary>
    public const string DialogBox = "DLOG";

    /// <summary>
    /// The 'dctb' resource type for dialog color lookup tables.
    /// </summary>
    public const string DialogColorLookupTable = "dctb";

    /// <summary>
    /// The 'DRVR' resource type for drivers.
    /// </summary>
    public const string Driver = "DRVR";

    /// <summary>
    /// The 'FCMT' resource type for file comments.
    /// </summary>
    public const string FileComment = "FCMT";

    /// <summary>
    /// The 'FOBJ' resource type for file objects.
    /// </summary>
    public const string FileObject = "FOBJ";

    /// <summary>
    /// The 'FREF' resource type for file references.
    /// </summary>
    public const string FileReference = "FREF";

    /// <summary>
    /// The 'FONT' resource type for fonts.
    /// </summary>
    public const string Font = "FONT";

    /// <summary>
    /// The 'FNUM' resource type for new fonts.
    /// </summary>
    public const string FontNew = "NFNT";

    /// <summary>
    /// The 'FOND' resource type for font families.
    /// </summary>
    public const string FontFamily = "FOND";

    /// <summary>
    /// The 'finf' resource type for font information.
    /// </summary>
    public const string FontInformation = "finf";

    /// <summary>
    /// The 'FMTR' resource type for formatting code.
    /// </summary>
    public const string FormattingCode = "FMTR";

    /// <summary>
    /// The 'FKEY' resource type for function key code.
    /// </summary>
    public const string FunctionKeyCode = "FKEY";

    /// <summary>
    /// The 'hwin' resource type for help windows.
    /// </summary>
    public const string HelpWindow = "hwin";

    /// <summary>
    /// The 'ICON' resource type for icons.
    /// </summary>
    public const string Icon = "ICON";

    /// <summary>
    /// The 'ICN#' resource type for icon lists.
    /// </summary>
    public const string IconList = "ICN#";

    /// <summary>
    /// The 'DITL' resource type for dialog or alert box item lists.
    /// </summary>
    public const string ItemList = "DITL";

    /// <summary>
    /// The 'itlc' resource type for international configuration resources.
    /// </summary>
    public const string InternationalConfiguration = "itlc";

    /// <summary>
    /// The 'itl0' resource type for international formatting information.
    /// </summary>
    public const string InternationalFormattingInformation = "itl0";

    /// <summary>
    /// The 'itl1' resource type for international date formatting information.
    /// </summary>
    public const string InternationalDateFormattingInformation = "itl1";

    /// <summary>
    /// The 'INTL' resource type for international resources (obsolete).
    /// </summary>
    public const string InternationalResource = "INTL";

    /// <summary>
    /// The 'itlb' resource type for international script bundles.
    /// </summary>
    public const string InternationalScriptBundle = "itlb";

    /// <summary>
    /// The 'itl2' resource type for international string comparison package hooks.
    /// </summary>
    public const string InternationalStringComparisonPackageHooks = "itl2";

    /// <summary>
    /// The 'itl4' resource type for international tokenize resources.
    /// </summary>
    public const string InternationalTokenize = "itl4";

    /// <summary>
    /// The 'KEYC' resource type for software keyboard layouts.
    /// </summary>
    public const string KeyboardLayout = "KEYC";

    /// <summary>
    /// The 'KMAP' resource type for keyboard mapping.
    /// </summary>
    public const string KeyboardMapping = "KMAP";

    /// <summary>
    /// The 'KSWP' resource type for keyboard swapping.
    /// </summary>
    public const string KeyboardSwapping = "KSWP";

    /// <summary>
    /// The 'icl4' resource type for large 4 bit icons.
    /// </summary>
    public const string LargeIcon4bit = "icl4";

    /// <summary>
    /// The 'icl8' resource type for large 8 bit icons.
    /// </summary>
    public const string LargeIcon8bit = "icl8";

    /// <summary>
    /// The 'LAYO' resource type for layout resources.
    /// </summary>
    public const string Layout = "LAYO";

    /// <summary>
    /// The 'LDEF' resource type for list definition functions.
    /// </summary>
    public const string ListDefinitionFunction = "LDEF";

    /// <summary>
    /// The 'mach' resource type for machine records.
    /// </summary>
    public const string Machine = "mach";

    /// <summary>
    /// The 'mcod' resource type for MacroMaker information.
    /// </summary>
    public const string MacroMakerInformation1 = "mcod";

    /// <summary>
    /// The 'mdct' resource type for MacroMaker information.
    /// </summary>
    public const string MacroMakerInformation2 = "mdct";

    /// <summary>
    /// The 'minf' resource type for MacroMaker information.
    /// </summary>
    public const string MacroMakerInformation3 = "minf";

    /// <summary>
    /// The 'mitq' resource type for make inverse table queue sizes.
    /// </summary>
    public const string MakeInverseTableQueueSizes = "mitq";

    /// <summary>
    /// The 'MDEF' resource type for menu definition functions.
    /// </summary>
    public const string MenuDefinitionFunction = "MDEF";

    /// <summary>
    /// The 'MBDF' resource type for menu bar definition functions.
    /// </summary>
    public const string MenuBarDefinitionFunction = "MBDF";

    /// <summary>
    /// The 'MENU' resource type for menus.
    /// </summary>
    public const string Menu = "MENU";

    /// <summary>
    /// The 'MBAR' resource type for menu bars.
    /// </summary>
    public const string MenuBar = "MBAR";

    /// <summary>
    /// The 'mntr' resource type for monitor resources.
    /// </summary>
    public const string MonitorCode = "mntr";

    /// <summary>
    /// The 'mcky' resource type for mouse tracking resources.
    /// </summary>
    public const string MouseTracking = "mcky";

    /// <summary>
    /// The 'PACK' resource type for packages.
    /// </summary>
    public const string Package = "PACK";

    /// <summary>
    /// The 'pltt' resource type for palettes.
    /// </summary>
    public const string Palette = "pltt";

    /// <summary>
    /// The 'PAT ' resource type for QuickDraw patterns.
    /// </summary>
    public const string Pattern = "PAT ";

    /// <summary>
    /// The 'PAT#' resource type for QuickDraw pattern lists.
    /// </summary>
    public const string PatternList = "PAT#";

    /// <summary>
    /// The 'KCAP' resource type for physical keyboard descriptions.
    /// </summary>
    public const string PhysicalKeyboardDescription = "KCAP";

    /// <summary>
    /// The 'PICT' resource type for pictures.
    /// </summary>
    public const string Picture = "PICT";

    /// <summary>
    /// The 'ppat' resource type for pixel patterns.
    /// </summary>
    public const string PixelPattern = "ppat";

    /// <summary>
    /// The 'ppt#' resource type for pixel pattern lists.
    /// </summary>
    public const string PixelPatternList = "ppt#";

    /// <summary>
    /// The 'PREF' resource type for preferences.
    /// </summary>
    public const string Preferences = "PREF";

    /// <summary>
    /// The 'PREC' resource type for print records.
    /// </summary>
    public const string PrintRecord = "PREC";

    /// <summary>
    /// The 'PAPA' resource type for printer access protocol addresses.
    /// </summary>
    public const string PrinterAccessProtocolAddress = "PAPA";

    /// <summary>
    /// The 'POST' resource type for PostScript resources.
    /// </summary>
    public const string PostScript = "POST";

    /// <summary>
    /// The 'proc' resource type for procedure code.
    /// </summary>
    public const string ProcCode = "proc";

    /// <summary>
    /// The 'RECT' resource type for rectangle resources.
    /// </summary>
    public const string Rectangle = "RECT";

    /// <summary>
    /// The 'nrct' resource type for rectangle positions list.
    /// </summary>
    public const string RectanglePositionsList = "nrct";

    /// <summary>
    /// The 'CACH' resource type for RAM cache control code.
    /// </summary>
    public const string RAMCacheControlCode = "CACH";

    /// <summary>
    /// The 'FRSV' resource type for ROM fonts.
    /// </summary>
    public const string ROMFonts = "FRSV";

    /// <summary>
    /// The 'ROvr' resource type for ROM override resources.
    /// </summary>
    public const string ROMOverride = "ROvr";

    /// <summary>
    /// The 'ROv#' resource type for ROM override list resources.
    /// </summary>
    public const string ROMOverrideList = "ROv#";

    /// <summary>
    /// The 'PTCH' resource type for ROM patches.
    /// </summary>
    public const string ROMPatchCode = "PTCH";

    /// <summary>
    /// The 'SIZE' resource type for size resources.
    /// </summary>
    public const string Size = "SIZE";

    /// <summary>
    /// The 'icl4' resource type for large 4 bit icons.
    /// </summary>
    public const string SmallIcon4bit = "ics4";

    /// <summary>
    /// The 'icl8' resource type for large 8 bit icons.
    /// </summary>
    public const string SmallIcon8bit = "ics8";

    /// <summary>
    /// The 'ics#' resource type for small icon lists.
    /// </summary>
    public const string SmallIconList = "ics#";

    /// <summary>
    /// The 'DSAT' resource type for startup alerts code.
    /// </summary>
    public const string StartupAlertsCode = "DSAT";

    /// <summary>
    /// The 'STR ' resource type for string resources.
    /// </summary>
    public const string String = "STR ";

    /// <summary>
    /// The 'STR#' resource type for string list resources.
    /// </summary>
    public const string StringList = "STR#";

    /// <summary>
    /// The 'KCHR' resource type for software keyboard layouts.
    /// </summary>
    public const string SoftwareKeyboardLayout = "KCHR";

    /// <summary>
    /// The 'snd ' resource type for sound resources.
    /// </summary>
    public const string Sound = "snd ";

    /// <summary>
    /// The 'snth' resource type for synthesizer resources.
    /// </summary>
    public const string Synthesizer = "snth";

    /// <summary>
    /// The 'INIT' resource type for initialization resources.
    /// </summary>
    public const string SystemExtension = "INIT";

    /// <summary>
    /// The 'SICN' resource type for icons.
    /// </summary>
    public const string SystemIcon = "SICN";

    /// <summary>
    /// The 'TEXT' resource type for text resources.
    /// </summary>
    public const string Text = "TEXT";

    /// <summary>
    /// The 'VERS' resource type for version resources.
    /// </summary>
    public const string Version = "VERS";

    /// <summary>
    /// The 'card' resource type for video card resources.
    /// </summary>
    public const string VideoCard = "card";

    /// <summary>
    /// The 'WIND' resource type for windows.
    /// </summary>
    public const string Window = "WIND";

    /// <summary>
    /// The 'wctb' resource type for window color tables.
    /// </summary>
    public const string WindowColorTable = "wctb";

    /// <summary>
    /// The 'WDEF' resource type for window definition functions.
    /// </summary>
    public const string WindowDefinitionFunction = "WDEF";
}
