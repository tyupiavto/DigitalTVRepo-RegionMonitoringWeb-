﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanelDevice.Models
{
    public class TrapLogInformationList
    {
        public List<Trap> TrapLogList = new List<Trap>();
        public int ErrorCount { get; set; }
        public int CorrectCount { get; set; }
        public int CrashCount { get; set; }
        public int WhiteCount { get; set; }
        public int AllCount { get; set; }
    }
}