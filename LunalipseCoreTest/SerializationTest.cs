using System;
using System.Reflection;
using Lunalipse.Common.Data;
using Lunalipse.Core.Cache;
using Lunalipse.Core.Communicator;
using Lunalipse.Core.I18N;
using Lunalipse.Core.Metadata;
using Lunalipse.Core.PlayList;
using LunalipseCoreTest.Support;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LunalipseCoreTest
{
    [TestClass]
    public class SerializationTest
    {
        MusicListPool mlp;
        CataloguePool cpl;
        MediaMetaDataReader mmdr;
        [TestInitialize]
        public void Initialize()
        {
            mlp = MusicListPool.INSATNCE;
            //cpl = CataloguePool.INSATNCE;
            mmdr = new MediaMetaDataReader(new I18NConvertor());
            //mlp.AddToPool(@"F:\M2\", mmdr);
        }

        [TestMethod]
        public void ExporterTest()
        {
            GeneralExporter<CataloguePool> exporter = new GeneralExporter<CataloguePool>("export.ld");
            Assert.IsTrue(exporter.Export(cpl));
        }

        [TestMethod]
        public void ImporterTest()
        {
            GeneralImporter<CataloguePool> importer = new GeneralImporter<CataloguePool>("export.ld");
            CataloguePool cp = importer.Import();
            Assert.IsNotNull(cp);
            foreach(MusicEntity me in cp.GetCatalogue(0).MusicList)
            {
                PrintClass(typeof(MusicEntity), me);
            }
        }

        [TestMethod]
        public void I18NTest()
        {
            I18NTokenizer it = new I18NTokenizer();
            it.LoadFromFile(@"C:\Users\Lunaixsky\Desktop\Lunalipse I18N\i18n.lang");
            it.GetPages(SupportLanguages.CHINESE_SIM);
            //string s = I18NPage.INSTANCE.GetPage("CORE_FUNC").getContext("CORE_LBS_InfiniteCall");
            //Assert.AreEqual("检测到循环调用", s);
        }

        [TestMethod]
        public void TestCache()
        {
            string str = Compressed.readCompressed("json.txt", false);
            Caches ch = new Caches();
            Catalogue c = ch.RestoreTo<Catalogue>(str);
            Assert.IsNotNull(c);
        }


        private void PrintClass(Type t, object instance)
        {
            foreach(FieldInfo fi in t.GetFields())
            {
                Console.WriteLine("{0}   =>   {1}", fi.Name, fi.GetValue(instance));
            }
            Console.WriteLine();
        }
    }
}
