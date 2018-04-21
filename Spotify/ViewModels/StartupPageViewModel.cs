using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;
using Spotify.Services;

namespace Spotify.ViewModels
{
  using Windows.UI.Xaml.Navigation;

  using Prism.Events;

  using Spotify.Data;
  using Spotify.Events;

  using Unity;

  class StartupPageViewModel : ViewModel
  {
    private Uri propConnectUrl;

    private ConnectionService connectionService;

    private SettingsData settings;

    public Uri ConnectUrl
    {
      get { return propConnectUrl; }
      set { this.SetProperty(ref propConnectUrl, value); }
    }
    public StartupPageViewModel(IUnityContainer container)
    {
      var eventAggregator = container.Resolve<IEventAggregator>();
      eventAggregator.GetEvent<ConnectionUriChangedEvent>().Subscribe(this.HandleConnectionUriChanged);
      eventAggregator.GetEvent<SettingsChangedEvent>().Subscribe(this.HandleSettingsChanged);

      this.ConnectUrl = new Uri("ms-appx-web:///Assets/SettingsInfo.html");

      var settingsService = container.Resolve<SettingsService>();
      settingsService.ReadSettings();

      this.connectionService = container.Resolve<ConnectionService>();
      this.connectionService.TryInitializeConnection(this.settings.ClientId, this.settings.ClientSecret);
    }

    internal void OnNavigatedTo()
    {
      this.connectionService.TryInitializeConnection(this.settings.ClientId, this.settings.ClientSecret);
    }

    private void HandleSettingsChanged(SettingsData settings)
    {
      this.settings = settings;
    }

    private void HandleConnectionUriChanged(Uri uri)
    {
      this.ConnectUrl = uri;
    }
  }
}
