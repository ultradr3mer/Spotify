namespace Spotify.Events
{
  using Prism.Events;

  using SpotifyWebApi.Model;

  internal class DevicesChangedEvent : PubSubEvent<DevicesContainer>
  {
  }
}