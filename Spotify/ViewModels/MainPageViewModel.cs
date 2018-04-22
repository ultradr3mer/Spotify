namespace Spotify.ViewModels
{
  using System.ComponentModel;
  using System.Linq;

  using Microsoft.Practices.Prism.Mvvm;

  using Prism.Events;

  using Spotify.Data;
  using Spotify.Events;
  using Spotify.Services;

  using Unity;

  /// <summary>The main pages view model.</summary>
  /// <seealso cref="Microsoft.Practices.Prism.Mvvm.ViewModel" />
  internal class MainPageViewModel : ViewModel
  {
    #region Fields

    /// <summary>The unity container.</summary>
    private readonly IUnityContainer container;

    /// <summary>The connection service.</summary>
    private ConnectionService connectionService;

    /// <summary>The property playlists value.</summary>
    private BindingList<PlaylistMenuItemViewModel> propPlaylists;

    #endregion

    #region Constructors

    /// <summary>Initializes a new instance of the <see cref="MainPageViewModel" /> class inside the context of the given container.</summary>
    /// <param name="container">The container.</param>
    public MainPageViewModel(IUnityContainer container)
    {
      this.container = container;

      this.connectionService = container.Resolve<ConnectionService>();

      var eventAggregator = container.Resolve<IEventAggregator>();

      eventAggregator.GetEvent<ConnectionEstablishedEvent>().Subscribe(this.HandleUserChanged);
    }

    #endregion

    #region Properties

    /// <summary>Gets or sets the playlists.</summary>
    public BindingList<PlaylistMenuItemViewModel> Playlists
    {
      get { return this.propPlaylists; }
      set { this.SetProperty(ref this.propPlaylists, value); }
    }

    #endregion

    #region Methods

    /// <summary>Handles the user changed.</summary>
    /// <param name="data">The data.</param>
    private void HandleUserChanged(ConnectionData data)
    {
      this.Playlists = new BindingList<PlaylistMenuItemViewModel>(
        data.Playlists.Select(
          o =>
            {
              var item = this.container.Resolve<PlaylistMenuItemViewModel>();
              item.AttachedDataModel = o;
              return item;
            }).ToList());
    }

    #endregion
  }
}