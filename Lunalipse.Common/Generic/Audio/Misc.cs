using CSCore;
using Lunalipse.Common.Data;

namespace Lunalipse.Common.Generic.Audio
{
    public static class Misc
    {
        public static Track ToTrack(this IWaveSource wavesource)
        {
            return new Track()
            {
                Duration = wavesource.GetLength(),
                Channel = (ChannelType)wavesource.WaveFormat.Channels,
                Encoding = wavesource.WaveFormat.WaveFormatTag,
                SampleRate = wavesource.WaveFormat.SampleRate,
                Sampling = wavesource.WaveFormat.BitsPerSample,
            };
        }
    }
}
