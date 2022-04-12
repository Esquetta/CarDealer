using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealer.Entities
{
    public class ActivityLog:IEntity
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string IpAdress { get; set; }
        public string Url { get; set; }
        public string Data { get; set; }
        public string Browser { get; set; }
        public DateTime ActivityTime { get; set; } = DateTime.Now;
    }
}
