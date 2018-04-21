using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify.Daten
{
  public class ExamplePlaylist : BindingList<TitleData>
  {
    public ExamplePlaylist()
    {
      this.Add(new TitleData()
      {
        Titel = "Das eine Lied",
        Kuenstler = "Luigi",
        Album = "Warum ich meinen Bruder hasse",
        Hinzugefuegt = DateTime.Parse("29.03.2018"),
        Dauer = new TimeSpan(0, 5, 42)
      });

      this.Add(new TitleData()
      {
        Titel = "Das andere Lied",
        Kuenstler = "Mario",
        Album = "Princess love",
        Hinzugefuegt = DateTime.Parse("29.03.2018"),
        Dauer = new TimeSpan(0, 3, 26)
      });

      this.Add(new TitleData()
      {
        Titel = "Das nich so beliebte Lied",
        Kuenstler = "Contoso",
        Album = "LN3",
        Hinzugefuegt = DateTime.Parse("29.03.2018"),
        Dauer = new TimeSpan(0, 4, 34)
      });
    }
  }
}
