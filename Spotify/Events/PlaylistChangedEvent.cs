namespace Spotify.Events
{
  using Prism.Events;

  using SpotifyWebApi.Model;

  /// <summary>The playlist changed event.</summary>
  internal class PlaylistChangedEvent : PubSubEvent<FullPlaylist>
  {
  }
}