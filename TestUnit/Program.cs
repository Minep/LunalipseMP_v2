using Lunalipse.Core.GlobalSetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestUnit
{
    class Program
    {
        static void Main(string[] args)
        {
            Setting.FFT_DELAY = 20;
            Setting.USE_FFT = true;
            Setting.VOLUME = 0.2f;
            Setting.SAVED_PATH = new string[]
            {
                "PATH 1","PATH 2","PATH 3"
            };
            GlobalSettingHelper<Setting> gsh = GlobalSettingHelper<Setting>.INSTANCE;
            //gsh.ReadSetting("config.lps");
            //foreach(string s in Setting.SAVED_PATH)
            //{
            //    Console.WriteLine(s);
            //}
            gsh.SaveSetting();
            Console.Read();
        }
    }
}
