namespace Spotify.ViewModels
{
  using System;

  using Microsoft.Practices.Prism.Mvvm;

  using Prism.Events;

  using Spotify.Data;
  using Spotify.Events;
  using Spotify.Services;

  using Unity;

  /// <summary>
  /// The startup page view model.
  /// </summary>
  /// <seealso cref="Microsoft.Practices.Prism.Mvvm.ViewModel" />
  internal class StartupPageViewModel : ViewModel
  {
    #region Fields

    /// <summary>
    /// The connection service.
    /// </summary>
    private readonly ConnectionService connectionService;

    /// <summary>
    /// The property connect URLs value.
    /// </summary>
    private Uri propConnectUrl;

    /// <summary>
    /// The settings.
    /// </summary>
    private SettingsData settings;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="StartupPageViewModel"/> class in the context of the given container.
    /// </summary>
    /// <param name="container">The container.</param>
    public StartupPageViewModel(IUnityContainer container)
    {
      var eventAggregator = container.Resolve<IEventAggregator>();
      eventAggregator.GetEvent<ConnectionUriChangedEvent>().Subscribe(this.HandleConnectionUriChanged);
      eventAggregator.GetEvent<SettingsChangedEvent>().Subscribe(this.HandleSettingsChanged);

      this.ConnectUrl = new Uri("ms-appx-web:///Assets/SettingsInfo.html");

      var settingsService = container.Resolve<SettingsService>();
      settingsService.ReadSettings();

      this.connectionService = container.Resolve<ConnectionService>();
      this.connectionService.TryInitializeConnection(this.settings.ClientId, this.settings.ClientSecret);
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the connect URL.
    /// </summary>
    public Uri ConnectUrl
    {
      get { return this.propConnectUrl; }
      set { this.SetProperty(ref this.propConnectUrl, value); }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Called when navigated to this view model.
    /// </summary>
    internal void OnNavigatedTo()
    {
      this.connectionService.TryInitializeConnection(this.settings.ClientId, this.settings.ClientSecret);
    }

    /// <summary>
    /// Handles the connection URI changed event.
    /// </summary>
    /// <param name="uri">The URI.</param>
    private void HandleConnectionUriChanged(Uri uri)
    {
      this.ConnectUrl = uri;
    }

    /// <summary>
    /// Handles the settings changed event.
    /// </summary>
    /// <param name="newSettings">The settings.</param>
    private void HandleSettingsChanged(SettingsData newSettings)
    {
      this.settings = newSettings;
    }

    #endregion
  }
}