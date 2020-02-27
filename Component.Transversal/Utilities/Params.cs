using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Transversal.Utilities
{

    public static class Params
    {
        public static int AttemptsByDocument = 3;

        public static int IntervalTime = 15;

        #region NominationProcess

        public const int campidAuthorized = 7611;

        public const int campidConfirmation = 7635;

        public const string codeBase = "cx";

        public const string codeVP = "cxvp";

        public const string codeVI = "cxvi";

        public const string codeVPError = "cx";

        public const string codeManual = "cxm";

        public const string codeManager = "cxmg";

        public const string codeLogin = "cxlg";

        public const string codeLoginI = "cxli";

        public static string[] codeResponse = { "cx1", "cx2", "1", "2" };

        public static string[] codeResponseTrue = { "cx1", "1" };

        public static string[] codeResponseFalse = { "cx2", "2" };

        #endregion
    }
}
