using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIwithAngularJS.Models
{
    public class WebApiDataLayer
    {
        private readonly List<Record> originalData = new List<Record> {
            new Record{RecordId=1, RecordCategory="one", RecordDate=DateTime.Now, RecordValue="1"},
            new Record{RecordId=2, RecordCategory="two", RecordDate=DateTime.Now.AddDays(-1), RecordValue="2"},
            new Record{RecordId=3, RecordCategory="one", RecordDate=DateTime.Now.AddDays(-2), RecordValue="3"},
            new Record{RecordId=4, RecordCategory="two", RecordDate=DateTime.Now.AddDays(-3), RecordValue="4"},
            new Record{RecordId=5, RecordCategory="one", RecordDate=DateTime.Now.AddDays(-4), RecordValue="5"}
        };

        private List<Record> data = new List<Record>();

        public List<Record> GetData()
        {
            if (!data.Any())
            {
                ResetData();
            }

            if (data.Count > 12)
            {
                ResetData();
            }

            return data;
        }

        public void Add(Record item)
        {
            data.Add(item);
        }

        public void Remove(Record item)
        {
            data.Remove(item);
        }

        private void ResetData()
        {
            data.Clear();
            data.AddRange(originalData);
        }

    }
}