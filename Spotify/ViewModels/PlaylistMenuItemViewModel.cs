namespace Spotify.ViewModels
{
  using Spotify.PrismExtensions;

  using SpotifyWebApi.Model;
  using SpotifyWebApi.Model.Uri;

  using Unity;

  /// <summary>The playlist menu item view model.</summary>
  /// <seealso cref="Spotify.PrismExtensions.ViewModelWithAttachable{SpotifyWebApi.Model.SimplePlaylist}" />
  internal class PlaylistMenuItemViewModel : ViewModelWithAttachable<SimplePlaylist>
  {
    #region Fields

    /// <summary>The play list view model.</summary>
    private readonly PlaylistPageViewModel playListViewModel;

    /// <summary>The property names value.</summary>
    private string propName;

    /// <summary>The property URIs value.</summary>
    private string propUri;

    #endregion

    #region Constructors

    /// <summary>Initializes a new instance of the <see cref="PlaylistMenuItemViewModel" /> class inside the context of the given container.</summary>
    /// <param name="container">The container.</param>
    public PlaylistMenuItemViewModel(IUnityContainer container)
    {
      this.playListViewModel = container.Resolve<PlaylistPageViewModel>();
    }

    #endregion

    #region Properties

    /// <summary>Gets or sets the name.</summary>
    public string Name
    {
      get { return this.propName; }
      set { this.SetProperty(ref this.propName, value); }
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
    public void ActivatePlaylist()
    {
      this.playListViewModel.ActivatePlaylist(SpotifyUri.Make(this.Uri));
    }

    /// <summary>Returns a <see cref="System.String" /> that represents this instance.</summary>
    /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
    public override string ToString()
    {
      return $"{nameof(PlaylistMenuItemViewModel.Name)}: {this.Name}, {nameof(PlaylistMenuItemViewModel.Uri)}: {this.Uri}";
    }

    #endregion
  }
}