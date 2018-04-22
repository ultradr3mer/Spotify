namespace Spotify.Views
{
  using Windows.UI.Xaml.Controls;

  /// <summary>
  /// The playlist track item.
  /// </summary>
  /// <seealso cref="Windows.UI.Xaml.Controls.UserControl" />
  /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
  /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
  public sealed partial class PlaylistTrackItem : UserControl
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="PlaylistTrackItem"/> class.
    /// </summary>
    public PlaylistTrackItem()
    {
      this.InitializeComponent();
    }

    #endregion
  }
}