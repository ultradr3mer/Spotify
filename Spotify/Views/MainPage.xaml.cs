namespace Spotify.Views
{
  using System.ComponentModel;
  using System.Linq;

  using Microsoft.Practices.Prism.Mvvm;

  using Spotify.ViewModels;

  using Windows.ApplicationModel;
  using Windows.UI.Xaml.Controls;

  using Microsoft.Toolkit.Uwp.UI.Animations;

  /// <summary>
  /// The main page.
  /// </summary>
  /// <seealso cref="Windows.UI.Xaml.Controls.Page" />
  /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
  /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
  /// <seealso cref="Microsoft.Practices.Prism.Mvvm.IView" />
  public sealed partial class MainPage : Page, IView
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="MainPage"/> class.
    /// </summary>
    public MainPage()
    {
      this.InitializeComponent();

      if (!DesignMode.DesignModeEnabled)
      {
        ViewModelLocator.SetAutoWireViewModel(this, true);

        this.ViewModel.PropertyChanged += this.ViewModelPropertyChanged;
      }
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the view model.
    /// </summary>
    private MainPageViewModel ViewModel
    {
      get { return this.DataContext as MainPageViewModel; }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Called when an item of the navigation view was invoked.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The <see cref="NavigationViewItemInvokedEventArgs"/> instance containing the event data.</param>
    private void NavigationViewItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
      if (args.IsSettingsInvoked)
      {
        this.ContentFrame.Navigate(typeof(SettingsPage));
      }
      else
      {
        // find NavigationViewItem with Content that equals InvokedItem
        var item = sender.MenuItems.OfType<NavigationViewItem>().First(x => (string)x.Content == (string)args.InvokedItem);
        this.Navigate(item);
      }
    }

    /// <summary>
    /// Navigates the to the view model of the specified item.
    /// </summary>
    /// <param name="item">The item.</param>
    private void Navigate(NavigationViewItem item)
    {
      switch (item.Tag)
      {
        case "Playlist":
          this.ContentFrame.Navigate(typeof(PlaylistPage));
          break;

        case "Startup":
          this.ContentFrame.Navigate(typeof(StartupPage));
          break;
      }
    }

    /// <summary>
    /// Called when a property of the view model was changed.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
    private async void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == "Playlists")
      {
        this.MainNavigation.MenuItems.Clear();

        foreach (var playList in this.ViewModel.Playlists)
        {
          var item = new PlaylistMenuItem
                       {
                         DataContext = playList
                       };
          this.MainNavigation.MenuItems.Add(item);
        }
      }
      else if (e.PropertyName == "HasPlaybackState")
      {
        await this.ZoomPlayerIn.BeginAsync();
      }
    }

    #endregion
  }
}