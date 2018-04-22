namespace Spotify.Events
{
  using Prism.Events;

  using Spotify.Data;

  /// <summary>The settings changed event.</summary>
  internal class SettingsChangedEvent : PubSubEvent<SettingsData>
  {
  }
}