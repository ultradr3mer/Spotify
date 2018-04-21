using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify.ViewModels
{
  using Prism.Events;

  using Spotify.Data;
  using Spotify.Events;
  using Spotify.PrismExtensions;
  using Spotify.Services;

  using Unity;

  class SettingsPageViewModel : ViewModelWithAttachable<SettingsData>
  {
    public SettingsPageViewModel(IUnityContainer container)
    {
      var eventAggregator = container.Resolve<IEventAggregator>();
      eventAggregator.GetEvent<SettingsChangedEvent>().Subscribe(this.HandleSettingsChanged);

      this.settingsService = container.Resolve<SettingsService>();
    }

    private void HandleSettingsChanged(SettingsData data)
    {
      this.AttachedDataModel = data;
    }

    private string propClientId;

    internal void OnNavigatedFrom()
    {
      this.settingsService.SetSettings(this.AttachedDataModel);
    }

    public string ClientId
    {
      get { return propClientId; }
      set { this.SetProperty(ref propClientId, value); }
    }

    private string propClientSecret;

    private SettingsService settingsService;

    public string ClientSecret
    {
      get { return propClientSecret; }
      set { this.SetProperty(ref propClientSecret, value); }
    }
  }
}
