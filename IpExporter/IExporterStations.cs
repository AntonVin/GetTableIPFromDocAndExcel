﻿using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordPlanIP;

namespace IpExporter
{
    public interface IExporterStations
    {
        public List<NetPS> Stations { get; }

    }
}