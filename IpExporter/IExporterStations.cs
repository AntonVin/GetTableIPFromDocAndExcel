using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanIP;

namespace IpExporter
{
    public interface IExporterStations
    {
        public string Directory{ get; set; }

        public string[] FileNames { get; }
        public List<NetPS> GetListNetPS();

        public event Action? FileCompleted;

    }
}
