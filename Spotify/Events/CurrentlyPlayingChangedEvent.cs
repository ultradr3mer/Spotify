namespace Spotify.Events
{
  using Prism.Events;

  using SpotifyWebApi.Model;

  /// <summary>The currently playing changed event.</summary>
  internal class CurrentlyPlayingChangedEvent : PubSubEvent<CurrentlyPlaying>
  {
  }
}