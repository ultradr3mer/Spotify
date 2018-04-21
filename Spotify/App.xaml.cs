using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Mvvm;
using Unity;

namespace Spotify
{
  /// <summary>
  /// Stellt das anwendungsspezifische Verhalten bereit, um die Standardanwendungsklasse zu ergänzen.
  /// </summary>
  sealed partial class App : MvvmAppBase
  {
    private IUnityContainer container = UnityContainerFactory.Create();

    /// <summary>
    /// Initialisiert das Singletonanwendungsobjekt. Dies ist die erste Zeile von erstelltem Code
    /// und daher das logische Äquivalent von main() bzw. WinMain().
    /// </summary>
    public App()
    {
      this.InitializeComponent();
    }

    protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
    {
      NavigationService.Navigate("Main", null);
      return Task.CompletedTask;
    }

    protected override Task OnInitializeAsync(IActivatedEventArgs args)
    {
      ViewModelLocationProvider.SetDefaultViewModelFactory(t => container.Resolve(t));
      ApplicationLanguages.PrimaryLanguageOverride = "en-US";
      return Task.CompletedTask;
    }
  }
}
