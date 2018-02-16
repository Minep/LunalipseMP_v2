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
        public int Position { get; private set; }
        public bool HasModified { get; private set; }

        public LyricToken(string Statement,TimeSpan TimeStamp)
        {
            this.Statement = Statement;
            this.TimeStamp = TimeStamp;
            HasModified = false;
        }

        public LyricToken(string Statement,string SubStatement,TimeSpan TimeStamp,int Offset)
            : this(Statement,TimeStamp)
        {
            this.SubStatement = SubStatement;
            this.Offset = Offset;
        }

        public LyricToken(string Statement, string SubStatement, TimeSpan TimeStamp, int Offset, int Position)
            : this(Statement, SubStatement, TimeStamp, Offset)
        {
            this.Position = Position;
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

        public void ChangeStatement(string stat)
        {
            Statement = stat;
            HasModified = true;
        }
        public void ChangeSubStatement(string sstat)
        {
            SubStatement = sstat;
            HasModified = true;
        }
    }
}
