namespace Spotify.Views
{
  using Microsoft.Practices.Prism.Mvvm;

  using Windows.ApplicationModel;
  using Windows.UI.Xaml.Controls;

  /// <summary>The playlist page.</summary>
  /// <seealso cref="Windows.UI.Xaml.Controls.Page" />
  /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
  /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
  /// <seealso cref="Microsoft.Practices.Prism.Mvvm.IView" />
  public sealed partial class PlaylistPage : Page, IView
  {
    #region Constructors

    /// <summary>Initializes a new instance of the <see cref="PlaylistPage" /> class.</summary>
    public PlaylistPage()
    {
      this.InitializeComponent();

      if (!DesignMode.DesignModeEnabled)
      {
        ViewModelLocator.SetAutoWireViewModel(this, true);
      }
    }

    #endregion
  }
}