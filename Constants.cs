﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanner_Scale_OPOS_Wrapper
{
    internal class Constants
    {
        public enum MessageType
        {
            normal,
            scale_error,
            scanner_error,
            namedPipes_error,
            consoleOnly,
            ini,
            misc,
        }
    }
}
