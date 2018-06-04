//using Lunalipse.Common.Generic.Cache;
using Lunalipse.Common.Generic.Cache;
using Lunalipse.Common.Interfaces.ICache;
using Lunalipse.Core.PlayList;
using Lunalipse.Utilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lunalipse.Common.Generic.Cache.CacheInfo;

namespace Lunalipse.Core.Cache
{
    public class MusicCacheIndexer: ICacheOperator
    {
        public bool UseLZ78Compress { get; set; }
        public string CacheDir { get; private set; }

        Caches caches;
        public MusicCacheIndexer()
        {
            caches = new Caches();
        }

        public void CacheMusicCatalogue(Catalogue cata)
        {
            WinterWrapUp cw = new WinterWrapUp()
            {
                createDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                deletable = false,
                markName = CacheUtils.GenerateMarkName("CATALOGUE"),
                uid = Guid.NewGuid().ToString()
            };
            byte[] cstring = Encoding.UTF8.GetBytes(caches.CacheTo(cata, cw));
            Compressed.writeCompressed(cstring, "{0}//{1}".FormateEx(CacheDir, CacheUtils.GenerateName(cw)), UseLZ78Compress);
        }

        public Catalogue RestoreMusicCataloge(object obj)
        {
            return caches.RestoreTo<Catalogue>(obj);
        }

        public void CacheCatalogues(List<Catalogue> catas)
        {
            foreach(Catalogue c in catas)
            {
                CacheMusicCatalogue(c);
            }
        }

        public void CacheCataloguesObject(CataloguePool cpool)
        {
            foreach (Catalogue c in cpool.All)
            {
                CacheMusicCatalogue(c);
            }
        }

        public List<Catalogue> RestoreCatalogues(List<WinterWrapUp> cws)
        {
            List<Catalogue> catas = new List<Catalogue>();
            foreach(WinterWrapUp cw in cws)
            {
                catas.Add(
                    RestoreMusicCataloge(
                        JObject.Parse(
                            Compressed.readCompressed("{0}//{1}".FormateEx(CacheDir, CacheUtils.GenerateName(cw)),UseLZ78Compress)
                        )["ctx"]
                    )
                );
            }
            return catas;
        }

        public object InvokeOperator(CacheResponseType crt, params object[] args)
        {
            switch (crt)
            {
                case CacheResponseType.SINGLE_CACHE:
                    CacheCataloguesObject((CataloguePool)args[0]);
                    break;
                case CacheResponseType.BLUCK_CACHE:
                    CacheCatalogues((List<Catalogue>)args[0]);
                    break;
                case CacheResponseType.SINGLE_RESTORE:
                    //RestoreMusicCataloge((Catalogue)args[0])
                    return null;
                case CacheResponseType.BLUCK_RESTORE:
                    return RestoreCatalogues((List<WinterWrapUp>)args[0]);
            }
            return null;
        }

        public void SetCacheDir(string BaseDir)
        {
            CacheDir = BaseDir;
        }
    }
}
