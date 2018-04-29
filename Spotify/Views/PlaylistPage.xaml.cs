namespace Spotify.Views
{
  using System.ComponentModel;

  using Microsoft.Practices.Prism.Mvvm;
  using Microsoft.Toolkit.Uwp.UI.Animations;

  using Spotify.ViewModels;

  using Windows.ApplicationModel;
  using Windows.UI.Xaml;
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

      this.ViewModel.PropertyChanged += this.ViewModelPropertyChanged;

      this.Unloaded += this.PlaylistPageUnloaded;
    }

    #endregion

    #region Properties

    /// <summary>Gets the view model.</summary>
    private PlaylistPageViewModel ViewModel
    {
      get { return this.DataContext as PlaylistPageViewModel; }
    }

    #endregion

    #region Methods

    /// <summary>Handles the Unloaded event of the PlaylistPage control.</summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs" /> instance containing the event data.</param>
    private void PlaylistPageUnloaded(object sender, RoutedEventArgs e)
    {
      this.ViewModel.PropertyChanged -= this.ViewModelPropertyChanged;
    }

    /// <summary>Handles the PropertyChanged event of the ViewModel control.</summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs" /> instance containing the event data.</param>
    private async void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == "Image" && this.ViewModel.Image != null)
      {
        await this.BlurImage.Blur(20).StartAsync();
      }
    }

    #endregion
  }
}