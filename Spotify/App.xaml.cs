namespace Spotify
{
  using System.Threading.Tasks;

  using Microsoft.Practices.Prism.Mvvm;

  using Spotify.ViewModels;

  using Unity;

  using Windows.ApplicationModel;
  using Windows.ApplicationModel.Activation;
  using Windows.Globalization;

  /// <summary>The application class.</summary>
  /// <seealso cref="Microsoft.Practices.Prism.Mvvm.MvvmAppBase" />
  /// <seealso cref="Windows.UI.Xaml.Markup.IXamlMetadataProvider" />
  public sealed partial class App : MvvmAppBase
  {
    #region Fields

    /// <summary>The container used in this application.</summary>
    private readonly IUnityContainer container = UnityContainerFactory.Create();

    #endregion

    #region Constructors

    /// <summary>Initializes a new instance of the <see cref="App" /> class.</summary>
    public App()
    {
      this.InitializeComponent();

      this.Suspending += this.AppSuspending;
    }

    #endregion

    #region Methods

    /// <summary>Raises the <see cref="E:InitializeAsync" /> event.</summary>
    /// <param name="args">The <see cref="IActivatedEventArgs" /> instance containing the event data.</param>
    /// <returns>A completed task.</returns>
    protected override Task OnInitializeAsync(IActivatedEventArgs args)
    {
      ViewModelLocationProvider.SetDefaultViewModelFactory(t => this.container.Resolve(t));
      ApplicationLanguages.PrimaryLanguageOverride = "en-US";
      return Task.CompletedTask;
    }

    /// <summary>Raises the <see cref="E:LaunchApplicationAsync" /> event.</summary>
    /// <param name="args">The <see cref="LaunchActivatedEventArgs" /> instance containing the event data.</param>
    /// <returns>A completed task.</returns>
    protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
    {
      this.NavigationService.Navigate("Main", null);
      return Task.CompletedTask;
    }

    /// <summary>Handles the Suspending event of the App.</summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="SuspendingEventArgs" /> instance containing the event data.</param>
    private void AppSuspending(object sender, SuspendingEventArgs e)
    {
      this.container.Resolve<SettingsPageViewModel>().OnNavigatedFrom();
    }

    #endregion
  }
}