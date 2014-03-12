using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIwithAngularJS.Models
{
    public class RecordRepository : IRecordRepository
    {
        private static WebApiDataLayer webapidata = new WebApiDataLayer();
        private static RecordRepository repo = new RecordRepository();

        public static IRecordRepository getRepository()
        {
            return repo;
        }

        public IEnumerable<Record> GetAll()
        {
            return webapidata.GetData();
        }

        public Record Get(int id)
        {
            return webapidata.GetData().Where(x => x.RecordId == id).FirstOrDefault();
        }

        public IEnumerable<Record> GetAllByCategory(string cat)
        {
            return repo.GetAll().Where(x => x.RecordCategory == cat);
        }

        public Record Add(Record item)
        {
            item.RecordId = webapidata.GetData().Count + 1;
            webapidata.Add(item);
            return item;
        }

        public void Remove(int id)
        {
            Record item = this.Get(id);
            if (item == null)
                throw new Exception("something happened");

            webapidata.Remove(item);
        }

        public bool Update(Record item)
        {
            Record storedItem = Get(item.RecordId);
            if (storedItem != null)
            {
                storedItem.RecordCategory = item.RecordCategory;
                storedItem.RecordDate = item.RecordDate;
                storedItem.RecordValue = item.RecordValue;
                return true;
            }

            return false;
        }
    }
}