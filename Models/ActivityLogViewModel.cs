using CarDealer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealer.Models
{
    public class ActivityLogViewModel
    {
        public List<ActivityLog> ActivityLogs { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
    }
}
