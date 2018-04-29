namespace Spotify.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
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

  using Windows.UI.Xaml.Media.Imaging;

  /// <summary>The playlist page view model.</summary>
  /// <seealso cref="Spotify.PrismExtensions.ViewModelWithAttachable{SpotifyWebApi.Model.FullPlaylist}" />
  internal class PlaylistPageViewModel : ViewModelWithAttachable<FullPlaylist>
  {
    #region Fields

    /// <summary>The unity container.</summary>
    private readonly IUnityContainer container;

    /// <summary>The play list service.</summary>
    private readonly PlaylistService playListService;

    /// <summary>The property images value.</summary>
    private BitmapImage propImage;

    /// <summary>The property informations value.</summary>
    private string propInfo;

    /// <summary>The property names value.</summary>
    private string propName;

    /// <summary>The <see cref="PlayCommand" /> property's value.</summary>
    private ICommand propPlayCommand;

    /// <summary>The property track view models value.</summary>
    private ObservableCollection<PlaylistTrackItemViewModel> propTrackViewModels = new ObservableCollection<PlaylistTrackItemViewModel>();

    /// <summary>The property URIs value.</summary>
    private string propUri;

    private PlaybackService playBackService;

    #endregion

    #region Constructors

    /// <summary>Initializes a new instance of the <see cref="PlaylistPageViewModel" /> class.</summary>
    /// <param name="container">The container.</param>
    public PlaylistPageViewModel(IUnityContainer container)
    {
      this.container = container;
      this.playListService = container.Resolve<PlaylistService>();
      this.playBackService = container.Resolve<PlaybackService>();

      var eventAggregator = container.Resolve<IEventAggregator>();
      eventAggregator.GetEvent<PlaylistChangedEvent>().Subscribe(this.HandlePlaylistChanged);
      eventAggregator.GetEvent<PlaylistItemsAddedEvent>().Subscribe(this.HandlePlaylistItemsAdded);
      eventAggregator.GetEvent<PlaylistImageChangedEvent>().Subscribe(this.HandlePlaylistImageChanged);

      this.ReadingDataModel += this.ReadDataModel;
      this.NullingDataModel += this.NullDataModel;

      this.PlayCommand = new DelegateCommand(this.PlayCommandExecute);
    }

    /// <summary>
    /// Executes the play command.
    /// </summary>
    private void PlayCommandExecute()
    {
      this.playBackService.SetPlaylist(SpotifyUri.Make(this.Uri));
    }

    #endregion

    #region Properties

    /// <summary>Gets or sets the image.</summary>
    public BitmapImage Image
    {
      get { return this.propImage; }

      set { this.SetProperty(ref this.propImage, value); }
    }

    /// <summary>Gets or sets the info.</summary>
    public string Info
    {
      get { return this.propInfo; }

      set { this.SetProperty(ref this.propInfo, value); }
    }

    /// <summary>Gets or sets the name.</summary>
    public string Name
    {
      get { return this.propName; }

      set { this.SetProperty(ref this.propName, value); }
    }

    /// <summary>Gets or sets the playCommand.</summary>
    public ICommand PlayCommand
    {
      get { return this.propPlayCommand; }
      set { this.SetProperty(ref this.propPlayCommand, value); }
    }

    /// <summary>Gets or sets the track view models.</summary>
    public ObservableCollection<PlaylistTrackItemViewModel> TrackViewModels
    {
      get { return this.propTrackViewModels; }

      set { this.SetProperty(ref this.propTrackViewModels, value); }
    }

    /// <summary>Gets or sets the uri.</summary>
    public string Uri
    {
      get { return this.propUri; }

      set { this.SetProperty(ref this.propUri, value); }
    }

    #endregion

    #region Methods

    /// <summary>Activates the playlist.</summary>
    /// <param name="uri">The URI of the playlist to activate.</param>
    public void ActivatePlaylist(SpotifyUri uri)
    {
      this.AttachedDataModel = null;
      this.playListService.SetPlaylist(uri);
    }

    /// <summary>Creates a playlist track item view model.</summary>
    /// <param name="playlistTrack">The playlist track.</param>
    /// <returns>the view model.</returns>
    private PlaylistTrackItemViewModel CreatePlaylistTrackItemViewModel(PlaylistTrack playlistTrack)
    {
      var viewModel = this.container.Resolve<PlaylistTrackItemViewModel>();
      viewModel.AttachedDataModel = new PlaylistTrackData
                                      {
                                        PlaylistTrack = playlistTrack,
                                        PlaylistUri = this.Uri
                                      };
      return viewModel;
    }

    /// <summary>Handles the playlist changed event.</summary>
    /// <param name="playlist">The playlist.</param>
    private void HandlePlaylistChanged(FullPlaylist playlist)
    {
      this.AttachedDataModel = playlist;
    }

    /// <summary>Handles the playlist image changed event.</summary>
    /// <param name="image">The image.</param>
    private void HandlePlaylistImageChanged(BitmapImage image)
    {
      this.Image = image;
    }

    /// <summary>Handles the playlist items added event.</summary>
    /// <param name="tracks">The tracks.</param>
    private void HandlePlaylistItemsAdded(IEnumerable<PlaylistTrack> tracks)
    {
      foreach (var playlistTrack in tracks)
      {
        var viewModel = this.CreatePlaylistTrackItemViewModel(playlistTrack);
        this.TrackViewModels.Add(viewModel);
      }
    }

    /// <summary>Nulls the data model.</summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
    private void NullDataModel(object sender, EventArgs e)
    {
      this.TrackViewModels.Clear();
      this.Info = string.Empty;
      this.Name = null;
      this.Image = null;
      this.Uri = null;
    }

    /// <summary>Reads the data model.</summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="EventArgs{FullPlaylist}" /> instance containing the event data.</param>
    private void ReadDataModel(object sender, EventArgs<FullPlaylist> e)
    {
      var data = e.Payload;

      var ownerName = data.Owner.Id;
      var songCount = data.Tracks.Total;
      this.Info = $"Created by {ownerName} • {songCount} Songs.";

      this.TrackViewModels = new ObservableCollection<PlaylistTrackItemViewModel>(data.Tracks.Items.Select(this.CreatePlaylistTrackItemViewModel).ToList());
    }

    #endregion
  }
}