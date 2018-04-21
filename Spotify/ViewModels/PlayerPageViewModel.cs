namespace Spotify.ViewModels
{
  using System;
  using System.Collections.ObjectModel;
  using System.ComponentModel;
  using System.Linq;
  using System.Windows.Input;

  using Microsoft.Practices.Prism.Mvvm;

  using Prism.Commands;
  using Prism.Events;

  using Spotify.Data;
  using Spotify.Events;
  using Spotify.Services;

  using SpotifyWebApi.Model;

  using Unity;

  using Windows.UI.Xaml.Media.Imaging;

  internal class PlayerPageViewModel : ViewModel
  {
    #region Fields

    private readonly IUnityContainer container;

    private readonly PlaybackService playbackService;

    private bool isPlaying;

    private bool isReadingEvents;

    private string propArtistsNames;

    private BitmapImage propCurrentlyPlayingImage;

    private ObservableCollection<DeviceComboboxItemViewModel> propDevices;

    private int propDuration;

    private bool propIsLocalDeviceActive;

    private ICommand propNextCommand;

    private ICommand propPlayPauseCommand;

    private ICommand propPreviousCommand;

    private int propProgress;

    private DeviceComboboxItemViewModel propSelectedDevice;

    private string propTrackName;

    private Uri propWebPlayerUri;

    #endregion

    #region Constructors

    public PlayerPageViewModel(IUnityContainer container)
    {
      this.container = container;
      var eventAggregator = container.Resolve<IEventAggregator>();
      eventAggregator.GetEvent<ConnectionEstablishedEvent>().Subscribe(this.HandleConnectionEstablished);
      eventAggregator.GetEvent<DevicesChangedEvent>().Subscribe(this.HandleDevicesChanged);
      eventAggregator.GetEvent<CurrentlyPlayingChangedEvent>().Subscribe(this.HandleCurrentlyPlayingChanged);
      eventAggregator.GetEvent<CurrentlyPlayingImageChanged>().Subscribe(this.HandleCurrentlyPlayingImageChanged);

      this.PlayPauseCommand = new DelegateCommand(this.PlayPauseCommandExecute);
      this.NextCommand = new DelegateCommand(this.NextCommandExecute);
      this.PreviousCommand = new DelegateCommand(this.PreviousCommandExecute);

      this.PropertyChanged += this.PlayerPageViewModel_PropertyChanged;

      this.playbackService = this.container.Resolve<PlaybackService>();
    }

    #endregion

    #region Properties

    public string ArtistsNames
    {
      get { return this.propArtistsNames; }
      set { this.SetProperty(ref this.propArtistsNames, value); }
    }

    public BitmapImage CurrentlyPlayingImage
    {
      get { return this.propCurrentlyPlayingImage; }
      set { this.SetProperty(ref this.propCurrentlyPlayingImage, value); }
    }

    public ObservableCollection<DeviceComboboxItemViewModel> Devices
    {
      get { return this.propDevices; }
      set { this.SetProperty(ref this.propDevices, value); }
    }

    public int Duration
    {
      get { return this.propDuration; }
      set { this.SetProperty(ref this.propDuration, value); }
    }

    public bool IsLocalDeviceActive
    {
      get { return this.propIsLocalDeviceActive; }
      set { this.SetProperty(ref this.propIsLocalDeviceActive, value); }
    }

    public ICommand NextCommand
    {
      get { return this.propNextCommand; }
      set { this.SetProperty(ref this.propNextCommand, value); }
    }

    public ICommand PlayPauseCommand
    {
      get { return this.propPlayPauseCommand; }
      set { this.SetProperty(ref this.propPlayPauseCommand, value); }
    }

    public ICommand PreviousCommand
    {
      get { return this.propPreviousCommand; }
      set { this.SetProperty(ref this.propPreviousCommand, value); }
    }

    public int Progress
    {
      get { return this.propProgress; }
      set { this.SetProperty(ref this.propProgress, value); }
    }

    public DeviceComboboxItemViewModel SelectedDevice
    {
      get { return this.propSelectedDevice; }
      set { this.SetProperty(ref this.propSelectedDevice, value); }
    }

    public string TrackName
    {
      get { return this.propTrackName; }
      set { this.SetProperty(ref this.propTrackName, value); }
    }

    public Uri WebPlayerUri
    {
      get { return this.propWebPlayerUri; }
      set { this.SetProperty(ref this.propWebPlayerUri, value); }
    }

    #endregion

    #region Methods

    private void HandleConnectionEstablished(ConnectionData data)
    {
      var uri = string.Format(
        "ms-appx-web:///Assets/Index.html?token={0}&client={1}",
        data.Token.AccessToken,
        Uri.EscapeUriString(this.playbackService.WebPlayerClientName));
      this.WebPlayerUri = new Uri(uri);

      this.playbackService.StartContinuousUpdate();
    }

    private void HandleCurrentlyPlayingChanged(CurrentlyPlaying data)
    {
      this.isReadingEvents = true;

      this.TrackName = data.Item?.Name;
      this.ArtistsNames = data.Item == null ? null : string.Join(", ", data.Item.Artists.Select(o => o.Name));

      if (data.Item != null)
      {
        this.Progress = data.ProgressMs;
        this.Duration = data.Item.DurationMs;
      }
      else
      {
        this.Progress = 0;
        this.Duration = 1;
      }

      this.isPlaying = data.IsPlaying;

      this.isReadingEvents = false;
    }

    private void HandleCurrentlyPlayingImageChanged(BitmapImage data)
    {
      this.CurrentlyPlayingImage = data;
    }

    private void HandleDevicesChanged(DevicesContainer data)
    {
      this.isReadingEvents = true;

      if (data.Devices == null)
      {
        return;
      }

      this.Devices = new ObservableCollection<DeviceComboboxItemViewModel>(
        data.Devices.Select(
          o =>
            {
              var viewModel = this.container.Resolve<DeviceComboboxItemViewModel>();
              viewModel.AttachedDataModel = o;
              return viewModel;
            }).ToList());

      this.SelectedDevice = this.Devices.FirstOrDefault(o => o.IsActive);

      this.UpdateIsLocalDeviceActive();

      this.isReadingEvents = false;
    }

    private void NextCommandExecute()
    {
      this.playbackService.Next();
    }

    private void PlayerPageViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == "SelectedDevice" && !this.isReadingEvents)
      {
        if (this.SelectedDevice != null && !this.SelectedDevice.IsActive)
        {
          this.playbackService.ActivateDevice(this.SelectedDevice.Id);
        }
      }

      if (e.PropertyName == "IsLocalDeviceActive" && !this.isReadingEvents)
      {
        if (this.IsLocalDeviceActive && (this.SelectedDevice == null
                                         || this.SelectedDevice.Name != this.playbackService.WebPlayerClientName))
        {
          this.SelectedDevice = this.Devices.FirstOrDefault(o => o.Name == this.playbackService.WebPlayerClientName);
        }
        else if (!this.IsLocalDeviceActive)
        {
          this.SelectedDevice = this.Devices.FirstOrDefault(o => o.Name != this.playbackService.WebPlayerClientName);
        }
      }

      if (e.PropertyName == "Progress" && !this.isReadingEvents)
      {
        this.playbackService.SetProgress(this.Progress);
      }
    }

    private void PlayPauseCommandExecute()
    {
      if (this.isPlaying)
      {
        this.playbackService.Pause();
      }
      else
      {
        this.playbackService.Resume();
      }
    }

    private void PreviousCommandExecute()
    {
      this.playbackService.Previous();
    }

    private void UpdateIsLocalDeviceActive()
    {
      this.IsLocalDeviceActive = this.SelectedDevice != null
                                 && this.SelectedDevice.Name == this.playbackService.WebPlayerClientName;
    }

    #endregion
  }
}