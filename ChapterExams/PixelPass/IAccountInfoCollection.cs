using System.Collections.Generic;

namespace PixelPass
{
    public interface IAccountInfoCollection
    {
        string Name { get; set; }
        List<AccountInfo> AccountInfos { get;  }
    }
}