

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.
namespace Spotify.Views
{
  using Microsoft.Practices.Prism.Mvvm;

  using Spotify.Services;
  using Spotify.ViewModels;

  using Windows.ApplicationModel;
  using Windows.UI.Xaml.Controls;
  using Windows.UI.Xaml.Navigation;

  /// <summary>Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.</summary>
  public sealed partial class StartupPage : Page, IView
  {
    #region Constructors

    public StartupPage()
    {
      this.InitializeComponent();

      if (!DesignMode.DesignModeEnabled)
      {
        ViewModelLocator.SetAutoWireViewModel(this, true);
      }
    }

    #endregion

    #region Properties

    private StartupPageViewModel ViewModel
    {
      get { return this.DataContext as StartupPageViewModel; }
    }

    #endregion

    #region Methods

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);

      this.ViewModel.OnNavigatedTo();
    }

    private void WebView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
    {
      if (args.Uri.Host == ConnectionService.CallbackHost)
      {
        this.ExitStoryboard.Begin();
      }
    }

    #endregion
  }
}