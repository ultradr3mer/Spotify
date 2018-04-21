namespace Spotify.Events
{
  using Prism.Events;

  using Windows.UI.Xaml.Media.Imaging;

  internal class PlaylistImageChangedEvent : PubSubEvent<BitmapImage>
  {
  }
}