namespace Spotify.Events
{
  using Prism.Events;

  using Windows.UI.Xaml.Media.Imaging;

  /// <summary>The currently playing image changed.</summary>
  internal class CurrentlyPlayingImageChanged : PubSubEvent<BitmapImage>
  {
  }
}