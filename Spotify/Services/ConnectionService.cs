namespace Spotify.Services
{
  using System;
  using System.Net;
  using System.Net.Sockets;
  using System.Text;

  using Prism.Events;

  using Spotify.Data;
  using Spotify.Events;

  using SpotifyWebApi;
  using SpotifyWebApi.Auth;
  using SpotifyWebApi.Model.Enum;

  using Unity;

  internal class ConnectionService
  {
    #region Fields

    public const string CallbackHost = "localhost";

    private readonly IUnityContainer container;

    private readonly IEventAggregator eventAggregator;

    private AuthParameters apiParameter;

    private bool isAwaiting;

    #endregion

    #region Constructors

    public ConnectionService(IUnityContainer container)
    {
      this.container = container;

      this.eventAggregator = container.Resolve<IEventAggregator>();
    }

    #endregion

    #region Methods

    /// <summary>Initializes the connection.</summary>
    /// <param name="clientId">The client id.</param>
    /// <param name="clientSecret">The client secret.</param>
    public void TryInitializeConnection(string clientId, string clientSecret)
    {
      if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
      {
        return;
      }

      this.apiParameter = new AuthParameters
                            {
                              Scopes = Scope.All,

                              ClientId = clientId,
                              ClientSecret = clientSecret,

                              RedirectUri = "http://localhost:8000",
                              ShowDialog = false // Set to true to login each time.
                            };

      var url = AuthorizationCode.GetUrl(this.apiParameter, "test");

      var connectUrl = new Uri(url);

      this.eventAggregator.GetEvent<ConnectionUriChangedEvent>().Publish(connectUrl);

      if (!this.isAwaiting)
      {
        this.AwaitResponse();
      }
    }

    /// <summary>Start a web server that listens on 8000 and retrieve the access code.</summary>
    private async void AwaitResponse()
    {
      var tcpListener = new TcpListener(IPAddress.Any, 8000);
      tcpListener.Start();
      this.isAwaiting = true;

      var s = await tcpListener.AcceptSocketAsync();

      var buffer = new byte[2048];
      var i = s.Receive(buffer, buffer.Length, 0);

      // Convert Byte to String
      var bufferString = Encoding.ASCII.GetString(buffer);

      s.Shutdown(SocketShutdown.Both);
      tcpListener.Stop();

      var t = bufferString.Split('?')[1].Split('&')[0].Split('=')[1];

      this.GetBasicData(t);

      this.isAwaiting = false;
    }

    /// <summary>The get basic data.</summary>
    /// <param name="tokenString">The token string.</param>
    private async void GetBasicData(string tokenString)
    {
      var token = AuthorizationCode.ProcessCallback(this.apiParameter, tokenString, string.Empty);
      this.container.RegisterInstance(token);

      var api = new SpotifyWebApi(token);
      this.container.RegisterInstance<ISpotifyWebApi>(api);

      var profile = api.UserProfile.GetMe();
      var playLists = api.Playlist.GetMyPlaylists(200);

      var data = new ConnectionData
                   {
                     Profile = await profile,
                     Playlists = await playLists,
                     Api = api,
                     Token = token
                   };

      this.eventAggregator.GetEvent<ConnectionEstablishedEvent>().Publish(data);
    }

    #endregion
  }
}