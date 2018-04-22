namespace Spotify.Data
{
  using SpotifyWebApi.Model;

  /// <summary>The playlist track data.</summary>
  internal class PlaylistTrackData
  {
    #region Properties

    /// <summary>Gets or sets the playlist track.</summary>
    public PlaylistTrack PlaylistTrack { get; set; }

    /// <summary>Gets or sets the playlist uri.</summary>
    public string PlaylistUri { get; set; }

    #endregion
  }
}