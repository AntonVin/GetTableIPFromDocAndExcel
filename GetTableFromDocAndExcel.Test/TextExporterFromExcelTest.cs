using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using PlanIP;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
using System.Xml.Linq;

namespace PlanIP.Test
{
    public class TextExporterFromExcelTest
    {
        [Fact]
        public void GetTextFromCell()
        {
            #region Expected properties textDataPS
            var name = "ПС Акулово";
            var totalNet = "10.146.120.0/21";
            var tm = "10.146.120.0/29-моэск1 10.146.120.8/29-моэск2 10.146.120.16/29-рду1 10.146.120.24/29-рду2 10.146.120.33/32-лупбэк маршрутизатора 1 (vrf moesk) 10.146.120.34/32-лупбэкмаршрутизатора 2 (vrf moesk) 10.146.120.36/30-линк между маршрутизаторами(vrf moesk) 10.146.120.40/30-основной линк с МОЭСК/vrf moesk) 10.146.120.44/30-резервный линк с МОЭСК/vrf moesk) 10.146.120.49/32-лупбэк маршрутизатора 1 (vrf rdu) 10.146.120.50/32-лупбэкмаршрутизатора 2 (vrf rdu) 10.146.120.52/30-линк между маршрутизаторами/vrf rdu) 10.146.120.56/30-основной линк с МОЭСК/vrf rdu) 10.146.120.60/30-резервный линк с МОЭСК/vrf rdu)";
            var mgmt = "";
            var crap = "10.146.120.176/28";
            var askue = "10.146.120.160/28";
            var control = "10.146.121.0/25";
            var voip = "10.146.121.128/26";
            var kisu = "10.146.121.192/26";
            var video = "10.146.122.0/26";
            var pa = "10.146.122.64/26";
            var monotoring = "10.146.123.0/24 10.146.124.0/24";
            var asu = "10.146.125.0/24 10.146.126.0/24";
            #endregion

            var curDir = Environment.CurrentDirectory;
            string pathFile = Directory.GetParent(curDir).Parent.Parent.FullName + @"\doc\IP план ТСПД  ПС.xlsx" ;
            var excelApp = new Excel.Application();
            excelApp.Visible = true;
            Excel.Workbook excelWB = excelApp.Workbooks.Open(Filename: pathFile,
                ReadOnly: true);

            Excel.Worksheet sheet = excelWB.Sheets[1];
            Excel.Range rangeHeader = sheet.Cells.Find("Наименование и номер ПС");

            var tdPS = TextExporterFromExcel.GetTextDataPsFromExcel(rangeHeader);

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
