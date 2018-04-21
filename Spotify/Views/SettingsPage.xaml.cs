namespace Spotify.Views
{
  using Microsoft.Practices.Prism.Mvvm;

  using Windows.ApplicationModel;
  using Windows.UI.Xaml.Controls;
  using Windows.UI.Xaml.Navigation;

  using Spotify.ViewModels;

  /// <summary>The page for the Application Settings.</summary>
  public sealed partial class SettingsPage : Page, IView
  {
    #region Constructors

    public SettingsPage()
    {
      this.InitializeComponent();

      if (!DesignMode.DesignModeEnabled)
      {
        ViewModelLocator.SetAutoWireViewModel(this, true);
      }
    }

    private SettingsPageViewModel ViewModel
    {
      get { return this.DataContext as SettingsPageViewModel;}
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      base.OnNavigatedFrom(e);

      this.ViewModel.OnNavigatedFrom();
    }

    #endregion
  }
}