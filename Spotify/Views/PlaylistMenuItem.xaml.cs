namespace Spotify.Views
{
  using Spotify.ViewModels;

  using Windows.UI.Xaml.Controls;
  using Windows.UI.Xaml.Input;

  /// <summary>
  /// The playlist menu item.
  /// </summary>
  /// <seealso cref="Windows.UI.Xaml.Controls.NavigationViewItem" />
  /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
  /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
  public sealed partial class PlaylistMenuItem : NavigationViewItem
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="PlaylistMenuItem"/> class.
    /// </summary>
    public PlaylistMenuItem()
    {
      this.InitializeComponent();
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the view model.
    /// </summary>
    private PlaylistMenuItemViewModel ViewModel
    {
      get { return (PlaylistMenuItemViewModel)this.DataContext; }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Called when this playlist menu item was tapped.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="TappedRoutedEventArgs"/> instance containing the event data.</param>
    private void PlaylistMenuItemOnTapped(object sender, TappedRoutedEventArgs e)
    {
      this.ViewModel.ActivatePlaylist();
    }

    #endregion
  }
}