﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageRoles.Models
{
    public class VW_OrderPackingList
    {
        public long ID { get; set; }
        public string BuyerOrderNumberName { get; set; }
        public DateTime? PackingList { get; set; }
        public DateTime? TodayDate { get; set; }
        public int? RemainingDays { get; set; }
    }
}
