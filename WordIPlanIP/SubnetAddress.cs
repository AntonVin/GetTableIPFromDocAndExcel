using IpLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordPlanIP
{
    public class SubnetAddress : NetAddress
    {
        public Group GroupNet { get;}
        public enum Group
        {
            TM,
            MGMT,
            CRAP, 
            ASKUE,
            Control,
            VoIP,
            KISU,
            Video,
            PA, 
            Monitoring,
            ASU
        }
        public SubnetAddress(string addressIp,Group group) : base(addressIp)
        {
            this.GroupNet = group;
        }
    }
}
