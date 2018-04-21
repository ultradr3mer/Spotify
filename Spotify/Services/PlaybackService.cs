namespace Spotify.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;

  using Prism.Events;

  using Spotify.ApiModelExtensions;
  using Spotify.Events;

  using SpotifyWebApi;
  using SpotifyWebApi.Model;
  using SpotifyWebApi.Model.Offset;
  using SpotifyWebApi.Model.Uri;

  using Unity;

  using Windows.ApplicationModel.Background;
  using Windows.UI.Xaml.Media.Imaging;

  internal class PlaybackService
  {
    #region Fields

    private readonly IUnityContainer container;

    private readonly DevicesContainerEqualityComparer devicesEqualityComparer = new DevicesContainerEqualityComparer();

    private readonly IEventAggregator eventAggregator;

    private readonly CurrentlyPlayingEqualityComparer playingEqualityComparer = new CurrentlyPlayingEqualityComparer();

    private readonly bool running = true;

    private ISpotifyWebApi api;

    private CurrentlyPlaying currentlyPlaying;

    private string currentlyPlayingAlbumUri;

    private DevicesContainer devicesContainer;

    private ApplicationTrigger trigger;

    #endregion

    #region Constructors

    public PlaybackService(IUnityContainer container)
    {
      this.container = container;

      this.WebPlayerClientName = "UWP Client on " + Environment.MachineName;

      this.eventAggregator = container.Resolve<IEventAggregator>();
    }

    #endregion

    #region Properties

    public string WebPlayerClientName { get; }

    #endregion

    #region Methods

    public async void ActivateDevice(string deviceId)
    {
      await this.api.Player.TransferPlayback(
        new List<string>
          {
            deviceId
          });
    }

    public void Next()
    {
      this.api.Player.Next();
    }

    public void Pause()
    {
      this.api.Player.PausePlayback();
    }

    public void Previous()
    {
      this.api.Player.Previous();
    }

    public void Resume()
    {
      this.api.Player.StartPlayback();
    }

    public void SetProgress(int progress)
    {
      this.api.Player.Seek(progress);
    }

    public async void SetSong(SpotifyUri context, SpotifyUri uri)
    {
      await this.EnsureActiveDeviceExists();

      await this.api.Player.StartPlayback(
        null,
        context,
        null,
        new UriOffset
          {
            Uri = uri.FullUri
          });
    }

    public void StartContinuousUpdate()
    {
      this.api = this.container.Resolve<ISpotifyWebApi>();
      this.UpdateContinuously();
    }

    private async Task EnsureActiveDeviceExists()
    {
      if (this.devicesContainer == null)
      {
        return;
      }

      var activeDevice = this.devicesContainer.Devices.FirstOrDefault(o => o.IsActive);
      if (activeDevice == null)
      {
        var localDevice = this.devicesContainer.Devices.FirstOrDefault(o => o.Name == this.WebPlayerClientName);
        if (localDevice != null)
        {
          this.ActivateDevice(localDevice.Id);
          while (!this.devicesContainer.Devices.Any(o => o.IsActive)) await Task.Delay(100);
        }
      }
    }

    private async void UpdateContinuously()
    {
      while (this.running)
      {
        var newDevicesContainer = await this.api.Player.GetAvailableDevices();

        if (!this.devicesEqualityComparer.Equals(this.devicesContainer, newDevicesContainer))
        {
          this.eventAggregator.GetEvent<DevicesChangedEvent>().Publish(newDevicesContainer);
          this.devicesContainer = newDevicesContainer;
        }

        var newCurrentlyPlaying = await this.api.Player.GetCurrentlyPlaying();

        if (!this.playingEqualityComparer.Equals(this.currentlyPlaying, newCurrentlyPlaying))
        {
          this.eventAggregator.GetEvent<CurrentlyPlayingChangedEvent>().Publish(newCurrentlyPlaying);
          this.currentlyPlaying = newCurrentlyPlaying;

          await this.UpdateCurrentlyPlayingImage();
        }

        await Task.Delay(TimeSpan.FromSeconds(1));
      }
    }

    private async Task UpdateCurrentlyPlayingImage()
    {
      var albumUriString = this.currentlyPlaying.Item?.Album.Uri;
      if (albumUriString == this.currentlyPlayingAlbumUri)
      {
        return;
      }

      this.currentlyPlayingAlbumUri = albumUriString;

      if (albumUriString != null)
      {
        var albumUri = SpotifyUri.Make(albumUriString);
        var album = await this.api.Album.GetAlbum(albumUri);
        var bitmapUri = new Uri(album.Images.First().Url);
        var image = new BitmapImage(bitmapUri);

        this.eventAggregator.GetEvent<CurrentlyPlayingImageChanged>().Publish(image);
      }
      else
      {
        this.eventAggregator.GetEvent<CurrentlyPlayingImageChanged>().Publish(null);
      }
    }

    #endregion
  }
}