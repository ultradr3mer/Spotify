namespace Spotify.ViewModels
{
  using Spotify.PrismExtensions;

  using SpotifyWebApi.Model;

  internal class DeviceComboboxItemViewModel : ViewModelWithAttachable<Device>
  {
    #region Fields

    private string propId;

    private bool propIsActive;

    private string propName;

    #endregion

    #region Properties

    public string Id
    {
      get { return this.propId; }
      set { this.SetProperty(ref this.propId, value); }
    }

    public bool IsActive
    {
      get { return this.propIsActive; }
      set { this.SetProperty(ref this.propIsActive, value); }
    }

    public string Name
    {
      get { return this.propName; }
      set { this.SetProperty(ref this.propName, value); }
    }

    #endregion
  }
}