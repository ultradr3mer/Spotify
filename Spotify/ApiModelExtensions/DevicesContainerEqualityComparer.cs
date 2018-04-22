namespace Spotify.ApiModelExtensions
{
  using System.Collections.Generic;
  using System.Linq;

  using SpotifyWebApi.Model;

  /// <summary>The equality comparer for the devices container model.</summary>
  public sealed class DevicesContainerEqualityComparer : IEqualityComparer<DevicesContainer>
  {
    #region Methods

    /// <summary>Checks whether the two models equal each other.</summary>
    /// <param name="x">The first model.</param>
    /// <param name="y">The second model.</param>
    /// <returns>Whether the two models equal each other.</returns>
    public bool Equals(DevicesContainer x, DevicesContainer y)
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

      if (x.Devices == null && y.Devices == null)
      {
        return true;
      }

      if (x.Devices == null || y.Devices == null)
      {
        return false;
      }

      return x.Devices.SequenceEqual(y.Devices, new DeviceEqualityComparer());
    }

    /// <summary>Gets the hash code for the model.</summary>
    /// <param name="obj">The model.</param>
    /// <returns>The hash code.</returns>
    public int GetHashCode(DevicesContainer obj)
    {
      return obj.GetHashCode();
    }

    #endregion
  }
}