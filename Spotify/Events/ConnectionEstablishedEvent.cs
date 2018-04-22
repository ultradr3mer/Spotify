namespace Spotify.Events
{
  using Prism.Events;

  using Spotify.Data;

  /// <summary>The connection established event.</summary>
  internal class ConnectionEstablishedEvent : PubSubEvent<ConnectionData>
  {
  }
}