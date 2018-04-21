using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify.Data
{
  using SpotifyWebApi.Model;
  using SpotifyWebApi.Model.Uri;

  class PlaylistTrackData
  {
    public string PlaylistUri { get; set; }
    public PlaylistTrack PlaylistTrack { get; set; }
  }
}
