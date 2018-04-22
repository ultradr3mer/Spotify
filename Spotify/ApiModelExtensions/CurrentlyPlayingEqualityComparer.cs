namespace Spotify.ApiModelExtensions
{
  using System.Collections.Generic;

  using SpotifyWebApi.Model;

  /// <summary>The equality comparer for the currently playing model.</summary>
  public sealed class CurrentlyPlayingEqualityComparer : IEqualityComparer<CurrentlyPlaying>
  {
    #region Methods

    /// <summary>Checks whether the two models equal each other.</summary>
    /// <param name="x">The first model.</param>
    /// <param name="y">The second model.</param>
    /// <returns>Whether the two models equal each other.</returns>
    public bool Equals(CurrentlyPlaying x, CurrentlyPlaying y)
    {
      if (object.ReferenceEquals(x, y))
      {
        return true;
      }

      if (object.ReferenceEquals(x, null))
      {
        return false;
      }

      if (object.ReferenceEquals(y, null))
      {
        return false;
      }

      if (x.GetType() != y.GetType())
      {
        return false;
      }

      return object.Equals(x.Context?.Uri, y.Context?.Uri) && x.Timestamp == y.Timestamp && x.ProgressMs == y.ProgressMs && x.IsPlaying == y.IsPlaying && object.Equals(x.Item?.Id, y.Item?.Id);
    }

    /// <summary>Gets the hash code for the model.</summary>
    /// <param name="obj">The model.</param>
    /// <returns>The hash code.</returns>
    public int GetHashCode(CurrentlyPlaying obj)
    {
      unchecked
      {
        var hashCode = obj.Context != null ? obj.Context.Uri.GetHashCode() : 0;
        hashCode = (hashCode * 397) ^ obj.Timestamp.GetHashCode();
        hashCode = (hashCode * 397) ^ obj.ProgressMs;
        hashCode = (hashCode * 397) ^ obj.IsPlaying.GetHashCode();
        hashCode = (hashCode * 397) ^ (obj.Item != null ? obj.Item.Id.GetHashCode() : 0);
        return hashCode;
      }
    }

    #endregion
  }
}