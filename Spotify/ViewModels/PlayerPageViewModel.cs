﻿namespace Spotify.ViewModels
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

  using Windows.UI.Xaml.Controls;
  using Windows.UI.Xaml.Media.Imaging;

  using SpotifyWebApi.Model.Enum;

  /// <summary>The player page view model.</summary>
  /// <seealso cref="Microsoft.Practices.Prism.Mvvm.ViewModel" />
  internal class PlayerPageViewModel : ViewModel
  {
    #region Fields

    /// <summary>The pause glyph</summary>
    private const char PauseGlyph = (char)Symbol.Pause;

    /// <summary>The play glyph</summary>
    private const char PlayGlyph = (char)Symbol.Play;

    /// <summary>The repeat all glyph.</summary>
    private const char RepeatAllGlyph = (char)Symbol.RepeatAll;

    /// <summary>The repeat all glyph.</summary>
    private const char RepeatOneGlyph = (char)Symbol.RepeatOne;

    /// <summary>The unity container.</summary>
    private readonly IUnityContainer container;

    /// <summary>The playback service.</summary>
    private readonly PlaybackService playbackService;

    /// <summary>Whether the player is playing.</summary>
    private bool isPlaying;

    /// <summary>Whether the view model is reading events. Used to prevent loop back.</summary>
    private bool isReadingEvents;

    /// <summary>A value indicating whether shuffle is active.</summary>
    private bool isShuffleActive;

    /// <summary>The property artists names value.</summary>
    private string propArtistsNames;

    /// <summary>The property currently playing images value.</summary>
    private BitmapImage propCurrentlyPlayingImage;

    /// <summary>The property devices value.</summary>
    private ObservableCollection<DeviceComboBoxItemViewModel> propDevices;

    /// <summary>The property durations value.</summary>
    private int propDuration;

    /// <summary>The property is local device actives value.</summary>
    private bool propIsLocalDeviceActive;

    /// <summary>The property next commands value.</summary>
    private ICommand propNextCommand;

    /// <summary>The property play pause commands value.</summary>
    private ICommand propPlayPauseCommand;

    /// <summary>The <see cref="PlayPauseGlyph" /> property's value.</summary>
    private char propPlayPauseGlyph;

    /// <summary>The property previous commands value.</summary>
    private ICommand propPreviousCommand;

    /// <summary>The property progresss value.</summary>
    private int propProgress;

    /// <summary>The <see cref="ProgressHasFocus" /> property's value.</summary>
    private bool propProgressHasFocus;

    /// <summary>The <see cref="RepeatCommand" /> property's value.</summary>
    private ICommand propRepeatCommand;

    /// <summary>The <see cref="RepeatGlyph" /> property's value.</summary>
    private char propRepeatGlyph;

    /// <summary>The <see cref="RepeatOpacity" /> property's value.</summary>
    private double propRepeatOpacity;

    /// <summary>The property selected devices value.</summary>
    private DeviceComboBoxItemViewModel propSelectedDevice;

    /// <summary>The <see cref="ShuffleCommand" /> property's value.</summary>
    private ICommand propShuffleCommand;

    /// <summary>The <see cref="ShuffleGlyph" /> property's value.</summary>
    private char propShuffleGlyph;

    /// <summary>The <see cref="ShuffleOpacity" /> property's value.</summary>
    private double propShuffleOpacity;

    /// <summary>The property track names value.</summary>
    private string propTrackName;

    /// <summary>The <see cref="Volume" /> property's value.</summary>
    private int propVolume;

    /// <summary>The <see cref="VolumeHasFocus" /> property's value.</summary>
    private bool propVolumeHasFocus;

    /// <summary>The property web player URIs value.</summary>
    private Uri propWebPlayerUri;

    /// <summary>
    /// The repeat state.
    /// </summary>
    private RepeatState repeatState;

    #endregion

    #region Constructors

    /// <summary>Initializes a new instance of the <see cref="PlayerPageViewModel" /> class inside the context of the given container.</summary>
    /// <param name="container">The container.</param>
    public PlayerPageViewModel(IUnityContainer container)
    {
      this.container = container;
      var eventAggregator = container.Resolve<IEventAggregator>();
      eventAggregator.GetEvent<ConnectionEstablishedEvent>().Subscribe(this.HandleConnectionEstablished);
      eventAggregator.GetEvent<DevicesChangedEvent>().Subscribe(this.HandleDevicesChanged);
      eventAggregator.GetEvent<CurrentlyPlayingContextChangedEvent>().Subscribe(this.HandleCurrentlyPlayingContextChanged);
      eventAggregator.GetEvent<CurrentlyPlayingImageChanged>().Subscribe(this.HandleCurrentlyPlayingImageChanged);

      this.PlayPauseCommand = new DelegateCommand(this.PlayPauseCommandExecute);
      this.NextCommand = new DelegateCommand(this.NextCommandExecute);
      this.PreviousCommand = new DelegateCommand(this.PreviousCommandExecute);
      this.ShuffleCommand = new DelegateCommand(this.ShuffleCommandExecute);
      this.RepeatCommand = new DelegateCommand(this.RepeatCommandExecute);

      this.PropertyChanged += this.PlayerPageViewModelPropertyChanged;

      this.playbackService = this.container.Resolve<PlaybackService>();
    }

    #endregion

    #region Properties

    /// <summary>Gets or sets the artists names.</summary>
    public string ArtistsNames
    {
      get { return this.propArtistsNames; }
      set { this.SetProperty(ref this.propArtistsNames, value); }
    }

    /// <summary>Gets or sets the currently playing image.</summary>
    public BitmapImage CurrentlyPlayingImage
    {
      get { return this.propCurrentlyPlayingImage; }
      set { this.SetProperty(ref this.propCurrentlyPlayingImage, value); }
    }

    /// <summary>Gets or sets the devices.</summary>
    public ObservableCollection<DeviceComboBoxItemViewModel> Devices
    {
      get { return this.propDevices; }
      set { this.SetProperty(ref this.propDevices, value); }
    }

    /// <summary>Gets or sets the duration.</summary>
    public int Duration
    {
      get { return this.propDuration; }
      set { this.SetProperty(ref this.propDuration, value); }
    }

    /// <summary>Gets or sets a value indicating whether the local device is active.</summary>
    public bool IsLocalDeviceActive
    {
      get { return this.propIsLocalDeviceActive; }
      set { this.SetProperty(ref this.propIsLocalDeviceActive, value); }
    }

    /// <summary>Gets or sets the next command.</summary>
    public ICommand NextCommand
    {
      get { return this.propNextCommand; }
      set { this.SetProperty(ref this.propNextCommand, value); }
    }

    /// <summary>Gets or sets the play pause command.</summary>
    public ICommand PlayPauseCommand
    {
      get { return this.propPlayPauseCommand; }
      set { this.SetProperty(ref this.propPlayPauseCommand, value); }
    }

    /// <summary>Gets or sets the glyph for the play pause button.</summary>
    public char PlayPauseGlyph
    {
      get { return this.propPlayPauseGlyph; }
      set { this.SetProperty(ref this.propPlayPauseGlyph, value); }
    }

    /// <summary>Gets or sets the previous command.</summary>
    public ICommand PreviousCommand
    {
      get { return this.propPreviousCommand; }
      set { this.SetProperty(ref this.propPreviousCommand, value); }
    }

    /// <summary>Gets or sets the progress.</summary>
    public int Progress
    {
      get { return this.propProgress; }
      set { this.SetProperty(ref this.propProgress, value); }
    }

    /// <summary>Gets or sets a value indicating whether the progress bar has focus.</summary>
    public bool ProgressHasFocus
    {
      get { return this.propProgressHasFocus; }
      set { this.SetProperty(ref this.propProgressHasFocus, value); }
    }

    /// <summary>Gets or sets the repeat command.</summary>
    public ICommand RepeatCommand
    {
      get { return this.propRepeatCommand; }
      set { this.SetProperty(ref this.propRepeatCommand, value); }
    }

    /// <summary>Gets or sets the repeat glyph.</summary>
    public char RepeatGlyph
    {
      get { return this.propRepeatGlyph; }
      set { this.SetProperty(ref this.propRepeatGlyph, value); }
    }

    /// <summary>Gets or sets the repeat opacity.</summary>
    public double RepeatOpacity
    {
      get { return this.propRepeatOpacity; }
      set { this.SetProperty(ref this.propRepeatOpacity, value); }
    }

    /// <summary>Gets or sets the selected device.</summary>
    public DeviceComboBoxItemViewModel SelectedDevice
    {
      get { return this.propSelectedDevice; }
      set { this.SetProperty(ref this.propSelectedDevice, value); }
    }

    /// <summary>Gets or sets the shuffle command.</summary>
    public ICommand ShuffleCommand
    {
      get { return this.propShuffleCommand; }
      set { this.SetProperty(ref this.propShuffleCommand, value); }
    }

    /// <summary>Gets or sets the shuffle opacity.</summary>
    public double ShuffleOpacity
    {
      get { return this.propShuffleOpacity; }
      set { this.SetProperty(ref this.propShuffleOpacity, value); }
    }

    /// <summary>Gets or sets the track name.</summary>
    public string TrackName
    {
      get { return this.propTrackName; }
      set { this.SetProperty(ref this.propTrackName, value); }
    }

    /// <summary>Gets or sets the player volume in percent.</summary>
    public int Volume
    {
      get { return this.propVolume; }
      set { this.SetProperty(ref this.propVolume, value); }
    }

    /// <summary>Gets or sets a value indicating whether the volume has focus.</summary>
    public bool VolumeHasFocus
    {
      get { return this.propVolumeHasFocus; }
      set { this.SetProperty(ref this.propVolumeHasFocus, value); }
    }

    /// <summary>Gets or sets the web player uri.</summary>
    public Uri WebPlayerUri
    {
      get { return this.propWebPlayerUri; }
      set { this.SetProperty(ref this.propWebPlayerUri, value); }
    }

    #endregion

    #region Methods

    /// <summary>Handles the connection established event.</summary>
    /// <param name="data">The data.</param>
    private void HandleConnectionEstablished(ConnectionData data)
    {
      var uri = string.Format("ms-appx-web:///Assets/Index.html?token={0}&client={1}", data.Token.AccessToken, Uri.EscapeUriString(this.playbackService.WebPlayerClientName));
      this.WebPlayerUri = new Uri(uri);

      this.playbackService.StartContinuousUpdate();
    }

    /// <summary>Handles the currently playing context changed event.</summary>
    /// <param name="data">The data.</param>
    private void HandleCurrentlyPlayingContextChanged(CurrentlyPlayingContext data)
    {
      this.isReadingEvents = true;

      this.TrackName = data.Item?.Name;
      this.ArtistsNames = data.Item == null ? null : string.Join(", ", data.Item.Artists.Select(o => o.Name));

      if (!this.ProgressHasFocus)
      {
        if (data.Item != null)
        {
          this.Progress = data.ProgressMs ?? 0;
          this.Duration = data.Item.DurationMs;
        }
        else
        {
          this.Progress = 0;
          this.Duration = 1;
        }
      }

      this.isPlaying = data.IsPlaying;
      this.PlayPauseGlyph = this.isPlaying ? PlayerPageViewModel.PauseGlyph : PlayerPageViewModel.PlayGlyph;

      this.isShuffleActive = data.ShuffleState;
      this.ShuffleOpacity = this.isShuffleActive ? 1.0 : 0.3;

      this.repeatState = string.IsNullOrEmpty(data.RepeatState) ? RepeatState.Off : Enum.Parse<RepeatState>(data.RepeatState, true);
      this.RepeatGlyph = this.repeatState == RepeatState.Track ? PlayerPageViewModel.RepeatOneGlyph : PlayerPageViewModel.RepeatAllGlyph;
      this.RepeatOpacity = this.repeatState == RepeatState.Off ? 0.3 : 1.0;

      this.isReadingEvents = false;
    }

    /// <summary>Handles the currently playing image changed event.</summary>
    /// <param name="data">The data.</param>
    private void HandleCurrentlyPlayingImageChanged(BitmapImage data)
    {
      this.CurrentlyPlayingImage = data;
    }

    /// <summary>Handles the devices changed event.</summary>
    /// <param name="data">The data.</param>
    private void HandleDevicesChanged(DevicesContainer data)
    {
      this.isReadingEvents = true;

      if (data.Devices == null)
      {
        return;
      }

      this.Devices = new ObservableCollection<DeviceComboBoxItemViewModel>(
        data.Devices.Select(
          o =>
            {
              var viewModel = this.container.Resolve<DeviceComboBoxItemViewModel>();
              viewModel.AttachedDataModel = o;
              return viewModel;
            }).ToList());

      this.SelectedDevice = this.Devices.FirstOrDefault(o => o.IsActive);

      if (!this.VolumeHasFocus)
      {
        this.Volume = this.SelectedDevice?.VolumePercent ?? 0;
      }

      this.UpdateIsLocalDeviceActive();

      this.isReadingEvents = false;
    }

    /// <summary>Executes the next command.</summary>
    private void NextCommandExecute()
    {
      this.playbackService.Next();
    }

    /// <summary>Handles the player page view model property changed event.</summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="PropertyChangedEventArgs" /> instance containing the event data.</param>
    private void PlayerPageViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
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
        if (this.IsLocalDeviceActive && (this.SelectedDevice == null || this.SelectedDevice.Name != this.playbackService.WebPlayerClientName))
        {
          this.SelectedDevice = this.Devices.FirstOrDefault(o => o.Name == this.playbackService.WebPlayerClientName);
        }
        else if (!this.IsLocalDeviceActive)
        {
          this.SelectedDevice = this.Devices.FirstOrDefault(o => o.Name != this.playbackService.WebPlayerClientName);
        }
      }

      if (e.PropertyName == "Volume" && !this.isReadingEvents)
      {
        this.playbackService.SetVolume(this.Volume);
      }

      if (e.PropertyName == "Progress" && !this.isReadingEvents)
      {
        this.playbackService.SetProgress(this.Progress);
      }
    }

    /// <summary>Executes the play pause command.</summary>
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

    /// <summary>Executes the previous command.</summary>
    private void PreviousCommandExecute()
    {
      this.playbackService.Previous();
    }

    /// <summary>Executes the repeat command.</summary>
    private void RepeatCommandExecute()
    {
      RepeatState nextRepeatState;

      switch (this.repeatState)
      {
        case RepeatState.Off:
          nextRepeatState = RepeatState.Context;
          break;

        case RepeatState.Context:
          nextRepeatState = RepeatState.Track;
          break;

        default:
          nextRepeatState = RepeatState.Off;
          break;
      }

      this.playbackService.SetRepeat(nextRepeatState);
    }

    /// <summary>Executes the shuffle command.</summary>
    private void ShuffleCommandExecute()
    {
      this.playbackService.SetShuffle(!this.isShuffleActive);
    }

    /// <summary>Updates the is local device active value.</summary>
    private void UpdateIsLocalDeviceActive()
    {
      this.IsLocalDeviceActive = this.SelectedDevice != null && this.SelectedDevice.Name == this.playbackService.WebPlayerClientName;
    }

    #endregion
  }
}