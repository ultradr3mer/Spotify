namespace Spotify.ViewModels
{
  using System;
  using System.Linq;
  using System.Windows.Input;

  using Microsoft.Practices.Prism.Commands;

  using Spotify.Data;
  using Spotify.PrismExtensions;
  using Spotify.Services;

  using SpotifyWebApi.Model.Uri;

  using Unity;

  internal class PlaylistTrackItemViewModel : ViewModelWithAttachable<PlaylistTrackData>
  {
    #region Fields

    private readonly PlaybackService playbackService;

    private string propAddedAtString;

    private string propAlbumName;

    private string propArtistsNames;

    private string propDurationString;

    private string propName;

    private ICommand propPlayCommand;

    private string propPlaylistUri;

    private string propUri;

    #endregion

    #region Constructors

    public PlaylistTrackItemViewModel(IUnityContainer container)
    {
      this.playbackService = container.Resolve<PlaybackService>();

      this.ReadingDataModel += this.PlaylistTrackViewModel_ReadingDataModel;

      this.PlayCommand = new DelegateCommand(this.PlayCommandExecute);
    }

    #endregion

    #region Properties

    public string AddedAtString
    {
      get { return this.propAddedAtString; }
      set { this.SetProperty(ref this.propAddedAtString, value); }
    }

    public string AlbumName
    {
      get { return this.propAlbumName; }
      set { this.SetProperty(ref this.propAlbumName, value); }
    }

    public string ArtistsNames
    {
      get { return this.propArtistsNames; }
      set { this.SetProperty(ref this.propArtistsNames, value); }
    }

    public string DurationString
    {
      get { return this.propDurationString; }
      set { this.SetProperty(ref this.propDurationString, value); }
    }

    public string Name
    {
      get { return this.propName; }
      set { this.SetProperty(ref this.propName, value); }
    }

    public ICommand PlayCommand
    {
      get { return this.propPlayCommand; }
      set { this.SetProperty(ref this.propPlayCommand, value); }
    }

    public string PlaylistUri
    {
      get { return this.propPlaylistUri; }
      set { this.SetProperty(ref this.propPlaylistUri, value); }
    }

    public string Uri
    {
      get { return this.propUri; }
      set { this.SetProperty(ref this.propUri, value); }
    }

    #endregion

    #region Methods

    private void PlayCommandExecute()
    {
      this.playbackService.SetSong(SpotifyUri.Make(this.PlaylistUri), SpotifyUri.Make(this.Uri));
    }

    private void PlaylistTrackViewModel_ReadingDataModel(object sender, EventArgs<PlaylistTrackData> e)
    {
      var data = e.Data;
      var playlistTrackTrack = data.PlaylistTrack.Track;

      this.AlbumName = playlistTrackTrack.Album.Name;
      this.ArtistsNames = string.Join(", ", playlistTrackTrack.Artists.Select(o => o.Name));
      this.DurationString = new TimeSpan(0, 0, 0, 0, playlistTrackTrack.DurationMs).ToString(@"m\:ss");
      this.Name = playlistTrackTrack.Name;
      this.AddedAtString = data.PlaylistTrack.AddedAt.ToString("d");
      this.Uri = playlistTrackTrack.Uri;
      this.PlaylistUri = data.PlaylistUri;
    }

    #endregion
  }
}