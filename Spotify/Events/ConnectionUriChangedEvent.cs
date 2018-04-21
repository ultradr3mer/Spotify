namespace Spotify.Events
{
  using System;

  using Prism.Events;

  internal class ConnectionUriChangedEvent : PubSubEvent<Uri>
  {
  }
}