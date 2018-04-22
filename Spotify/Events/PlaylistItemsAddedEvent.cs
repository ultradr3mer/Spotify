namespace Spotify.Events
{
  using System.Collections.Generic;

  using Prism.Events;

  using SpotifyWebApi.Model;

  /// <summary>The playlist items added event.</summary>
  internal class PlaylistItemsAddedEvent : PubSubEvent<IEnumerable<PlaylistTrack>>
  {
  }
}