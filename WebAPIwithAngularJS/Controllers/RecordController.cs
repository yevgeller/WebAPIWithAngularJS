using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPIwithAngularJS.Models;

namespace WebAPIwithAngularJS.Controllers
{
    public class RecordController : ApiController
    {
        IRecordRepository repo = RecordRepository.getRepository();

        public IEnumerable<Record> GetAllRecords()
        {
            return repo.GetAll();
        }

        public Record GetRecord(int id)
        {
            return repo.Get(id);
        }

        [HttpPost]
        public HttpResponseMessage CreateRecord(Record item)
        {
            var commandType = Request.Headers.GetValues("CommandType").FirstOrDefault();
            if (commandType != "AddRecord")
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            item = repo.Add(item);
            var response = Request.CreateResponse<Record>(HttpStatusCode.Created, item);
            string uri = Url.Link("DefaultApi", new { id = item.RecordId });
            response.Headers.Location = new Uri(uri);

            return response;
            //HttpContent.Current.Response.AddHeader("Location", "api/record/" + item.RecordId);//does not work
        }

        [HttpPut]
        public void UpdateRecord(Record item)
        {
            var commandType = Request.Headers.GetValues("CommandType").FirstOrDefault();
            if (commandType != "UpdateRecord")
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            if (!repo.Update(item))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        public void DeleteRecord(int id)
        {
            var commandType = Request.Headers.GetValues("CommandType").FirstOrDefault();
            if (commandType != "DeleteRecord")
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            repo.Remove(id);
        }
    }
}