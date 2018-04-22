namespace Spotify.Events
{
  using Prism.Events;

  using SpotifyWebApi.Model;

  /// <summary>The devices changed event.</summary>
  internal class DevicesChangedEvent : PubSubEvent<DevicesContainer>
  {
  }
}