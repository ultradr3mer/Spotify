namespace Spotify.ViewModels
{
  using System;
  using System.Linq;
  using System.Windows.Input;

  using Microsoft.Practices.Prism.Commands;

  using Prism.Events;

  using Spotify.Data;
  using Spotify.Events;
  using Spotify.PrismExtensions;
  using Spotify.Services;

  using SpotifyWebApi.Model;
  using SpotifyWebApi.Model.Uri;

  using Unity;

  /// <summary>The playlist track item view model.</summary>
  /// <seealso cref="Spotify.PrismExtensions.ViewModelWithAttachable{Spotify.Data.PlaylistTrackData}" />
  internal class PlaylistTrackItemViewModel : ViewModelWithAttachable<PlaylistTrackData>
  {
    #region Fields

    /// <summary>The playback service.</summary>
    private readonly PlaybackService playbackService;

    /// <summary>The property added at strings value.</summary>
    private string propAddedAtString;

    /// <summary>The property album names value.</summary>
    private string propAlbumName;

    /// <summary>The property artists names value.</summary>
    private string propArtistsNames;

    /// <summary>The property duration strings value.</summary>
    private string propDurationString;

    /// <summary>The <see cref="IsSelected" /> property's value.</summary>
    private bool propIsSelected;

    /// <summary>The property names value.</summary>
    private string propName;

    /// <summary>The property play commands value.</summary>
    private ICommand propPlayCommand;

    /// <summary>The property playlist URIs value.</summary>
    private string propPlaylistUri;

    /// <summary>The property URIs value.</summary>
    private string propUri;

    #endregion

    #region Constructors

    /// <summary>Initializes a new instance of the <see cref="PlaylistTrackItemViewModel" /> class inside the context of the given container.</summary>
    /// <param name="container">The container.</param>
    public PlaylistTrackItemViewModel(IUnityContainer container)
    {
      this.playbackService = container.Resolve<PlaybackService>();

      this.ReadingDataModel += this.ReadDataModel;

      this.PlayCommand = new DelegateCommand(this.PlayCommandExecute);

      var eventAggregator = container.Resolve<IEventAggregator>();
      eventAggregator.GetEvent<CurrentlyPlayingContextChangedEvent>().Subscribe(this.HandleCurrentlyPlayingContextChanged);
    }

    #endregion

    #region Properties

    /// <summary>Gets or sets the added at date as string.</summary>
    public string AddedAtString
    {
      get { return this.propAddedAtString; }
      set { this.SetProperty(ref this.propAddedAtString, value); }
    }

    /// <summary>Gets or sets the album name.</summary>
    public string AlbumName
    {
      get { return this.propAlbumName; }
      set { this.SetProperty(ref this.propAlbumName, value); }
    }

    /// <summary>Gets or sets the artists names.</summary>
    public string ArtistsNames
    {
      get { return this.propArtistsNames; }
      set { this.SetProperty(ref this.propArtistsNames, value); }
    }

    /// <summary>Gets or sets the duration as string.</summary>
    public string DurationString
    {
      get { return this.propDurationString; }
      set { this.SetProperty(ref this.propDurationString, value); }
    }

    /// <summary>Gets or sets a value indicating whether the track item is selected.</summary>
    public bool IsSelected
    {
      get { return this.propIsSelected; }
      set { this.SetProperty(ref this.propIsSelected, value); }
    }

    /// <summary>Gets or sets the name.</summary>
    public string Name
    {
      get { return this.propName; }
      set { this.SetProperty(ref this.propName, value); }
    }

    /// <summary>Gets or sets the play command.</summary>
    public ICommand PlayCommand
    {
      get { return this.propPlayCommand; }
      set { this.SetProperty(ref this.propPlayCommand, value); }
    }

    /// <summary>Gets or sets the playlist uri.</summary>
    public string PlaylistUri
    {
      get { return this.propPlaylistUri; }
      set { this.SetProperty(ref this.propPlaylistUri, value); }
    }

    /// <summary>Gets or sets the uri.</summary>
    public string Uri
    {
      get { return this.propUri; }
      set { this.SetProperty(ref this.propUri, value); }
    }

    #endregion

    #region Methods

    /// <summary>Handles the currently playing context changed event.</summary>
    /// <param name="data">The object.</param>
    private void HandleCurrentlyPlayingContextChanged(CurrentlyPlayingContext data)
    {
      this.IsSelected = data.Context.Uri == this.PlaylistUri && data.Item.Uri == this.Uri;
    }

    /// <summary>Executes the play command.</summary>
    private void PlayCommandExecute()
    {
      this.playbackService.SetSong(SpotifyUri.Make(this.PlaylistUri), SpotifyUri.Make(this.Uri));
    }

    /// <summary>Reads the data model.</summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="EventArgs{PlaylistTrackData}" /> instance containing the event data.</param>
    private void ReadDataModel(object sender, EventArgs<PlaylistTrackData> e)
    {
      var data = e.Payload;
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