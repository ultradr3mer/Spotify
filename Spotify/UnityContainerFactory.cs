namespace Spotify
{
  using Prism.Events;

  using Spotify.Services;
  using Spotify.ViewModels;

  using Unity;
  using Unity.Lifetime;

  internal class UnityContainerFactory
  {
    #region Methods

    public static IUnityContainer Create()
    {
      var container = new UnityContainer();

      ContainerControlledLifetimeManager Clm()
      {
        return new ContainerControlledLifetimeManager();
      }

      container.RegisterType<ConnectionService>(Clm());
      container.RegisterType<PlaylistService>(Clm());
      container.RegisterType<SettingsService>(Clm());
      container.RegisterType<PlaybackService>(Clm());

      container.RegisterType<PlaylistPageViewModel>(Clm());
      container.RegisterType<PlayerPageViewModel>(Clm());
      container.RegisterType<SettingsPageViewModel>(Clm());
      container.RegisterType<StartupPageViewModel>(Clm());

      container.RegisterType<IEventAggregator, EventAggregator>(Clm());

      container.Resolve<SettingsPageViewModel>();
      container.Resolve<ConnectionService>();

      return container;
    }

    #endregion
  }
}