namespace Spotify.ApiModelExtensions
{
  using SpotifyWebApi.Model;

  /// <summary>The equality comparer for the currently playing context model.</summary>
  internal class CurrentlyPlayingContextEqualityComparer
  {
    #region Methods

    /// <summary>Checks whether the two models equal each other.</summary>
    /// <param name="x">The first model.</param>
    /// <param name="y">The second model.</param>
    /// <returns>Whether the two models equal each other.</returns>
    public bool Equals(CurrentlyPlayingContext x, CurrentlyPlayingContext y)
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

      return object.Equals(x.Device?.Id, y.Device?.Id) && string.Equals(x.RepeatState, y.RepeatState) && x.ShuffleState == y.ShuffleState && object.Equals(x.Context?.Uri, y.Context?.Uri) && x.TimestampMs == y.TimestampMs && x.ProgressMs == y.ProgressMs && x.IsPlaying == y.IsPlaying
             && object.Equals(x.Item?.Id, y.Item?.Id);
    }

    /// <summary>Gets the hash code for the model.</summary>
    /// <param name="obj">The model.</param>
    /// <returns>The hash code.</returns>
    public int GetHashCode(CurrentlyPlayingContext obj)
    {
      unchecked
      {
        var hashCode = obj.Device != null ? obj.Device.Id.GetHashCode() : 0;
        hashCode = (hashCode * 397) ^ (obj.RepeatState != null ? obj.RepeatState.GetHashCode() : 0);
        hashCode = (hashCode * 397) ^ obj.ShuffleState.GetHashCode();
        hashCode = (hashCode * 397) ^ (obj.Context != null ? obj.Context.Uri.GetHashCode() : 0);
        hashCode = (hashCode * 397) ^ obj.TimestampMs.GetHashCode();
        hashCode = (hashCode * 397) ^ obj.ProgressMs.GetHashCode();
        hashCode = (hashCode * 397) ^ obj.IsPlaying.GetHashCode();
        hashCode = (hashCode * 397) ^ (obj.Item != null ? obj.Item.GetHashCode() : 0);
        return hashCode;
      }
    }

    #endregion
  }
}