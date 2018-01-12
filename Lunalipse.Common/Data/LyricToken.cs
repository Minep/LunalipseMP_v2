using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Common.Data
{
    public class LyricToken
    {
        public TimeSpan TimeStamp { get; private set; }
        public string Statement { get; private set; }
        public string SubStatement { get; private set; }
        public int Offset { get; private set; }

        public LyricToken(string Statement,TimeSpan TimeStamp)
        {
            this.Statement = Statement;
            this.TimeStamp = TimeStamp;
        }

        public LyricToken(string Statement,string SubStatement,TimeSpan TimeStamp,int Offset)
            : this(Statement,TimeStamp)
        {
            this.SubStatement = SubStatement;
            this.Offset = Offset;
        }

        public LyricToken(string Statement, string SubStatement, long TimeStampMS)
            : this(Statement, TimeSpan.FromMilliseconds(TimeStampMS))
        {
            this.SubStatement = SubStatement;
        }

        public LyricToken(string Statement, string SubStatement, int TimeStampS)
            : this(Statement, TimeSpan.FromSeconds(TimeStampS))
        {
            this.SubStatement = SubStatement;
        }
    }
}
