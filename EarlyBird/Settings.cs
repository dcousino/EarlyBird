using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EarlyBird
{
    public class Settings
    {
        public string ClubId { get; set; }
        public string WorkOutEventId { get; set; } = "5af78242ec2245e1a0cdd96707b67692";
        public IDictionary<string, string> PassWords { get; set; }

       
    }
}
