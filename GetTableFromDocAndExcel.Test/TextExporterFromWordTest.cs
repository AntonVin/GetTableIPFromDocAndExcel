using System.Reflection;
using System.Threading.Channels;
using PlanIP;
using word = Microsoft.Office.Interop.Word;

namespace GetTableFromDocAndExcel.Test
{
    public class TextExporterFromWordTest
    {
        [Fact]
        public void GetTextDataPsFromDoc_Vstrecha()
        {
            #region Expected properties textDataPS
            var name = "25 Встреча";
            var totalNet = "10.146.216.0/21";
            var tm = "10.146.216.0/29-моэск1\r10.146.216.8/29-моэск2\r10.146.216.16/29-рду1\r10.146.216.24/29-рду2\r\r\r\r\r\r\r\r\r\r\r10.146.216.64/29 – Телемеханика осн ВКС\r10.146.216.72/29 – Телемеханика рез ВКС\r10.146.216.80/30 - Термоконтроль ВКС";
            var mgmt = "Маршрутизатор 1\rLoopback 0\r10.146.216.129/32\rLoopback 1  (vrf MGMT)\r10.146.216.129/32\rGi0/1\r10.146.216.151/30 – линк к Rt2\rGi0/0/0\r10.146.216.33 – внутренний линк с интерфейсным модулем\rGi 0/0/0.10 (vrf MGMT)\r10.146.216.145/28 – standby ip\r10.146.216.146/28 – ip \rИнтерфейсная плата 1\rVlan10\r10.146.216.150/28\rКоммутатор 1\rVlan10\r10.146.216.148/28\rМаршрутизатор 2\rLoopback 0\r10.146.216.130/32\rLoopback 1  (vrf MGMT)\r10.146.216.130/32\rGi0/1\r10.146.216.151/30 – линк к Rt1\rGi0/0/0\r10.146.216..34  – внутренний линк с интерфейсным модулем\rGi 0/0/0.10 (vrf MGMT)\r10.146.216.145/28 – standby ip\r10.146.216.147/28 – ip \rИнтерфейсная плата 2\rVlan10\r10.146.216.151/28\rКоммутатор 2\rVlan10\r10.146.216.149/28";
            var crap = @"10.146.216.176/28";
            var askue = "10.146.216.160/28";
            var control = "10.146.217.0/25";
            var voip = "10.146.217.128/26";
            var kisu = "10.146.217.192/26";
            var video = "10.146.218.0/26";
            var pa = "10.146.218.64/26";
            var monotoring = "10.146.219.0/24\r10.146.220.0/24\r\r10.146.219.192/29\r(мониторинг UPS /vrf UPS)";
            var asu = "10.146.221.0/24\r10.146.222.0/24\r\r10.146.215.0/22-\rНе является подсетью(добавил для теста)"; 
            #endregion


            string fullPath = Path.Combine(
                 Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), 
                @"doc\25 Встреча.docx");
            

            TextDataPS tdPS = TextExporterFromWord.GetTextDataPsFromDoc(fullPath);

            Assert.NotNull(tdPS);
            Assert.Equal(name, tdPS.NamePS);
            Assert.Equal(totalNet, tdPS.TotalNet);
            Assert.Equal(tm, tdPS.TM);
            Assert.Equal(mgmt, tdPS.MGMT);
            Assert.Equal(crap, tdPS.CRAP);
            Assert.Equal(askue, tdPS.ASKUE);
            Assert.Equal(control, tdPS.Control);
            Assert.Equal(voip, tdPS.VoIP);
            Assert.Equal(kisu, tdPS.KISU);
            Assert.Equal(video, tdPS.Video);
            Assert.Equal(pa, tdPS.PA);
            Assert.Equal(monotoring, tdPS.Monitoring);
            Assert.Equal(asu, tdPS.ASU);
        }
    }
}