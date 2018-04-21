using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Mvvm;
using Spotify.Services;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace Spotify.Views
{
  using Spotify.ViewModels;

  /// <summary>
  /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
  /// </summary>
  public sealed partial class StartupPage : Page, IView
  {
    public StartupPage()
    {
      this.InitializeComponent();

      if (!DesignMode.DesignModeEnabled)
      {
        ViewModelLocator.SetAutoWireViewModel(this, true);
      }
    }

    private StartupPageViewModel ViewModel
    {
      get { return this.DataContext as StartupPageViewModel; }
    }

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);

      this.ViewModel.OnNavigatedTo();
    }

    private void WebView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
    {
      if (args.Uri.Host == ConnectionService.CallbackHost)
      {
        this.ExitStoryboard.Begin();
      }
    }
  }
}
