using CSCore;
using Lunalipse.Common.Generic.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Common.Data
{
    public class Track
    {
        public TimeSpan Duration;
        public ChannelType Channel;
        public AudioEncoding Encoding;
        public int SampleRate, Sampling;
    }
}
