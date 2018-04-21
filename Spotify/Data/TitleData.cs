using System;

namespace Spotify.Daten
{
    public class TitleData
    {
      public string Titel { get; set; }
      public string Kuenstler { get; set; }
      public string Album { get; set; }
      public DateTime Hinzugefuegt { get; set; }
      public TimeSpan Dauer { get; set; }
    }
}