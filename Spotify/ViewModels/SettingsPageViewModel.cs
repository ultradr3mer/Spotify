namespace Spotify.ViewModels
{
  using Prism.Events;

  using Spotify.Data;
  using Spotify.Events;
  using Spotify.PrismExtensions;
  using Spotify.Services;

  using Unity;

  /// <summary>The settings page view model.</summary>
  /// <seealso cref="Spotify.PrismExtensions.ViewModelWithAttachable{Spotify.Data.SettingsData}" />
  internal class SettingsPageViewModel : ViewModelWithAttachable<SettingsData>
  {
    #region Fields

    /// <summary>The settings service</summary>
    private readonly SettingsService settingsService;

    /// <summary>The property client identifiers value.</summary>
    private string propClientId;

    /// <summary>The property client secrets value.</summary>
    private string propClientSecret;

    #endregion

    #region Constructors

    /// <summary>Initializes a new instance of the <see cref="SettingsPageViewModel" /> class.</summary>
    /// <param name="container">The container.</param>
    public SettingsPageViewModel(IUnityContainer container)
    {
      var eventAggregator = container.Resolve<IEventAggregator>();
      eventAggregator.GetEvent<SettingsChangedEvent>().Subscribe(this.HandleSettingsChanged);

      this.settingsService = container.Resolve<SettingsService>();
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the client id.
    /// </summary>
    public string ClientId
    {
      get { return this.propClientId; }
      set { this.SetProperty(ref this.propClientId, value); }
    }

    /// <summary>
    /// Gets or sets the client secret.
    /// </summary>
    public string ClientSecret
    {
      get { return this.propClientSecret; }
      set { this.SetProperty(ref this.propClientSecret, value); }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Called when navigated from this view.
    /// </summary>
    internal void OnNavigatedFrom()
    {
      this.settingsService.SetSettings(this.AttachedDataModel);
    }

    /// <summary>
    /// Handles the settings changed event.
    /// </summary>
    /// <param name="data">The data.</param>
    private void HandleSettingsChanged(SettingsData data)
    {
      this.AttachedDataModel = data;
    }

    #endregion
  }
}