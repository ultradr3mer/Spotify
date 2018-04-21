using System;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Spotify.Interfaces;
using Spotify.Views;
using SpotifyWebApi.Model;
using SpotifyWebApi.Model.Uri;
using Unity;

namespace Spotify.ViewModels
{
  using Spotify.PrismExtensions;

  internal class PlaylistMenuItemViewModel : ViewModelWithAttachable<SimplePlaylist>
  {
    #region Fields

    private string propUri;
    private string propName;
    private readonly PlaylistPageViewModel playListViewModel;

    #endregion

    #region Constructors

    public PlaylistMenuItemViewModel(IUnityContainer container)
    {
      this.playListViewModel = container.Resolve<PlaylistPageViewModel>();
    }

    public void ActivatePlaylist()
    {
      this.playListViewModel.ActivatePlaylist(SpotifyUri.Make(Uri));
    }

    #endregion

    #region Properties

    public string Uri
    {
      get { return propUri; }
      set { SetProperty(ref propUri, value); }
    }

    public string Name
    {
      get { return propName; }
      set { SetProperty(ref propName, value); }
    }

    #endregion

    #region Methods

    public override string ToString()
    {
      return $"{nameof(PlaylistMenuItemViewModel.Name)}: {Name}, {nameof(PlaylistMenuItemViewModel.Uri)}: {Uri}";
    }


    #endregion
  }
}