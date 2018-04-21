namespace Spotify.Events
{
  using Prism.Events;

  using SpotifyWebApi.Model;

  internal class PlaylistChangedEvent : PubSubEvent<FullPlaylist>
  {
  }
}