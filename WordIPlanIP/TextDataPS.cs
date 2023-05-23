using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanIP
{
    public record TextDataPS(string NamePS, string TotalNet,
            string TM, string MGMT, string CRAP, string ASKUE, string Control,
            string VoIP, string KISU, string Video, string PA, string Monitoring, string ASU);
}
