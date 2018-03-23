using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BNE.Auth
{
    public enum LogoffType
    {
        NONE,
        BY_USER,
        UNAUTHORIZED,
        OVERRIDDEN_SESSION,
        EXCEEDED_USER,
        COMPANY_TRYING_TO_USE_OTHER_STC
    }
}
