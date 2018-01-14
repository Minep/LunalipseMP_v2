using Lunalipse.Common.Data.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Core
{
    public class ErrorDelegation
    {
        public delegate void ErrorRaisedGSH(ErrorGSH error, params string[] args);
        public delegate void ErrorRaisedI18N(ErrorI18N error, params string[] args);

        public static ErrorRaisedGSH OnErrorRaisedGSH;
        public static ErrorRaisedI18N OnErrorRaisedI18N;
    }
}
