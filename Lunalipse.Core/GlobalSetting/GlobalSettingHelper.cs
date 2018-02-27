using Lunalipse.Common.Data.Attribute;
using Lunalipse.Common.Data.Errors;
using Lunalipse.Common.Interfaces.IConsole;
using Lunalipse.Common.Interfaces.ISetting;
using Lunalipse.Core.Communicator;
using Lunalipse.Core.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lunalipse.Core.GlobalSetting
{
    public class GlobalSettingHelper<GS> : ComponentHandler, ISettingHelper<GS> where GS : IGlobalSetting
    {
        static volatile GlobalSettingHelper<GS> GSH_INSTANCE;
        static readonly object GSH_LOCK = new object();
        public static GlobalSettingHelper<GS> INSTANCE
        {
            get
            {
                if (GSH_INSTANCE == null)
                    lock (GSH_LOCK)
                        GSH_INSTANCE = GSH_INSTANCE ?? new GlobalSettingHelper<GS>();
                return GSH_INSTANCE;
            }
        }

        string VERSION;
        public string OutputFile { get; set; }
        private GlobalSettingHelper()
        {
            VERSION = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            ConsoleAdapter.INSTANCE.RegisterComponent("lpsseting", this);
            OutputFile = "config.lps";
        }

        public GS ReadSetting(string path)
        {
            GeneralImporter<GS> importer = new GeneralImporter<GS>(path);
            string ver = importer.GetExtra()[1];
            if (!VERSION.Equals(ver))
            {
                ErrorDelegation.OnErrorRaisedGSH?.Invoke("CORE_GSH_DamagedSave", -1, VERSION, ver);
                return default(GS);
            }
            return importer.Import();
        }

        public bool SaveSetting(GS instance)
        {
            GeneralExporter<GS> exporter = new GeneralExporter<GS>(OutputFile, "version", VERSION);
            return exporter.Export(instance);
        }

        #region Command Handler
        public override bool OnCommand(params string[] args)
        {
            return true;
        }
        #endregion
    }
}
