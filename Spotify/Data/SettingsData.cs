namespace Spotify.Data
{
  /// <summary>The settings data.</summary>
  internal class SettingsData
  {
    #region Properties

    /// <summary>Gets or sets the client id.</summary>
    public string ClientId { get; set; }

    /// <summary>Gets or sets the client secret.</summary>
    public string ClientSecret { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to use the system accent color.
    /// </summary>
    public bool UseSystemAccentColor { get; set; }

    #endregion
  }
}