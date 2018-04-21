namespace Spotify.Services
{
  using Prism.Events;

  using Spotify.Data;
  using Spotify.Events;

  using Unity;

  using Windows.Storage;

  internal class SettingsService
  {
    #region Fields

    private const string ClientIdValueName = "ClientId";

    private const string ClientSecretValueName = "ClientSecret";

    private readonly IEventAggregator eventAggregator;

    private readonly ApplicationDataContainer localSettings;

    #endregion

    #region Constructors

    public SettingsService(IUnityContainer container)
    {
      this.eventAggregator = container.Resolve<IEventAggregator>();
      this.localSettings = ApplicationData.Current.LocalSettings;
    }

    #endregion

    #region Methods

    public void ReadSettings()
    {
      var settings = new SettingsData
                       {
                         ClientId = (string)this.localSettings.Values[SettingsService.ClientIdValueName],
                         ClientSecret = (string)this.localSettings.Values[SettingsService.ClientSecretValueName]
                       };

      this.eventAggregator.GetEvent<SettingsChangedEvent>().Publish(settings);
    }

    public void SetSettings(SettingsData data)
    {
      this.localSettings.Values[SettingsService.ClientIdValueName] = data.ClientId;
      this.localSettings.Values[SettingsService.ClientSecretValueName] = data.ClientSecret;

      this.ReadSettings();
    }

    #endregion
  }
}