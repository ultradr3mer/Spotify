namespace Spotify.Views
{
  using Microsoft.Practices.Prism.Mvvm;

  using Windows.ApplicationModel;
  using Windows.UI.Xaml.Controls;

  /// <summary>
  /// The player page.
  /// </summary>
  /// <seealso cref="Windows.UI.Xaml.Controls.Page" />
  /// <seealso cref="Microsoft.Practices.Prism.Mvvm.IView" />
  /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
  /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
  public sealed partial class PlayerPage : Page, IView
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="PlayerPage"/> class.
    /// </summary>
    public PlayerPage()
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