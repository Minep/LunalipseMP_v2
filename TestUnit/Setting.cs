using Lunalipse.Common.Data.Attribute;
using Lunalipse.Common.Interfaces.ISetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUnit
{
    public class Setting : IGlobalSetting
    {
        [ExportedSettingItem]
        public static float VOLUME;
        [ExportedSettingItem]
        public static string[] SAVED_PATH;
        [ExportedSettingItem]
        public static bool USE_FFT;
        [ExportedSettingItem]
        public static int FFT_DELAY;
    }
}
