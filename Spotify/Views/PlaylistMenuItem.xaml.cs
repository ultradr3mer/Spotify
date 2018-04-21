using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Spotify.ViewModels;

// Die Elementvorlage "Benutzersteuerelement" wird unter https://go.microsoft.com/fwlink/?LinkId=234236 dokumentiert.

namespace Spotify.Views
{
  public sealed partial class PlaylistMenuItem : NavigationViewItem
  {
    public PlaylistMenuItem()
    {
      this.InitializeComponent();
    }

    private PlaylistMenuItemViewModel ViewModel
    {
      get { return (PlaylistMenuItemViewModel) this.DataContext; }
    }

    private void PlaylistMenuItem_OnTapped(object sender, TappedRoutedEventArgs e)
    {
      this.ViewModel.ActivatePlaylist();
    }
  }
}
