using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Common.Interfaces.ISetting
{
    public interface ISettingHelper<T> where T : IGlobalSetting
    {
        bool ReadSetting(string path);
        void SaveSetting();
    }
}
