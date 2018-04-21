namespace Spotify.Events
{
  using Prism.Events;

  using SpotifyWebApi.Model;

  internal class CurrentlyPlayingChangedEvent : PubSubEvent<CurrentlyPlaying>
  {
  }
}