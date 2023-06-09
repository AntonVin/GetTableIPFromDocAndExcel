﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using PlanIP;
using Excel = Microsoft.Office.Interop.Excel;

namespace IpExporter
{
    public class ExporterStationsFromExcel : IExporterStations
    {
        public string Directory { get; set; }
        public string[] FileNames => System.IO.Directory.GetFiles(this.Directory, "*.xls*");

        public event Action? FileCompleted;
        public ExporterStationsFromExcel(string directory)
        {
            this.Directory = directory;
        }

        public List<NetPS> GetListNetPS()
        {
            var output = new List<NetPS>();
            var excelApp = new Excel.Application();
            excelApp.Visible = true;
            foreach (var fileName in this.FileNames)
            {
                var stationsFromFile = GetFromOneFile(fileName, excelApp);
                output.AddRange(stationsFromFile);
                FileCompleted?.Invoke();
            }
            excelApp.Quit();
            return output;
        }

        private List<NetPS> GetFromOneFile(string fileName, Excel.Application excelApp)
        {
            var listNetPS = new List<NetPS>();

            var wb = excelApp.Workbooks.Open(Filename: fileName,
                ReadOnly: true);
            var sheet = wb.Sheets[1];

            var firstHeader = sheet.Cells.Find("Наименование и номер ПС");
            Excel.Range currenHeader = firstHeader;

            do
            {
                TextDataPS tdPS = TextExporterFromExcel.GetTextDataPsFromExcel(currenHeader);
                var netPs = new NetPS(tdPS);
                TMtoMGMT(netPs);//мигрируем сети из TM в MGMT, оставляя только сети с 29 префиксом.
                listNetPS.Add(netPs);
                currenHeader = sheet.Cells.FindNext(currenHeader);
            } while (currenHeader.Address != firstHeader.Address);

            wb.Close();
            return listNetPS;
        }

        private void TMtoMGMT(NetPS netPs)
        {
            var TMsubnets = netPs.AllSubnets.Where(net => net.GroupNet == SubnetAddress.Group.TM);
            foreach (var subnet in TMsubnets)
            {
                if (subnet.Prefix != 29)
                    subnet.GroupNet = SubnetAddress.Group.MGMT;
            }
}
    }
}
