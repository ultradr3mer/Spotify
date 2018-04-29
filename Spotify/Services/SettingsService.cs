namespace Spotify.Services
{
  using Prism.Events;

  using Spotify.Data;
  using Spotify.Events;

  using Unity;

  using Windows.Storage;
  using Windows.UI;
  using Windows.UI.Xaml;

  /// <summary>The settings service.</summary>
  internal class SettingsService
  {
    #region Fields

    /// <summary>The client identifier value name.</summary>
    private const string ClientIdValueName = "ClientId";

    /// <summary>The client secret value name.</summary>
    private const string ClientSecretValueName = "ClientSecret";

    /// <summary>The use system accent color value name.</summary>
    private static readonly string UseSystemAccentColorValueName = "UseSystemAccentColor";

    /// <summary>The event aggregator.</summary>
    private readonly IEventAggregator eventAggregator;

    /// <summary>The local settings.</summary>
    private readonly ApplicationDataContainer localSettings;

    /// <summary>A value indicating whether the accent color is set.</summary>
    private bool isAccentColorSet;

    #endregion

    #region Constructors

    /// <summary>Initializes a new instance of the <see cref="SettingsService" /> class inside the context of the given container.</summary>
    /// <param name="container">The container.</param>
    public SettingsService(IUnityContainer container)
    {
      this.eventAggregator = container.Resolve<IEventAggregator>();
      this.localSettings = ApplicationData.Current.LocalSettings;

      this.eventAggregator.GetEvent<SettingsChangedEvent>().Subscribe(this.HandleSettingsChanged);
    }

    #endregion

    #region Methods

    /// <summary>Reads the settings.</summary>
    public void ReadSettings()
    {
      var settings = new SettingsData
                       {
                         ClientId = (string)this.localSettings.Values[SettingsService.ClientIdValueName],
                         ClientSecret = (string)this.localSettings.Values[SettingsService.ClientSecretValueName],
                         UseSystemAccentColor = (bool?)this.localSettings.Values[SettingsService.UseSystemAccentColorValueName] ?? false
                       };

      this.eventAggregator.GetEvent<SettingsChangedEvent>().Publish(settings);
    }

    /// <summary>Sets the settings.</summary>
    /// <param name="data">The data.</param>
    public void SetSettings(SettingsData data)
    {
      this.localSettings.Values[SettingsService.ClientIdValueName] = data.ClientId;
      this.localSettings.Values[SettingsService.ClientSecretValueName] = data.ClientSecret;
      this.localSettings.Values[SettingsService.UseSystemAccentColorValueName] = data.UseSystemAccentColor;

      this.ReadSettings();
    }

    /// <summary>Sets the color of the lower bar color from the accent color.</summary>
    /// <param name="multiplier">The color value multiplier.</param>
    /// <param name="accentColor">The accent color</param>
    /// <returns>The <see cref="Color" />.</returns>
    private Color GetLowerBarColorFromAccentColor(double multiplier, Color accentColor)
    {
      return Color.FromArgb(byte.MaxValue, (byte)(accentColor.R * multiplier), (byte)(accentColor.G * multiplier), (byte)(accentColor.B * multiplier));
    }

    /// <summary>Handles the settings changed event.</summary>
    /// <param name="data">The data.</param>
    private void HandleSettingsChanged(SettingsData data)
    {
      if (!this.isAccentColorSet)
      {
        var systemAccentColor = (Color)Application.Current.Resources["SystemAccentColor"];
        var spotifyAccentColor = (Color)Application.Current.Resources["SpotifyAccentColor"];

        var accentColor = data.UseSystemAccentColor ? systemAccentColor : spotifyAccentColor;
        Application.Current.Resources["SystemAccentColor"] = accentColor;
        Application.Current.Resources["LowerBarColor"] = this.GetLowerBarColorFromAccentColor(0.5, accentColor);

        this.isAccentColorSet = true;
      }
    }

    #endregion
  }
}