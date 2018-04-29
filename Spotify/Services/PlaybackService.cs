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

  using Windows.UI.Xaml.Media.Imaging;

  /// <summary>The playback service.</summary>
  internal class PlaybackService
  {
    #region Fields

    /// <summary>The unity container.</summary>
    private readonly IUnityContainer container;

    /// <summary>The devices equality comparer.</summary>
    private readonly DevicesContainerEqualityComparer devicesEqualityComparer = new DevicesContainerEqualityComparer();

    /// <summary>The event aggregator.</summary>
    private readonly IEventAggregator eventAggregator;

    /// <summary>The playing equality comparer.</summary>
    private readonly CurrentlyPlayingContextEqualityComparer playingEqualityComparer = new CurrentlyPlayingContextEqualityComparer();

    /// <summary>The running.</summary>
    private readonly bool running = true;

    /// <summary>The api client.</summary>
    private ISpotifyWebApi api;

    /// <summary>The currently playing album uri.</summary>
    private string currentlyPlayingAlbumUri;

    /// <summary>The currently playing context model.</summary>
    private CurrentlyPlayingContext currentlyPlayingContext;

    /// <summary>The devices container.</summary>
    private DevicesContainer devicesContainer;

    #endregion

    #region Constructors

    /// <summary>Initializes a new instance of the <see cref="PlaybackService" /> class inside the context of the given container.</summary>
    /// <param name="container">The container.</param>
    public PlaybackService(IUnityContainer container)
    {
      this.container = container;

      this.WebPlayerClientName = "UWP Client on " + Environment.MachineName;

      this.eventAggregator = container.Resolve<IEventAggregator>();
    }

    #endregion

    #region Properties

    /// <summary>Gets the web player client name.</summary>
    public string WebPlayerClientName { get; }

    #endregion

    #region Methods

    /// <summary>Activates the device.</summary>
    /// <param name="deviceId">The device identifier.</param>
    /// <param name="playBack">Whether the playback on the new device is active.</param>
    public async void ActivateDevice(string deviceId, bool? playBack = null)
    {
      await this.api.Player.TransferPlayback(
        new List<string>
          {
            deviceId
          },
        playBack);
    }

    /// <summary>Sets the playback to the next title.</summary>
    public void Next()
    {
      this.api.Player.Next();
    }

    /// <summary>Pauses the playback.</summary>
    public void Pause()
    {
      this.api.Player.PausePlayback();
    }

    /// <summary>Sets the playback to the previous title.</summary>
    public void Previous()
    {
      this.api.Player.Previous();
    }

    /// <summary>Resumes the playback.</summary>
    public void Resume()
    {
      this.api.Player.StartPlayback();
    }

    /// <summary>Sets the  currently played playlist.</summary>
    /// <param name="uri">The uri of the playlist.</param>
    public async void SetPlaylist(SpotifyUri uri)
    {
      await this.EnsureActiveDeviceExists();

      await this.api.Player.StartPlayback(null, uri, null, null);
    }

    /// <summary>Sets the progress inside the current title.</summary>
    /// <param name="progressMs">The progress in milliseconds.</param>
    public void SetProgress(int progressMs)
    {
      this.api.Player.Seek(progressMs);
    }

    /// <summary>Sets whether the shuffle is active.</summary>
    /// <param name="b">if set to <c>true</c> the shuffle is active.</param>
    public void SetShuffle(bool b)
    {
      this.api.Player.SetShuffle(b);
    }

    /// <summary>The set currently played song.</summary>
    /// <param name="context">The playing context.</param>
    /// <param name="uri">The uri of the title to play.</param>
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

    /// <summary>Sets the playback volume.</summary>
    /// <param name="volume">The volume in percent.</param>
    public async void SetVolume(int volume)
    {
      await this.api.Player.SetVolume(volume);
    }

    /// <summary>Starts the continuous update.</summary>
    public void StartContinuousUpdate()
    {
      this.api = this.container.Resolve<ISpotifyWebApi>();
      this.UpdateContinuously();
    }

    /// <summary>Ensures an active device exists by checking for an active device and activating this device as active device if no device was found.</summary>
    /// <returns>an awaitable.</returns>
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
          this.ActivateDevice(localDevice.Id, false);
          while (!this.devicesContainer.Devices.Any(o => o.IsActive)) await Task.Delay(100);
        }
      }
    }

    /// <summary>Updates continuously.</summary>
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

        var newCurrentlyPlayingContext = await this.api.Player.GetCurrentlyPlayingContext();

        if (!this.playingEqualityComparer.Equals(this.currentlyPlayingContext, newCurrentlyPlayingContext))
        {
          this.eventAggregator.GetEvent<CurrentlyPlayingContextChangedEvent>().Publish(newCurrentlyPlayingContext);
          this.currentlyPlayingContext = newCurrentlyPlayingContext;

          await this.UpdateCurrentlyPlayingImage();
        }

        await Task.Delay(TimeSpan.FromSeconds(1));
      }
    }

    /// <summary>Updates the currently playing image.</summary>
    /// <returns>an awaitable</returns>
    private async Task UpdateCurrentlyPlayingImage()
    {
      var albumUriString = this.currentlyPlayingContext.Item?.Album.Uri;
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