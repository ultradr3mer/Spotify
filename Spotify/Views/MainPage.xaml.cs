using System.Linq;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.StoreApps;
using Spotify.Interfaces;
using Spotify.ViewModels;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x407 dokumentiert.

namespace Spotify.Views
{
  /// <summary>
  /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
  /// </summary>
  public sealed partial class MainPage : Page, IView
  {
    public MainPage()
    {
      this.InitializeComponent();

      if (!DesignMode.DesignModeEnabled)
      {
        ViewModelLocator.SetAutoWireViewModel(this, true);

        this.ViewModel.PropertyChanged += ViewModel_PropertyChanged;
      }
    }

    private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      if (e.PropertyName == "Playlists")
      {
        this.MainNavigation.MenuItems.Clear();

        foreach (var playList in this.ViewModel.Playlists)
        {
          var item = new PlaylistMenuItem {DataContext = playList};
          this.MainNavigation.MenuItems.Add(item);
        }
      }
    }

    private MainPageViewModel ViewModel
    {
      get { return this.DataContext as MainPageViewModel; }
    }

    private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
      if (args.IsSettingsInvoked)
      {
        ContentFrame.Navigate(typeof(SettingsPage));
      }
      else
      {
        // find NavigationViewItem with Content that equals InvokedItem
        var item = sender.MenuItems.OfType<NavigationViewItem>().First(x => (string)x.Content == (string)args.InvokedItem);
        NavView_Navigate(item);
      }
    }

    private void NavView_Navigate(NavigationViewItem item)
    {
      switch (item.Tag)
      {
        case "Playlist":
          ContentFrame.Navigate(typeof(PlaylistPage));
          break;

        case "Startup":
          ContentFrame.Navigate(typeof(StartupPage));
          break;
      }
    }
  }
}
