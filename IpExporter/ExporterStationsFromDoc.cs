using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanIP;

namespace IpExporter
{
    public class ExporterStationsFromDoc : IExporterStations
    {
        public string Directory { get; set; }
        public string[] FileNames => System.IO.Directory.GetFiles(this.Directory, "*.doc*");

        public event Action?FileCompleted;

        public ExporterStationsFromDoc(string directory)
        {
            this.Directory = directory;
        }

        public List<NetPS> GetListNetPS()
        {
            var output = new List<NetPS>();
            foreach (var file in FileNames)
            {
                TextDataPS tdPS = TextExporterFromWord.GetTextDataPsFromDoc(file);
                var netPS = new NetPS(tdPS);
                output.Add(netPS);
               FileCompleted?.Invoke();
            }
            return output;
        }
    }
}
