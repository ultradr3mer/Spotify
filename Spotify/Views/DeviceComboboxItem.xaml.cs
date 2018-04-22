namespace Spotify.Views
{
  using Windows.UI.Xaml.Controls;

  /// <summary>
  /// The device combo box item.
  /// </summary>
  /// <seealso cref="Windows.UI.Xaml.Controls.UserControl" />
  /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
  /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
  public sealed partial class DeviceComboBoxItem : UserControl
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="DeviceComboBoxItem"/> class.
    /// </summary>
    public DeviceComboBoxItem()
    {
      this.InitializeComponent();
    }

    #endregion
  }
}