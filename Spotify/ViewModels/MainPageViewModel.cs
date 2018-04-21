using System.ComponentModel;
using System.Linq;
using Microsoft.Practices.Prism.Mvvm;
using Prism.Events;
using Spotify.Data;
using Spotify.Events;
using Spotify.Services;
using Unity;

namespace Spotify.ViewModels
{
  internal class MainPageViewModel : ViewModel
  {
    #region Fields

    private ConnectionService connectionService;

    private BindingList<PlaylistMenuItemViewModel> propPlaylists;
    private IUnityContainer container;

    #endregion

    #region Properties

    public BindingList<PlaylistMenuItemViewModel> Playlists
    {
      get { return propPlaylists; }
      set { SetProperty(ref propPlaylists, value); }
    }

    #endregion

    #region Constructors

    public MainPageViewModel(IUnityContainer container)
    {
      this.container = container;

      connectionService = container.Resolve<ConnectionService>();

      var eventAggregator = container.Resolve<IEventAggregator>();

      eventAggregator.GetEvent<ConnectionEstablishedEvent>().Subscribe(this.HandleUserChanged);
    }

    #endregion

    #region Methods

    private void HandleUserChanged(ConnectionData data)
    {
      Playlists = new BindingList<PlaylistMenuItemViewModel>(data.Playlists.Select(o =>
      {
        var item = this.container.Resolve<PlaylistMenuItemViewModel>();
        item.AttachedDataModel = o;
        return item;
      }).ToList());
    }

    #endregion
  }
}