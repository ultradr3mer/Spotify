namespace Spotify.Services
{
  using System;
  using System.Linq;

  using Prism.Events;

  using Spotify.Events;

  using SpotifyWebApi;
  using SpotifyWebApi.Business;
  using SpotifyWebApi.Model;
  using SpotifyWebApi.Model.Auth;
  using SpotifyWebApi.Model.Uri;

  using Unity;

  using Windows.UI.Xaml.Media.Imaging;

  /// <summary>The Playlist Service.</summary>
  internal class PlaylistService
  {
    #region Fields

    /// <summary>The API Client.</summary>
    private readonly ISpotifyWebApi api;

    /// <summary>The event aggregator.</summary>
    private readonly IEventAggregator eventAggregator;

    /// <summary>The token.</summary>
    private readonly Token token;

    /// <summary>The container.</summary>
    private IUnityContainer container;

    #endregion

    #region Constructors

    /// <summary>Initializes a new instance of the <see cref="PlaylistService" /> class inside the context of the given container.</summary>
    /// <param name="container">The container.</param>
    public PlaylistService(IUnityContainer container)
    {
      this.container = container;
      this.eventAggregator = container.Resolve<IEventAggregator>();

      this.api = container.Resolve<ISpotifyWebApi>();
      this.token = container.Resolve<Token>();
    }

    #endregion

    #region Methods

    /// <summary>Sets the playlist.</summary>
    /// <param name="uri">The URI of the playlist.</param>
    public async void SetPlaylist(SpotifyUri uri)
    {
      var getPlaylist = this.api.Playlist.GetPlaylist(uri);

      var playList = await getPlaylist;

      this.eventAggregator.GetEvent<PlaylistChangedEvent>().Publish(playList);

      this.LoadAllTracks(playList.Tracks);
      this.LoadImage(playList.Images.First());
    }

    /// <summary>Loads all tracks.</summary>
    /// <param name="curPage">The current page.</param>
    private async void LoadAllTracks(Paging<PlaylistTrack> curPage)
    {
      while (curPage.Next != null)
      {
        var nextPage = await ApiClient.GetAsync<Paging<PlaylistTrack>>(new Uri(curPage.Next), this.token);
        Paging<PlaylistTrack> response;
        if ((response = nextPage.Response as Paging<PlaylistTrack>) != null)
        {
          curPage = response;
          this.eventAggregator.GetEvent<PlaylistItemsAddedEvent>().Publish(response.Items);
        }
      }
    }

    /// <summary>Loads the image.</summary>
    /// <param name="image">The image.</param>
    private void LoadImage(Image image)
    {
      var bitmapImage = new BitmapImage(new Uri(image.Url));
      this.eventAggregator.GetEvent<PlaylistImageChangedEvent>().Publish(bitmapImage);
    }

    #endregion
  }
}