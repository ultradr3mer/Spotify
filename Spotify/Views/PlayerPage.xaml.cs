namespace Spotify.Views
{
  using Microsoft.Practices.Prism.Mvvm;

  using Spotify.ViewModels;

  using Windows.ApplicationModel;
  using Windows.UI.Xaml;
  using Windows.UI.Xaml.Controls;
  using Windows.UI.Xaml.Input;

  /// <summary>The player page.</summary>
  /// <seealso cref="Windows.UI.Xaml.Controls.Page" />
  /// <seealso cref="Microsoft.Practices.Prism.Mvvm.IView" />
  /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
  /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
  public sealed partial class PlayerPage : Page, IView
  {
    #region Constructors

    /// <summary>Initializes a new instance of the <see cref="PlayerPage" /> class.</summary>
    public PlayerPage()
    {
      this.InitializeComponent();

      if (!DesignMode.DesignModeEnabled)
      {
        ViewModelLocator.SetAutoWireViewModel(this, true);
      }
    }

    #endregion

    #region Properties

    /// <summary>Gets the view model.</summary>
    private PlayerPageViewModel ViewModel
    {
      get { return this.DataContext as PlayerPageViewModel; }
    }

    #endregion

    #region Methods

    /// <summary>Is called when the progress got focus.</summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
    private void ProgressGotFocus(object sender, RoutedEventArgs e)
    {
      this.ViewModel.ProgressHasFocus = true;
    }

    /// <summary>Is called when the progress lost focus.</summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
    private void ProgressLostFocus(object sender, RoutedEventArgs e)
    {
      this.ViewModel.ProgressHasFocus = false;
    }

    /// <summary>Is called when the pointer exited the progress bar.</summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="PointerRoutedEventArgs" /> instance containing the event data.</param>
    private void ProgressPointerExited(object sender, PointerRoutedEventArgs e)
    {
      if (this.ViewModel.ProgressHasFocus)
      {
        this.Focus(FocusState.Pointer);
      }
    }

    /// <summary>Is called when the volume got focus.</summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
    private void VolumeGotFocus(object sender, RoutedEventArgs e)
    {
      this.ViewModel.VolumeHasFocus = true;
    }

    /// <summary>Is called when the volume lost focus.</summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
    private void VolumeLostFocus(object sender, RoutedEventArgs e)
    {
      this.ViewModel.VolumeHasFocus = false;
    }

    /// <summary>Is called when the pointer exited volume.</summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="PointerRoutedEventArgs" /> instance containing the event data.</param>
    private void VolumePointerExited(object sender, PointerRoutedEventArgs e)
    {
      if (this.ViewModel.VolumeHasFocus)
      {
        this.Focus(FocusState.Pointer);
      }
    }

    #endregion
  }
}