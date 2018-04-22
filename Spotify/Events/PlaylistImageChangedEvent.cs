namespace Spotify.Events
{
  using Prism.Events;

  using Windows.UI.Xaml.Media.Imaging;

  /// <summary>The playlist image changed event.</summary>
  internal class PlaylistImageChangedEvent : PubSubEvent<BitmapImage>
  {
  }
}