namespace Spotify.Services
{
  using Prism.Events;

  using Spotify.Data;
  using Spotify.Events;

  using Unity;

  using Windows.Storage;

  /// <summary>The settings service.</summary>
  internal class SettingsService
  {
    #region Fields

    /// <summary>The client identifier value name.</summary>
    private const string ClientIdValueName = "ClientId";

    /// <summary>The client secret value name.</summary>
    private const string ClientSecretValueName = "ClientSecret";

    /// <summary>The event aggregator.</summary>
    private readonly IEventAggregator eventAggregator;

    /// <summary>The local settings.</summary>
    private readonly ApplicationDataContainer localSettings;

    #endregion

    #region Constructors

    /// <summary>Initializes a new instance of the <see cref="SettingsService" /> class inside the context of the given container.</summary>
    /// <param name="container">The container.</param>
    public SettingsService(IUnityContainer container)
    {
      this.eventAggregator = container.Resolve<IEventAggregator>();
      this.localSettings = ApplicationData.Current.LocalSettings;
    }

    #endregion

    #region Methods

    /// <summary>Reads the settings.</summary>
    public void ReadSettings()
    {
      var settings = new SettingsData
                       {
                         ClientId = (string)this.localSettings.Values[SettingsService.ClientIdValueName],
                         ClientSecret = (string)this.localSettings.Values[SettingsService.ClientSecretValueName]
                       };

      this.eventAggregator.GetEvent<SettingsChangedEvent>().Publish(settings);
    }

    /// <summary>Sets the settings.</summary>
    /// <param name="data">The data.</param>
    public void SetSettings(SettingsData data)
    {
      this.localSettings.Values[SettingsService.ClientIdValueName] = data.ClientId;
      this.localSettings.Values[SettingsService.ClientSecretValueName] = data.ClientSecret;

      this.ReadSettings();
    }

    #endregion
  }
}