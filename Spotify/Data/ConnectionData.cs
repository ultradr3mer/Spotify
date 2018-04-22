namespace Spotify.Data
{
  using System.Collections.Generic;

  using SpotifyWebApi;
  using SpotifyWebApi.Model;
  using SpotifyWebApi.Model.Auth;

  /// <summary>The connection data.</summary>
  internal class ConnectionData
  {
    #region Properties

    /// <summary>Gets or sets the api client.</summary>
    public ISpotifyWebApi Api { get; set; }

    /// <summary>Gets or sets the users playlists.</summary>
    public IList<SimplePlaylist> Playlists { get; set; }

    /// <summary>Gets or sets the users profile.</summary>
    public PrivateUser Profile { get; set; }

    /// <summary>Gets or sets the api token.</summary>
    public Token Token { get; set; }

    #endregion
  }
}