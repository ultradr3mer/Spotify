namespace Spotify.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Linq;

  using global::Prism.Events;

  using Spotify.Data;
  using Spotify.Events;
  using Spotify.Services;

  using SpotifyWebApi.Model;
  using SpotifyWebApi.Model.Uri;

  using Unity;

  using Windows.UI.Xaml.Media.Imaging;

  using Spotify.PrismExtensions;

  internal class PlaylistPageViewModel : ViewModelWithAttachable<FullPlaylist>
  {
    #region Fields

    private readonly IUnityContainer container;

    private readonly PlaylistService playListService;

    private BitmapImage propImage;

    private string propInfo;

    private string propName;

    private ObservableCollection<PlaylistTrackItemViewModel> propTrackViewModels =
      new ObservableCollection<PlaylistTrackItemViewModel>();

    private string propUri;

    #endregion

    #region Constructors

    public PlaylistPageViewModel(IUnityContainer container)
    {
      this.container = container;
      this.playListService = container.Resolve<PlaylistService>();

      var eventAggregator = container.Resolve<IEventAggregator>();
      eventAggregator.GetEvent<PlaylistChangedEvent>().Subscribe(this.HandlePlaylistChanged);
      eventAggregator.GetEvent<PlaylistItemsAddedEvent>().Subscribe(this.HandlePlaylistItemsAdded);
      eventAggregator.GetEvent<PlaylistImageChangedEvent>().Subscribe(this.HandlePlaylistImageChanged);

      this.ReadingDataModel += this.PlaylistPageViewModel_ReadingDataModel;
      this.NullingDataModel += this.PlaylistPageViewModel_NullingDataModel;
    }

    #endregion

    #region Properties

    public BitmapImage Image
    {
      get { return this.propImage; }

      set { this.SetProperty(ref this.propImage, value); }
    }

    public string Info
    {
      get { return this.propInfo; }

      set { this.SetProperty(ref this.propInfo, value); }
    }

    public string Name
    {
      get { return this.propName; }

      set { this.SetProperty(ref this.propName, value); }
    }

    public ObservableCollection<PlaylistTrackItemViewModel> TrackViewModels
    {
      get { return this.propTrackViewModels; }

      set { this.SetProperty(ref this.propTrackViewModels, value); }
    }

    public string Uri
    {
      get { return this.propUri; }

      set { this.SetProperty(ref this.propUri, value); }
    }

    #endregion

    #region Methods

    public void ActivatePlaylist(SpotifyUri uri)
    {
      this.AttachedDataModel = null;
      this.playListService.SetPlaylist(uri);
    }

    private void HandlePlaylistChanged(FullPlaylist playlist)
    {
      this.AttachedDataModel = playlist;
    }

    private void HandlePlaylistImageChanged(BitmapImage image)
    {
      this.Image = image;
    }

    private void HandlePlaylistItemsAdded(IEnumerable<PlaylistTrack> tracks)
    {
      foreach (var playlistTrack in tracks)
      {
        var viewModel = this.CreatePlaylistTrackItemViewModel(playlistTrack);
        this.TrackViewModels.Add(viewModel);
      }
    }

    private void PlaylistPageViewModel_NullingDataModel(object sender, EventArgs e)
    {
      this.TrackViewModels.Clear();
      this.Info = string.Empty;
      this.Name = null;
      this.Image = null;
      this.Uri = null;
    }

    private void PlaylistPageViewModel_ReadingDataModel(object sender, EventArgs<FullPlaylist> e)
    {
      var data = e.Data;

      var ownerName = data.Owner.Id;
      var songCount = data.Tracks.Total;
      this.Info = $"Created by {ownerName} • {songCount} Songs.";

      this.TrackViewModels = new ObservableCollection<PlaylistTrackItemViewModel>(
        data.Tracks.Items.Select(this.CreatePlaylistTrackItemViewModel).ToList());
    }

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

    #endregion
  }
}