namespace Spotify.PrismExtensions
{
  using System;

  /// <summary>Generic event args of type t.</summary>
  /// <typeparam name="T">The type of the event args payload.</typeparam>
  public class EventArgs<T> : EventArgs
  {
    #region Constructors

    /// <summary>Initializes a new instance of the <see cref="EventArgs{T}" /> class.</summary>
    public EventArgs()
    {
    }

    /// <summary>Initializes a new instance of the <see cref="EventArgs{T}" /> class.</summary>
    /// <param name="payload">The payload.</param>
    public EventArgs(T payload)
    {
      this.Payload = payload;
    }

    #endregion

    #region Properties

    /// <summary>Gets or sets the data.</summary>
    public T Payload { get; set; }

    #endregion
  }
}