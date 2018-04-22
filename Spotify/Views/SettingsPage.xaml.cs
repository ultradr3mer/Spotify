namespace Spotify.Views
{
  using Microsoft.Practices.Prism.Mvvm;

  using Spotify.ViewModels;

  using Windows.ApplicationModel;
  using Windows.UI.Xaml.Controls;
  using Windows.UI.Xaml.Navigation;

  /// <summary>The page for the application settings.</summary>
  public sealed partial class SettingsPage : Page, IView
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsPage"/> class.
    /// </summary>
    public SettingsPage()
    {
      this.InitializeComponent();

      if (!DesignMode.DesignModeEnabled)
      {
        ViewModelLocator.SetAutoWireViewModel(this, true);
      }
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the view model.
    /// </summary>
    private SettingsPageViewModel ViewModel
    {
      get { return this.DataContext as SettingsPageViewModel; }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Invoked immediately after the Page is unloaded and is no longer the current source of a parent Frame.
    /// </summary>
    /// <param name="e">Event data that can be examined by overriding code. The event data is representative of the navigation that has unloaded the current Page.</param>
    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      base.OnNavigatedFrom(e);

      this.ViewModel.OnNavigatedFrom();
    }

    #endregion
  }
}