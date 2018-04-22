namespace Spotify.ViewModels
{
  using Spotify.PrismExtensions;

  using SpotifyWebApi.Model;

  /// <summary>The view model for device combo box items.</summary>
  /// <seealso cref="Spotify.PrismExtensions.ViewModelWithAttachable{SpotifyWebApi.Model.Device}" />
  internal class DeviceComboBoxItemViewModel : ViewModelWithAttachable<Device>
  {
    #region Fields

    /// <summary>The property identifiers value.</summary>
    private string propId;

    /// <summary>The property is actives value.</summary>
    private bool propIsActive;

    /// <summary>The property names value.</summary>
    private string propName;

    #endregion

    #region Properties

    /// <summary>Gets or sets the identifier.</summary>
    public string Id
    {
      get { return this.propId; }
      set { this.SetProperty(ref this.propId, value); }
    }

    /// <summary>Gets or sets a value indicating whether this instance is active.</summary>
    public bool IsActive
    {
      get { return this.propIsActive; }
      set { this.SetProperty(ref this.propIsActive, value); }
    }

    /// <summary>Gets or sets the name.</summary>
    public string Name
    {
      get { return this.propName; }
      set { this.SetProperty(ref this.propName, value); }
    }

    #endregion
  }
}