using Lunalipse.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Common.Interfaces.IAudio
{
    public interface ILpsAudio
    {
        void SetEqualizer(double[] data);
        bool SetEqualizerIndex(int inx, double data);
        void Load(MusicEntity music);
        void Play();
        void Pause();
        void Resume();
        void Stop();
        void MoveTo(long secs);
    }
}
