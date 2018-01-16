using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lunalipse.Core.Lyric;
using Lunalipse.Common.Data;
using System.Collections.Generic;

namespace LunalipseCoreTest
{
    [TestClass]
    public class ParserTest
    {
        LyricTokenizer lt;
        [TestInitialize]
        public void Initialize()
        {
            lt = LyricTokenizer.INSTANCE;
        }
        [TestMethod]
        public void LyricTokenizerTest()
        {
            List<LyricToken> outp = lt.CreateTokensFromFile(@"F:\M2\Lyrics\亚历山大红旗歌舞团 - 万岁,我们强大的祖国.lrc");
            CollectionAssert.AllItemsAreUnique(outp);
            outp.ForEach((ltk) =>
            {
                Console.WriteLine("{0} => {1} => {2}", ltk.TimeStamp, ltk.Statement, ltk.SubStatement);
            });
            //Assert.IsNotNull(lt.ParseStatement("[00:14.79]Got my guitar shreddin' up the latest tune", ref offset));
        }
    }
}
