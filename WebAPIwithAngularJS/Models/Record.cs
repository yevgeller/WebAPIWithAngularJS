using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIwithAngularJS.Models
{
    public class Record
    {
        public int RecordId { get; set; }
        public DateTime RecordDate { get; set; }
        public string RecordValue { get; set; }
        public string RecordCategory { get; set; }
    }
}