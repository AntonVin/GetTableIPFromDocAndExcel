#define TEST
using word = Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IpLibrary;

namespace PlanIP.Test
{
    public class NetPSTests
    {
        [Fact]
        public void NetPS_Const()
        {

            var exceptedCorrectSubnets = new string[] {
                "10.146.216.0/29 TM",
                "10.146.216.8/29 TM",
                "10.146.216.16/29 TM",
                "10.146.216.24/29 TM",
                "10.146.216.64/29 TM",
                "10.146.216.72/29 TM",
                "10.146.216.80/30 TM",

                "10.146.216.129/32 MGMT",
                "10.146.216.33/32 MGMT",
                "10.146.216.130/32 MGMT",
                "10.146.216.34/32 MGMT",
                "10.146.216.144/28 MGMT",

                "10.146.216.176/28 CRAP",

                "10.146.216.160/28 ASKUE",

                "10.146.217.0/25 Control",

                "10.146.217.128/26 VoIP",

                "10.146.217.192/26 KISU",

                "10.146.218.0/26 Video",

                "10.146.218.64/26 PA",

                "10.146.220.0/24 Monitoring",

                "10.146.221.0/24 ASU",
                "10.146.222.0/24 ASU"
            };
            var exceptedCrossedSubnets = new string[] {
                "10.146.219.0/24 Monitoring",
                "10.146.219.192/29 Monitoring",
            };
            var exceptedNotOwnedSubnets = new string[] {
                "10.146.212.0/22 ASU"
            };

            string fullPath = Path.Combine(
                 Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            @"doc\25 Встреча.docx");

            TextDataPS tdPS = TextExporterFromWord.GetTextDataPsFromDoc(fullPath);

            var netPS = new NetPS(tdPS);

            var correctSubnets = ManyIpToString(netPS.CorrectSubnets);
            var crossedSubnets = ManyIpToString(netPS.CrossedSubnets);
            var notOwnedSubnets = ManyIpToString(netPS.NotOwnedSubnets);

            Assert.True(exceptedCorrectSubnets.SequenceEqual(correctSubnets));
            Assert.True(exceptedCrossedSubnets.SequenceEqual(crossedSubnets));
            Assert.True(exceptedNotOwnedSubnets.SequenceEqual(notOwnedSubnets));
        }

        private string[] ManyIpToString(List<SubnetAddress> listIp)
        {
            return listIp.Select(x => x.ToString()+" "+x.GroupNet).ToArray();
        }
    }
}
