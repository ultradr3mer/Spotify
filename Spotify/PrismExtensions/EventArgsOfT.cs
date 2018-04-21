namespace Spotify.PrismExtensions
{
  using System;
  using System.ComponentModel;

  public class EventArgs<T> : EventArgs
  {
    #region Properties

    public T Data { get; set; }

    #endregion

    #region Constructors

    public EventArgs()
    {
    }

    public EventArgs(T data)
    {
      this.Data = data;
    }

    #endregion
  }

  public class CancelEventArgs<T> : CancelEventArgs
  {
    #region Properties

    public T Data { get; set; }

    #endregion

    #region Constructors

    public CancelEventArgs()
    {
    }

    public CancelEventArgs(T data)
    {
      this.Data = data;
    }

    #endregion
  }
}