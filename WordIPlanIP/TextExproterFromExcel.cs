using Excel = Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PlanIP
{
    public class TextExporterFromExcel
    {
        public static TextDataPS GetTextDataPsFromExcel(Excel.Range headerNameAndNumber)
        {
            string NamePs = GetTextFromCell(headerNameAndNumber, 2, 0);
            string TotalNet = GetTotalNet(headerNameAndNumber);
            string TM = GetTextFromCell(headerNameAndNumber,2, 2);
            string MGMT = ""; // объединён с TM
            string CRAP = GetTextFromCell(headerNameAndNumber, 2, 3);
            string ASKUE = GetTextFromCell(headerNameAndNumber, 2, 4);
            string Control = GetTextFromCell(headerNameAndNumber, 2, 5);
            string VoIP = GetTextFromCell(headerNameAndNumber, 2, 6);
            string KISU = GetTextFromCell(headerNameAndNumber, 2, 7);
            string VIDEO = GetTextFromCell(headerNameAndNumber, 2, 8);
            string PA = GetTextFromCell(headerNameAndNumber, 2, 9);
            string Monitoring = GetTextFromCell(headerNameAndNumber, 2, 10);
            string ASU = GetTextFromCell(headerNameAndNumber, 2, 11);

            return new TextDataPS(NamePs, TotalNet, TM, MGMT, CRAP, ASKUE, Control, VoIP,
                KISU, VIDEO, PA, Monitoring, ASU);

        }
        private static string GetTotalNet(Excel.Range headerNameAndNumber)
        {
            string net = GetTextFromCell(headerNameAndNumber, 2, 1);
            string prefix = new Regex(@"/\d+")
                .Match(headerNameAndNumber.Offset[0, 1].Offset[1, 0].Value2).Value;
            return net + prefix;
        }

        // ищем ячейку относительно заголовка "Наименование и номер ПС"
        private static string GetTextFromCell(Excel.Range headerNameAndNumber, int rowOffset, int columnOffset)
        {
            var text = headerNameAndNumber.Offset[rowOffset, columnOffset].Value2.Trim();
            return text;
        }
    }
}
