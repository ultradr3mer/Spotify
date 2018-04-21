using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using Prism.Events;
using Spotify.Data;
using Spotify.Events;
using SpotifyWebApi;
using SpotifyWebApi.Business;
using SpotifyWebApi.Model;
using SpotifyWebApi.Model.Auth;
using SpotifyWebApi.Model.Uri;
using Unity;

namespace Spotify.Services
{
  class PlaylistService
  {
    private IUnityContainer container;
    private ISpotifyWebApi api;
    private IEventAggregator eventAggregator;
    private Token token;

    public PlaylistService(IUnityContainer container)
    {
      this.container = container;
      this.eventAggregator = container.Resolve<IEventAggregator>();

      this.api = container.Resolve<ISpotifyWebApi>();
      this.token = container.Resolve<Token>();
    }

    public async void SetPlaylist(SpotifyUri uri)
    {
      var getPlaylist = api.Playlist.GetPlaylist(uri);

      var playList = await getPlaylist;

      eventAggregator.GetEvent<PlaylistChangedEvent>().Publish(playList);

      this.LoadAllTracks(playList.Tracks);
      this.LoadImage(playList.Images.First());
    }

    private void LoadImage(Image image)
    {
      var bitmapImage = new BitmapImage(new Uri(image.Url));
      eventAggregator.GetEvent<PlaylistImageChangedEvent>().Publish(bitmapImage);
    }

    private async void LoadAllTracks(Paging<PlaylistTrack> curPage)
    {
      while (curPage.Next != null)
      {
        var nextPage = await ApiClient.GetAsync<Paging<PlaylistTrack>>(new Uri(curPage.Next), this.token);
        Paging<PlaylistTrack> response;
        if ((response = nextPage.Response as Paging<PlaylistTrack>) != null)
        {
          curPage = response;
          eventAggregator.GetEvent<PlaylistItemsAddedEvent>().Publish(response.Items);
        }
      }
    }
  }
}
