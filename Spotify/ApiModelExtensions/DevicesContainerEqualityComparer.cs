namespace Spotify.ApiModelExtensions
{
  using System.Collections.Generic;
  using System.Linq;

  using SpotifyWebApi.Model;

  public sealed class DevicesContainerEqualityComparer : IEqualityComparer<DevicesContainer>
  {
    #region Methods

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

    public int GetHashCode(DevicesContainer obj)
    {
      return obj.GetHashCode();
    }

    #endregion
  }
}