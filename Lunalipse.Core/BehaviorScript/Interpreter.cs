using Lunalipse.Common.Interfaces.IBehaviorScript;
using Lunalipse.Common.Interfaces.IConsole;
using Lunalipse.Core.Console;
using Lunalipse.Core.PlayList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Core.BehaviorScript
{
    public class Interpreter : ComponentHandler , IInterpreter
    {
        static volatile Interpreter INT_INSTANCE;
        static readonly object LOCK_OBJ = new object();

        public static Interpreter INSTANCE(string BasePath)
        {
            if (INT_INSTANCE == null)
            {
                lock (LOCK_OBJ)
                {
                    INT_INSTANCE = INT_INSTANCE ?? new Interpreter(BasePath);
                }
            }
            return INT_INSTANCE;
        }

        Parser ScriptParser;
        CataloguePool CataPool;

        protected Interpreter(string basePath)
        {
            ScriptParser = new Parser();
            CataPool = CataloguePool.INSATNCE;
            ScriptParser.RootPath = basePath;
            ScriptParser.ErrorOccured += (x, y, z) =>
            {
                ErrorDelegation.OnErrorRaisedBSI?.Invoke(x, y, z);
            };
            ConsoleAdapter.INSTANCE.RegisterComponent("lbsi", this);
        }

        public bool Load(string ScriptID)
        {
            if (!ScriptParser.Load(ScriptID))
            {
                return false;
            }
            if(!ScriptParser.Parse())
            {
                return false;
            }
            return true;
        }

        public bool LoadPath(string ScriptPath)
        {
            if (!ScriptParser.LoadPath(ScriptPath))
            {
                return false;
            }
            if (!ScriptParser.Parse())
            {
                return false;
            }
            return true;
        }
    }
}
