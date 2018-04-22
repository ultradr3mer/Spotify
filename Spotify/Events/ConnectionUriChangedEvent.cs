namespace Spotify.Events
{
  using System;

  using Prism.Events;

  /// <summary>The connection uri changed event.</summary>
  internal class ConnectionUriChangedEvent : PubSubEvent<Uri>
  {
  }
}