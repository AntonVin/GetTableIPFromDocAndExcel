using IpLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WordPlanIP
{
    public class NetPS
    {
        public string Name { get; }
        public List<SubnetAddress> AllSubnets { get; }
        public List<SubnetAddress> CorrectSubnets { get; }
        public List<SubnetAddress> CrossedSubnets { get; }//пересечённые сети
        public List<SubnetAddress> NotOwnedSubnets { get; }//не входят в адрес сети подстанции

        public NetAddress TotalNet { get; }

        public NetPS(TextDataPS textDataPS)
        {
            this.Name = textDataPS.NamePS;
            this.TotalNet = new NetAddress(textDataPS.TotalNet);

            this.AllSubnets = GetAllSubnets(textDataPS);
            this.CrossedSubnets = GetCrossedSubnets(this.AllSubnets);
            this.NotOwnedSubnets = GetNotOwnedSubnets(this.AllSubnets);
            this.CorrectSubnets = GetCorrectSubnets(this.AllSubnets, this.CrossedSubnets,this.NotOwnedSubnets);
        }

        private List<SubnetAddress> GetCorrectSubnets(
            List<SubnetAddress> allNets,
            List<SubnetAddress> crossedNets,
            List<SubnetAddress> notOwnedNets
            ) =>
            allNets.
                Except(crossedNets).
                Except(notOwnedNets).
                ToList();

        private List<SubnetAddress> GetNotOwnedSubnets(List<SubnetAddress> nets)=>
            nets.
                Where(net =>
                    !SubnetAddress.IsAffiliation(this.TotalNet, net) ||
                    net.Prefix < this.TotalNet.Prefix
                ).
                ToList();

        private List<SubnetAddress> GetCrossedSubnets(List<SubnetAddress> nets)
        {
            var crossedNets = new List<SubnetAddress>();
            var projectionCrosNets = new bool[nets.Count()];  //проекция пересекающихся сетей
            for (int i = 0; i < projectionCrosNets.Length; i++)
            {
                if (projectionCrosNets[i] == true)
                    continue;
                for (int j = i + 1; j < nets.Count; j++)
                {
                    if (projectionCrosNets[j] == true)
                        continue;
                    if (SubnetAddress.IsAffiliation(nets[i], nets[j]))
                    {
                        if(projectionCrosNets[i]==false) 
                            projectionCrosNets[i] = true;
                        projectionCrosNets[j] = true;
                    }
                }
            }
            crossedNets = nets.Zip(projectionCrosNets).
                Where(x => x.Second == true).
                Select(x=>x.First).
                ToList();
            return crossedNets;
        }
        private List<SubnetAddress> GetAllSubnets(TextDataPS tdPS)
        {
            var subnetsTM = ParseNets(tdPS.TM, SubnetAddress.Group.TM);
            var totalsubnetsMGMT = ParseNets(tdPS.MGMT, SubnetAddress.Group.MGMT);
                totalsubnetsMGMT = GetOnlyTotalSubnets(totalsubnetsMGMT);// убираем дубликаты и вложенные подсети
            var subnetsCRAP = ParseNets(tdPS.CRAP, SubnetAddress.Group.CRAP);
            var subnetsASKUE = ParseNets(tdPS.ASKUE, SubnetAddress.Group.ASKUE);
            var subnetsControl = ParseNets(tdPS.Control, SubnetAddress.Group.Control);
            var subnetsVoIP = ParseNets(tdPS.VoIP, SubnetAddress.Group.VoIP);
            var subnetsKISU = ParseNets(tdPS.KISU, SubnetAddress.Group.KISU);
            var subnetsVideo = ParseNets(tdPS.Video, SubnetAddress.Group.Video);
            var subnetsPA = ParseNets(tdPS.PA, SubnetAddress.Group.PA);
            var subnetsMonitoring = ParseNets(tdPS.Monitoring, SubnetAddress.Group.Monitoring);
            var subnetsASU = ParseNets(tdPS.ASU, SubnetAddress.Group.ASU);

            var groups = new List<List<SubnetAddress>>() { subnetsTM,totalsubnetsMGMT,subnetsCRAP, subnetsASKUE, subnetsControl, subnetsVoIP,
                subnetsKISU,subnetsVideo,subnetsPA,subnetsMonitoring, subnetsASU};
            var allSubnets = groups.
                SelectMany(x => x).
                ToList();

            return allSubnets;
        }

        private List<SubnetAddress> GetOnlyTotalSubnets(List<SubnetAddress> subnetsMGMT)
        {
            for (int i = 0; i < subnetsMGMT.Count; i++)
            {
                var net1 = subnetsMGMT[i];
                if (net1 == null)
                    continue;
                for (int j = i + 1; j < subnetsMGMT.Count(); j++)
                {
                    if (subnetsMGMT[j] == null)
                        continue;
                    if (SubnetAddress.IsAffiliation(net1, subnetsMGMT[j]))
                    {
                        if (net1.Prefix > subnetsMGMT[j].Prefix)
                            net1 = subnetsMGMT[j];
                        subnetsMGMT[i] = null;
                    }
                }
            }
            return subnetsMGMT.Where(net => net is not null).ToList();
        }

        private List<SubnetAddress> ParseNets(string text, SubnetAddress.Group group)
        {
            var output = new List<SubnetAddress>();

            // учитывая ошибку с двумя точками между 3им и 4ым октетом в некоторых ip -> 10.146.216..34
            var reg = new Regex(@"\d+\.\d+\.\d+\.{1,2}\d+(\/\d+)?");

            var matches = reg.Matches(text);
            foreach (Match match in matches)
            {
                string strSubnet = match.Value;
                strSubnet = strSubnet.Replace("..", "."); // убираем ошибочные точки между 3м и 4м октетом
                if (!strSubnet.Contains('/'))
                    strSubnet += "/32";
                try
                {
                    var subnet = new SubnetAddress(strSubnet, group);
                    output.Add(subnet);
                }
                catch (ArgumentException ex)
                {
                    //можно будет потом вести лог
                }
            }
            return output;
        }

    }
}

