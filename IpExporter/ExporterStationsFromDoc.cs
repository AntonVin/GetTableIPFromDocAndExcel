using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordPlanIP;

namespace IpExporter
{
    public class ExporterStationsFromDoc : IExporterStations
    {
        public List<NetPS> Stations{ get; }

        public ExporterStationsFromDoc(string directory)
        {
            Stations = GetListNetPS(directory);
        }

        private List<NetPS> GetListNetPS(string directory)
        {
            var output = new List<NetPS>();
            var files = System.IO.Directory.GetFiles(directory, "*.doc*");
            foreach (var file in files)
            {
                TextDataPS tdPS = TextExporterFromWord.GetTextDataPsFromDoc(file);
                var netPS = new NetPS(tdPS);
                output.Add(netPS);
            }
            return output;
        }
    }
}
