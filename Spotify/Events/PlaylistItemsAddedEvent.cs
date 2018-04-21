namespace Spotify.Events
{
  using System.Collections.Generic;

  using Prism.Events;

  using SpotifyWebApi.Model;

  internal class PlaylistItemsAddedEvent : PubSubEvent<IEnumerable<PlaylistTrack>>
  {
  }
}