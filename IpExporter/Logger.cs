﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordPlanIP;
using IpLibrary;
using System.Windows;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace IpExporter
{
    public  class Logger
    {
        private List<NetPS> stations;
        public Logger(IExporterStations exporter)
        {
            stations = exporter.Stations;
        }
        public  string GetInformation()
        {

            var sb = new StringBuilder();
            foreach (var netPS in stations)
            {
                sb.AppendLine(netPS.Name + " " + netPS.TotalNet);

                if (netPS.NotOwnedSubnets.Count > 0)
                {
                    PrintNotOwnedSB(netPS, stations, sb);
                }

                if (netPS.CrossedSubnets.Count > 0)
                {
                    PrintCrossedSB(netPS, sb); 
                }

                if(netPS.CrossedSubnets.Count==0 && netPS.NotOwnedSubnets.Count==0)
                    sb.AppendLine("  Все подсети корректны");

                sb.AppendLine();
            }
            return sb.ToString();
        }

        public void GetInformation(RichTextBox rich)
        {
            var flowDoc = new FlowDocument();
            foreach (var netPS in stations)
            {
                flowDoc.Blocks.Add(new Paragraph(new Bold(new Run(netPS.Name + "\n" + netPS.TotalNet))));

                if (netPS.NotOwnedSubnets.Count > 0)
                {
                    PrintNotOwnedRich(netPS, stations, flowDoc);
                }

                if (netPS.CrossedSubnets.Count > 0)
                {
                    PrintCrossedRich(netPS, flowDoc);
                }

                if (netPS.CrossedSubnets.Count == 0 && netPS.NotOwnedSubnets.Count == 0)
                    flowDoc.Blocks.Add(new Paragraph(new Run("Все подсети корректны"){
                        Foreground = new SolidColorBrush(System.Windows.Media.Colors.Green)}));

                rich.Document = flowDoc;
            }
        }

        private void PrintNotOwnedSB(NetPS netPS, List<NetPS> listNetPS, StringBuilder sb)
        {

                sb.AppendLine("  Не принадлежащие подсети");
                foreach (var subnet in netPS.NotOwnedSubnets)
                {
                    string s = $"\t{subnet}[{subnet.GroupNet}] не входит ни в одну сеть";
                    foreach (var PS in listNetPS)
                    {
                        if (NetAddress.IsAffiliation(subnet, PS.TotalNet)
                            && subnet.Prefix > PS.TotalNet.Prefix)
                        {
                            s = $"\t{subnet}[{subnet.GroupNet}] входит в сеть {PS.Name}({PS.TotalNet}";
                            break;
                        }
                    }
                    sb.AppendLine(s);
                }
            }
        private void PrintNotOwnedRich(NetPS netPS, List<NetPS> listNetPS, FlowDocument flowDoc)
        {
            var paragraph = new Paragraph() {
                Foreground = new SolidColorBrush(System.Windows.Media.Colors.DarkRed)
            };
            paragraph.Inlines.Add("  Не принадлежащие подсети\n") ;
            foreach (var subnet in netPS.NotOwnedSubnets)
            {
                string s = $"\t{subnet}[{subnet.GroupNet}] не входит ни в одну сеть";
                foreach (var PS in listNetPS)
                {
                    if (NetAddress.IsAffiliation(subnet, PS.TotalNet)
                        && subnet.Prefix > PS.TotalNet.Prefix)
                    {
                        s = $"\t{subnet}[{subnet.GroupNet}] входит в сеть {PS.Name}({PS.TotalNet}";
                        break;
                    }
                }
                paragraph.Inlines.Add(s+'\n');
            }
            flowDoc.Blocks.Add(paragraph);
        }

        private void PrintCrossedSB(NetPS netPS, StringBuilder sb)
        {
            sb.AppendLine("  Пересекающиеся внутри подсети");
            sb.AppendLine("\t" + string.Join($"{Environment.NewLine}\t", netPS.CrossedSubnets));
        }

        private void PrintCrossedRich(NetPS netPS, FlowDocument flowDoc)
        {
            var paragraph = new Paragraph()
            {
                Foreground = new SolidColorBrush(System.Windows.Media.Colors.OrangeRed)
            };
             paragraph.Inlines.Add("  Пересекающиеся внутри подсети\n");
            paragraph.Inlines.Add("\t" + string.Join($"{Environment.NewLine}\t", netPS.CrossedSubnets));
            flowDoc.Blocks.Add(paragraph);
        }

    }
}
