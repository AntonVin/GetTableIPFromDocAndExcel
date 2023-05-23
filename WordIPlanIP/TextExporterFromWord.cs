using System.Collections.Generic;
using System.Runtime.InteropServices;
using word = Microsoft.Office.Interop.Word;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Word;

namespace PlanIP
{
    public class TextExporterFromWord
    {

       public static TextDataPS GetTextDataPsFromDoc(string pathDoc)
        {
            var app = new word.Application();
            word.Document doc = app.Documents.Open(FileName: pathDoc, 
                ReadOnly:true);
            word.Table table = doc.Tables[1];

            app.Visible = false;

            string NamePs = GetTextFromCell(doc, table.Cell(4, 2));
            string TotalNet = GetTotalNet(doc, table.Cell(4, 3), table.Cell(2, 3));
            string TM = GetTextFromCell(doc, table.Cell(4, 4));
            string MGMT = GetTextFromCell(doc, table.Cell(4, 5));
            string CRAP = GetTextFromCell(doc, table.Cell(4, 6));
            string ASKUE = GetTextFromCell(doc, table.Cell(4, 7));
            string Control = GetTextFromCell(doc, table.Cell(4, 8));
            string VoIP = GetTextFromCell(doc, table.Cell(4, 9));
            string KISU = GetTextFromCell(doc, table.Cell(4, 10));
            string VIDEO = GetTextFromCell(doc, table.Cell(4, 11));
            string PA = GetTextFromCell(doc, table.Cell(4, 12));
            string Monitoring = GetTextFromCell(doc, table.Cell(4, 13));
            string ASU = GetTextFromCell(doc, table.Cell(4, 14));

            doc.Close(false);
            app.Quit();
            return new TextDataPS(NamePs, TotalNet, TM, MGMT, CRAP, ASKUE, Control, VoIP,
                    KISU, VIDEO, PA, Monitoring, ASU);
        }
          
        private static string GetTotalNet(word.Document doc, word.Cell cellNet,word.Cell cellPrefix)
        {
            string net = GetTextFromCell(doc, cellNet);
            string prefix = new Regex(@"/(\d+)")
                .Match(GetTextFromCell(doc, cellPrefix)).
                Groups[1].Value;
            return net + "/" + prefix;
        }

        private static  string GetTextFromCell(word.Document doc, word.Cell cell)
        {
            word.Range range = doc.Range(cell.Range.Start, cell.Range.End - 1);
            return range.Text.Trim();
        }
    }
}