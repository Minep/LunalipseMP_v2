using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lunalipse.Core.Lyric;
using Lunalipse.Common.Data;
using System.Collections.Generic;
using Lunalipse.Core.BehaviorScript;
using Lunalipse.Common.Data.BehaviorScript;

namespace LunalipseCoreTest
{
    [TestClass]
    public class ParserTest
    {
        LyricTokenizer lt;
        Parser BSP;
        [TestInitialize]
        public void Initialize()
        {
            lt = LyricTokenizer.INSTANCE;
            BSP = Parser.INSTANCE;
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

        [TestMethod]
        public void ScriptToenizerTest()
        {
            BSP.RootPath = @"F:\Lunalipse\TestUnit\bin\Debug";
            BSP.ErrorOccured += (x, y, z) =>
            {
                Console.WriteLine("{0} : {2} -> {1}", x, y, z);
            };
            BSP.Load("prg2");
            BSP.Parse();
            List<ScriptToken> ts = BSP.Tokens;
            Assert.IsTrue(ts.Count > 0);
            foreach(ScriptToken st in ts)
            {
                Console.WriteLine("Command: {0} | Args:[ {1} ]", st.Command, string.Join(",", st.Args));
                Console.WriteLine("Command: {0} | Args:[ {1} ]", st.TailFix, string.Join(",", st.TailArgs));
                Console.WriteLine();
            }
        }
    }
}
