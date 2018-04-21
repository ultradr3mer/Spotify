using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyWebApi;
using SpotifyWebApi.Model;
using SpotifyWebApi.Model.Auth;

namespace Spotify.Data
{
  class ConnectionData
  {
    public PrivateUser Profile { get; set; }
    public IList<SimplePlaylist> Playlists { get; set; }
    public ISpotifyWebApi Api { get; set; }
    public Token Token { get; set; }
  }
}
