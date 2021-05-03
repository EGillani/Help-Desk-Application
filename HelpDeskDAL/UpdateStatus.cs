/**
 * Class Name:UpdateStatus.cs
 * Purpose: creats an emunerated value to contain some statuses to let us know what state the data is in 
 * Coder: Eraj Gillani 0858887
 * Date: November 12th 2020
 **/
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskDAL
{
    public enum UpdateStatus
    {
        Ok = 1,
        Failed = -1,
        Stale = -2
    }
}
