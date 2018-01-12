using CSCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Common.Interfaces.IAudio
{
    public interface ILpsFftWarp
    {
        IWaveSource Initialize(ISampleSource OrgWave);
    }
}
