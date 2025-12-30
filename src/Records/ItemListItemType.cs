namespace ResourceForkReader.Records;

/// <summary>
/// The type of item in an item list.
/// </summary>
public enum ItemListItemType : byte
{
    /// <summary>
    /// User item.
    /// </summary>
    UserItem = 0,

    /// <summary>
    /// Help item.
    /// </summary>
    Help = 1,

    /// <summary>
    /// Button item.
    /// </summary>
    Button = 4,

    /// <summary>
    /// Checkbox item.
    /// </summary>
    CheckBox = 5,

    /// <summary>
    /// Radio button item.
    /// </summary>
    RadioButton = 6,

    /// <summary>
    /// Icon button item.
    /// </summary>
    IconButton = 7,

    /// <summary>
    /// Static text item.
    /// </summary>
    StaticText = 8,

    /// <summary>
    /// Editable text item.
    /// </summary>
    EditText = 16,

    /// <summary>
    /// Icon item.
    /// </summary>
    Icon = 32,

    /// <summary>
    /// Picture item.
    /// </summary>
    Picture = 64
}
