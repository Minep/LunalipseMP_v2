using Lunalipse.Common.Generic.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lunalipse.Common.Generic.Cache.CacheInfo;

namespace Lunalipse.Common.Interfaces.ICache
{
    public interface ICacheHub
    {
        bool CacheObject<T>(T obj, CacheType type) where T : ICachable;
        bool CacheObjects<T>(List<T> obj, CacheType type) where T : ICachable;

        T RestoreObject<T>(Func<WinterWrapUp, bool> Conditions, CacheType type);
        IEnumerable<T> RestoreObjects<T>(Func<WinterWrapUp, bool> Conditions, CacheType type);

        void DeleteCaches(bool forced = false);

    }
}
