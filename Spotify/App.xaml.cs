namespace Spotify
{
  using System.Threading.Tasks;

  using Microsoft.Practices.Prism.Mvvm;

  using Unity;

  using Windows.ApplicationModel.Activation;
  using Windows.Globalization;

  /// <summary>Stellt das anwendungsspezifische Verhalten bereit, um die Standardanwendungsklasse zu ergänzen.</summary>
  sealed partial class App : MvvmAppBase
  {
    #region Fields

    private readonly IUnityContainer container = UnityContainerFactory.Create();

    #endregion

    #region Constructors

    /// <summary>Initialisiert das Singletonanwendungsobjekt. Dies ist die erste Zeile von erstelltem Code und daher das logische Äquivalent von main() bzw. WinMain().</summary>
    public App()
    {
      this.InitializeComponent();
    }

    #endregion

    #region Methods

    protected override Task OnInitializeAsync(IActivatedEventArgs args)
    {
      ViewModelLocationProvider.SetDefaultViewModelFactory(t => this.container.Resolve(t));
      ApplicationLanguages.PrimaryLanguageOverride = "en-US";
      return Task.CompletedTask;
    }

    protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
    {
      this.NavigationService.Navigate("Main", null);
      return Task.CompletedTask;
    }

    #endregion
  }
}