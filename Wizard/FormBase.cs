﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VSIX_MVC_Layered_Wizard
{
    public class FormBase:Form
    {
        public bool IsDebugMode
        {
            get
            {
#if DEBUG==true
                return true;
#else
                return false;
#endif
            }
        }
    }
}
